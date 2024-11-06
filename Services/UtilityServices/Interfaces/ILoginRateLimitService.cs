namespace elite_shop.Services.UtilityServices.Interfaces;

public interface ILoginRateLimitService
{
    Task<bool> IsLockedOutAsync(string username);
    Task RecordFailedAttemptAsync(string username);
    Task ResetAttemptsAsync(string username);
}
