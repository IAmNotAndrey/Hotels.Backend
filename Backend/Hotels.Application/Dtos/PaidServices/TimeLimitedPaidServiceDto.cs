namespace Hotels.Application.Dtos.PaidServices;

public class TimeLimitedPaidServiceDto : PaidServiceDto
{
    public TimeSpan Duration { get; set; }
}
