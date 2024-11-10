
namespace DO;



/// <summary>
/// Represents an assignment record, linking a volunteer to a call with details about the timing and type of assignment.
/// </summary>
/// <param name="Id">Unique identifier for the assignment.</param>
/// <param name="CallId">Identifier of the associated call.</param>
/// <param name="VolunteerId">Identifier of the assigned volunteer.</param>
/// <param name="CallTime">The time the assignment call was made.</param>
/// <param name="TotalTime">Total duration from the call time until the assignment was completed, if available.</param>
/// <param name="type">Type of the call, indicating the purpose of the assignment (e.g., DeliverFood).</param>
public record Assignment
(
    int Id,
    int CallId,
    int VolunteerId,
    DateTime CallTime,
    DateTime? TotalTime,
    CallType? type


)
{
    public Assignment():this(0,0,0,DateTime.Now,DateTime.Now, CallType.DeliverFood) { }
}
