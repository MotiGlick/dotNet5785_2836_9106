 namespace Dal;
using DalApi;

/// <summary>
/// Implementation of the IConfig interface for managing application configuration.
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Gets or sets the global clock value.
    /// </summary>
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    ///// <summary>
    ///// Gets the next unique ID for Call entities.
    ///// </summary>
    //public int NextCallId
    //{
    //    get => Config.NextCallId;
    //}

    ///// <summary>
    ///// Gets the next unique ID for Assignment entities.
    ///// </summary>
    //public int NextAssignmentId
    //{
    //    get => Config.NextAssignmentId;
    //}

    /// <summary>
    /// Resets the configuration to its default state.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }

    /// <summary>
    /// Gets or sets the time span representing the risk range.
    /// </summary>
    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }
}
