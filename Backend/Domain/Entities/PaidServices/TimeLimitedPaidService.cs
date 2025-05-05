namespace Hotels.Domain.Entities.PaidServices;

public abstract class TimeLimitedPaidService : PaidService
{
    /// <summary>
    /// Длительность действия
    /// </summary>
    public TimeSpan Duration { get; set; }
}
