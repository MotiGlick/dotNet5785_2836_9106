

using Helpers;

namespace BO;

/// <summary>
/// Represents a call assignment in a list, including details about the volunteer, 
/// handling start and end times, and closure type.
/// </summary>
public class CallAssignInList
{

    /// <summary>
    /// Gets the ID of the volunteer assigned to the call. 
    /// This value can be null if no volunteer is assigned.
    /// </summary>
    public int? VolunteerId { get; init; }


    /// <summary>
    /// Gets the name of the volunteer assigned to the call.
    /// This value can be null if no volunteer is assigned.
    /// </summary>
    public string? VolunteerName { get; init; }


    /// <summary>
    /// Gets the start time of handling the call.
    /// </summary>
    public DateTime HndalStartTime { get; init; }


    /// <summary>
    /// Gets the end time of handling the call, if available.
    /// </summary>
    public DateTime? HndalEndTime { get; init; }


    /// <summary>
    /// Gets the type of closure for the call, if available.
    /// </summary>
    public CallClosureType? ClosureType  { get; init; }


    /// <summary>
    /// Returns a string that represents the current object, including its properties.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString() => this.ToStringProperty();

}
