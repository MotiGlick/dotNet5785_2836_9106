
using Helpers;
namespace BO;


/// <summary>
/// Represents a volunteer in a  list view, including personal details and performance statistics.
/// </summary>
public class VolunteerInList
{
    /// <summary>
    /// Gets the  id of the volunteer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the name of the volunteer.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets a value indicating whether the volunteer is currently active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets the total number of calls the volunteer has successfully handled.
    /// </summary>
    public int TotalHandledCalls { get; init; }

    /// <summary>
    /// Gets the total number of calls the volunteer has canceled.
    /// </summary>
    public int TotalCanceledCalls { get; init; }

    /// <summary>
    /// Gets the total number of calls that have expired for the volunteer.
    /// </summary>
    public int TotalExpiredCalls { get; init; }


    /// <summary>
    /// Gets the ID of the call currently being handled by the volunteer, if any.
    /// </summary>
    public int? CurrentCallId { get; init; }


    /// <summary>
    /// Gets the type of the call currently being handled by the volunteer.
    /// </summary>
    public CallType CurrentCallType { get; init; }

    /// <summary>
    /// Returns a string that represents the current object, including its properties.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString() => this.ToStringProperty();
}


   
   