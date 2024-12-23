

using Helpers;

namespace BO;

/// <summary>
/// Represents a closed call in a list, including details about the call type, 
/// address, timing, and closure information.
/// </summary>
public class ClosedCallInList
{
    /// <summary>
    /// Gets the  id of the closed call.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the type of the call.
    /// </summary>
    public CallType CallType { get; init; }

    /// <summary>
    /// Gets the address associated with the call.
    /// </summary>
    public string Address { get; init; }

    /// <summary>
    /// Gets the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; init; }

    /// <summary>
    /// Gets the time when the handling of the call started.
    /// </summary>
    public DateTime HndalStartTime { get; init; }

    /// <summary>
    /// Gets the time when the call was closed, if available.
    /// </summary>
    public DateTime? EndTime { get; init; }

    /// <summary>
    /// Gets the closure type of the call.
    /// </summary>
    public CallClosureType? ClosureType { get; init; }

    /// <summary>
    /// Returns a string that represents the current object, including its properties.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString() => this.ToStringProperty();

}
