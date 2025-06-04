namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class BookingController : ApplicationControllerBase<Booking, Guid, BookingDto, BookingDtoB>
{
    private readonly IPaymentService<string> _paymentService;
    private readonly IMapper _mapper;
    private readonly IApplicationUserService _appUserService;
    private readonly ISubobjectService _subobjectService;
    private readonly IBookingService _bookingService;
    private readonly IBookingRepo _bookingRepo;
    private readonly IGenericRepo<Subobject, Guid> _genSubobjectRepo;

    public BookingController(IPaymentService<string> yooKassaService,
                             IMapper mapper,
                             IApplicationUserService appUserService,
                             ISubobjectService subobjectService,
                             IBookingService bookingService,
                             IGenericRepo<Subobject, Guid> genSubobjectRepo,
                             IBookingRepo bookingRepo,
                             IGenericRepo<Booking, Guid> repo
                             ) : base(repo)
    {
        _paymentService = yooKassaService;
        _mapper = mapper;
        _appUserService = appUserService;
        _subobjectService = subobjectService;
        _bookingService = bookingService;
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
        if (!await _appUserService.IsUserAllowedAsync(User, dtoB.TouristId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        // Prohibit booking if there are conflicting dates.
        if (await _bookingService.HasBookingConflictWithSubobjectAsync(dtoB.SubobjectId, dtoB.DateIn, dtoB.DateOut))
        {
            return BadRequest("There are conflicting date periods.");
        }

        Booking booking = _mapper.Map<Booking>(dtoB);
        string paymentId = await _bookingService.BookAsync(booking);

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
        Booking booking = await _bookingRepo.GetByPaymentIdAsync(payment.Id);
        await _repo.DeleteAsync(booking.Id);

        return Ok();
    }

    /// <summary>
    /// Получает все 'Subobjects', у 'Bookings' которых есть пересечения с интервалом (dateIn, dateOut)
    /// </summary>
    [HttpPost("{partnerId}")]
    public async Task<ActionResult<List<SubobjectDto>>> GetBookedSubobjects(string partnerId, [FromForm] DateOnly dateIn, [FromForm] DateOnly dateOut)
    {
        var subobjectDtos = await _bookingService.GetBookedSubobjectDtosByPartnerIdAsync(partnerId, dateIn, dateOut);

        return Ok(subobjectDtos);
    }

    /// <summary>
    /// Получает все 'Bookings', у 'Partner.Subobjects' где есть пересечения с интервалом (dateIn, dateOut)
    /// </summary>
    [HttpPost("{subobjectId}")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetSubobjectsBookings(Guid subobjectId, [FromForm] DateOnly dateIn, [FromForm] DateOnly dateOut)
    {
        var bookingDtos = await _bookingService.GetBookingDtosAsync(subobjectId, dateIn, dateOut);

        return Ok(bookingDtos);
    }
}