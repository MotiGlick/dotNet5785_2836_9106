using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// Internal configuration class with identifiers, clock, and risk range.
/// </summary>
internal static class Config
{
    internal const int startNextCallId = 0;  // Initial call ID

    private static int nextCallId = startNextCallId;  // Current call ID
    internal static int NextCallId
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => nextCallId++; }  // Returns and increments the call ID

    internal const int startnextAssignmentId = 10;  // Initial assignment ID

    private static int nextAssignmentId = startnextAssignmentId;  // Current assignment ID
    internal static int NextAssignmentId 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => nextAssignmentId++; }  // Returns and increments the assignment ID

    internal static DateTime Clock 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; } = DateTime.Now;  // Current system time


    internal static TimeSpan RiskRange {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = TimeSpan.FromHours(1);  // Risk range (default: 1 hour)

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    internal static void Reset()
    {
        // Resets IDs and clock to their initial values
        nextCallId = startNextCallId;
        nextAssignmentId = startnextAssignmentId;
        Clock = DateTime.Now;
    }
}