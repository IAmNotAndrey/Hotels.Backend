namespace Hotels.Application.Interfaces.Services;

public interface IPaymentService<TPaymentKey> where TPaymentKey : notnull
{
    Task<TPaymentKey> CreatePaymentAsync(decimal amount);
    Task CapturePaymentAsync(TPaymentKey id);
}
