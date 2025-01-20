  

using Helpers;

namespace BO;

/// <summary>
/// Represents an open call in a list, including details about the call type, 
/// description, address, timing, and distance from a volunteer.
/// </summary>
public class OpenCallInList
{

    /// <summary>
    /// Gets the  id of the open call.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the type of the call.
    /// </summary>
    public CallType CallType { get; init; }

    /// <summary>
    /// Gets the description of the call, if available.
    /// </summary>

    public string? Description { get; init; }

    /// <summary>
    /// Gets the address associated with the call.
    /// </summary>
    public string Address { get; init; }

    /// <summary>
    /// Gets the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; init; }

    /// <summary>
    /// Gets the maximum allowed time for handling the call, if applicable.
    /// </summary>
    public DateTime? MaxTime { get; init; }

    /// <summary>
    /// Gets the distance from the volunteer to the call's location.
    /// </summary>
    public double DistanceFromVolunteer { get; init; }

    /// <summary>
    /// Returns a string that represents the current object, including its properties.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString() => this.ToStringProperty();

}
