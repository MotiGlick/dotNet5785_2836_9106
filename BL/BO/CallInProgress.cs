
using DO;
using Helpers;
using System.Data;

namespace BO;


/// <summary>
/// Represents a call in progress, including details such as call type, description, 
/// address, timing, and volunteer-related information.
/// </summary>
public class CallInProgress
{

    /// <summary>
    /// Gets the Id of the assignment for the call.
    /// </summary>
    public int Id { get; init; }//AssignmentId

    /// <summary>
    /// Gets the Id of the call.
    /// </summary>
    public int CallId { get; init; }

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
    /// Gets the time when the handling of the call started.
    /// </summary>
    public DateTime HndalStartTime { get; init; }

    /// <summary>
    /// Gets the distance of the volunteer from the call's location.
    /// </summary>
    public double DistanceFromVolunteer { get; init; }

    /// <summary>
    /// Gets or sets the current status of the call.
    /// </summary>
    public StatusOfCall status { get; set; }

    /// <summary>
    /// Returns a string that represents the current object, including its properties.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString() => this.ToStringProperty();

}
