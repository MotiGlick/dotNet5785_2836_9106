

 

namespace DO;




/// <summary>
/// Represents a call record with details about the call type, location, time, and other optional information.
/// </summary>
/// <param name="Id">Unique identifier for the call.</param>
/// <param name="type">Type of the call, indicating the purpose (e.g., DeliverFood or makingFood).</param>
/// <param name="Address">The address where the call is located.</param>
/// <param name="Description">Additional description or notes related to the call.</param>
/// <param name="Latitude">Geographical latitude of the call location.</param>
/// <param name="Longitude">Geographical longitude of the call location.</param>
/// <param name="CallTime">The exact time when the call was made.</param>
/// <param name="TotalTime">Total duration from the call time until completion, if applicable.</param>

public record Call
(
    int Id,
    CallType type,
    string CallAddress,
    string? Description,
    double Latitude,
    double Longitude,
    DateTime CallTime,
    DateTime? TotalTime
)
{

    public Call() : this(0, CallType.ProvideTransportation, string.Empty, null, 0.0, 0.0, DateTime.Now, null) { }
}