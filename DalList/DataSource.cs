namespace Dal;

/// <summary>
/// Represents the data source for storing application entities in memory.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// Gets the list of Volunteer entities.
    /// </summary>
    internal static List<DO.Volunteer> Volunteers { get; } = new();

    /// <summary>
    /// Gets the list of Call entities.
    /// </summary>
    internal static List<DO.Call> Calls { get; } = new();

    /// <summary>
    /// Gets the list of Assignment entities.
    /// </summary>
    internal static List<DO.Assignment> Assignments { get; } = new();
}

