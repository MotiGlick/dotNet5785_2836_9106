namespace Dal;

/// <summary>
/// Internal configuration class with identifiers, clock, and risk range.
/// </summary>
internal static class Config
{
    internal const int startNextCallId = 0;  // Initial call ID

    private static int nextCallId = startNextCallId;  // Current call ID
    internal static int NextCallId { get => nextCallId++; }  // Returns and increments the call ID

    internal const int startnextAssignmentId = 10;  // Initial assignment ID

    private static int nextAssignmentId = startnextAssignmentId;  // Current assignment ID
    internal static int NextAssignmentId { get => nextAssignmentId++; }  // Returns and increments the assignment ID

    internal static DateTime Clock { get; set; } = DateTime.Now;  // Current system time

    internal static TimeSpan RiskRange { get; set; } = TimeSpan.FromHours(1);  // Risk range (default: 1 hour)

    internal static void Reset()
    {
        // Resets IDs and clock to their initial values
        nextCallId = startNextCallId;
        nextAssignmentId = startnextAssignmentId;
        Clock = DateTime.Now;
    }
}