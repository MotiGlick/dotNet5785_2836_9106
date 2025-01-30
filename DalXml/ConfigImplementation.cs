namespace Dal;
using DalApi;
using System.Runtime.CompilerServices;

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
        //[MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => Config.Clock;
       // [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
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
    //[MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Reset()
    {
        Config.Reset();
    }

    /// <summary>
    /// Gets or sets the time span representing the risk range.
    /// </summary>
    
    public TimeSpan RiskRange
    {
        //[MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => Config.RiskRange;
       // [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => Config.RiskRange = value;
    }
}
