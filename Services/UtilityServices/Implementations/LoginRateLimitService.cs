using elite_shop.Models.Settings;
using Microsoft.Extensions.Options;

namespace elite_shop.Services.UtilityServices.Implementations;

using elite_shop.Services.UtilityServices.Interfaces;
using StackExchange.Redis;

public class LoginRateLimitService : ILoginRateLimitService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly int _maxAttempts;
    private readonly TimeSpan _lockoutDuration;

    public LoginRateLimitService(IConnectionMultiplexer redis, IOptions<RateLimitSetting> rateLimitSettings)
    {
        _redis = redis;
        _maxAttempts = rateLimitSettings.Value.MaxAttempts;
        _lockoutDuration = TimeSpan.FromMinutes(rateLimitSettings.Value.LockoutDurationMinutes);
    }

    public async Task<bool> IsLockedOutAsync(string email)
    {
        var db = _redis.GetDatabase();
        var attemptCount = await db.StringGetAsync($"login:attempts:{email}");

        // Ensure attemptCount exists and is parsed to an integer; check if it meets or exceeds the max attempt threshold
        return attemptCount.HasValue && int.Parse(attemptCount) >= _maxAttempts;
    }

    public async Task RecordFailedAttemptAsync(string email)
    {
        var db = _redis.GetDatabase();
        var attemptCountKey = $"login:attempts:{email}";

        // Increment attempt count; if it's a new key, it initializes to 1
        var attemptCount = await db.StringIncrementAsync(attemptCountKey);

        // Set expiration only on the first attempt
        if (attemptCount == 1)
        {
            await db.KeyExpireAsync(attemptCountKey, _lockoutDuration);
        }
    }


    public async Task ResetAttemptsAsync(string email)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync($"login:attempts:{email}");
    }
}
