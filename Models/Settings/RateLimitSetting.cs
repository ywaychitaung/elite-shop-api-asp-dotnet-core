namespace elite_shop.Models.Settings;

public class RateLimitSetting
{
    public int MaxAttempts { get; set; }
    public int LockoutDurationMinutes { get; set; }
}
