namespace AuthenticationSystem.Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTime => DateTime.UtcNow;
}
