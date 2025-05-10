namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class BookingController : ApplicationControllerBase<Booking, Guid, BookingDto, BookingDtoB>
{
    private readonly ApplicationContext _db;
    private readonly IPaymentService<string> _paymentService;
    private readonly IMapper _mapper;
    private readonly IApplicationUserService _appUserRepo;
    private readonly ISubobjectService _subobjectRepo;
    private readonly IBookingService _bookingRepo;
    private readonly IGenericRepo<Subobject, Guid> _genSubobjectRepo;

    public BookingController(ApplicationContext db,
                             IPaymentService<string> yooKassaService,
                             IMapper mapper,
                             IApplicationUserService appUserRepo,
                             ISubobjectService subobjectRepo,
                             IBookingService bookingRepo,
                             IGenericRepo<Subobject, Guid> genSubobjectRepo,
                             IGenericRepo<Booking, Guid> repo
                             ) : base(repo)
    {
        _db = db;
        _paymentService = yooKassaService;
        _mapper = mapper;
        _appUserRepo = appUserRepo;
        _subobjectRepo = subobjectRepo;
        _bookingRepo = bookingRepo;
        _genSubobjectRepo = genSubobjectRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> Get()
    {
        return await Get_();
    }

    /// <summary>
    /// Booking of a subobject.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<ActionResult<Payment>> Book([FromForm] BookingDtoB dtoB)
    {
        if (!await _genSubobjectRepo.ExistsAsync(dtoB.SubobjectId))
        {
            return NotFound($"{nameof(Subobject)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.TouristId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        // Prohibit booking if there are conflicting dates.
        if (await _subobjectRepo.HasBookingConflictAsync(dtoB.SubobjectId, dtoB.DateIn, dtoB.DateOut))
        {
            return BadRequest("There are conflicting date periods.");
        }

        // Get the total cost of the reservation for the entire booking period.
        decimal totalPrice = await _subobjectRepo.CalculateBookingCostAsync(dtoB.SubobjectId, dtoB.DateIn, dtoB.DateOut);

        // Создаём бронь
        Booking booking = _mapper.Map<Booking>(dtoB);
        // Создаём платёж ЮКасса
        string paymentId = await _paymentService.CreatePaymentAsync(totalPrice); // BUG: не регистрируется платёж
        booking.PaymentId = paymentId;

        // Сохраняем бронь в БД
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return Ok(paymentId);
    }

    /// <summary>
    /// Триггер, который вызывается при изменении статуса оплаты
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> HandlePaymentNotification()
    {
        using var reader = new StreamReader(Request.Body);
        string? body = await reader.ReadToEndAsync();

        // Парсим уведомление
        Notification notification = Client.ParseMessage(Request.Method, Request.ContentType, body);

        if (notification is not PaymentWaitingForCaptureNotification paymentWaitingForCaptureNotification)
        {
            return BadRequest();
        }

        Payment payment = paymentWaitingForCaptureNotification.Object;
        // Если статус платежа "Оплачен"
        if (payment.Paid)
        {
            // Подтверждаем готовность принять платеж
            await _paymentService.CapturePaymentAsync(payment.Id);
            return Ok();
        }

        // Если платёж не был оплачен, то удаляем бронь в БД
        Booking booking = await _db.Bookings.FirstAsync(b => b.PaymentId == payment.Id);
        _db.Bookings.Remove(booking);
        await _db.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Получает все 'Subobjects', у 'Bookings' которых есть пересечения с интервалом (dateIn, dateOut)
    /// </summary>
    [HttpPost("{partnerId}")]
    public async Task<ActionResult<List<SubobjectDto>>> GetBookedSubobjects(string partnerId, [FromForm] DateOnly dateIn, [FromForm] DateOnly dateOut)
    {
        Partner partner = await _db.Partners.FirstAsync(p => p.Id == partnerId);
        List<SubobjectDto> subobjectDtos = partner.Subobjects
            .Where(s => _subobjectRepo.HasBookingConflictAsync(s.Id, dateIn, dateOut).Result)
            .Select(s => _mapper.Map<SubobjectDto>(s))
            .ToList();

        return Ok(subobjectDtos);
    }

    /// <summary>
    /// Получает все 'Bookings', у 'Partner.Subobjects' где есть пересечения с интервалом (dateIn, dateOut)
    /// </summary>
    [HttpPost("{subobjectId}")]
    public async Task<ActionResult<List<BookingDto>>> GetSubobjectsBookings(Guid subobjectId, [FromForm] DateOnly dateIn, DateOnly dateOut)
    {
        Subobject subobject = await _db.Subobjects.FirstAsync(p => p.Id == subobjectId);
        List<BookingDto> bookingDtos = subobject.Bookings
            .Where(b => _bookingRepo.HasBookingConflictAsync(b.Id, dateIn, dateOut).Result)
            .Select(b => _mapper.Map<BookingDto>(b))
            .ToList();

        return Ok(bookingDtos);
    }
}