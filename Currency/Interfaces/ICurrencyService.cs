namespace Currency.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> GetUsdToKztRateAsync(string url);
    }
}
