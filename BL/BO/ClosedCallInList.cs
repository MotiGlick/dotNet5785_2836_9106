

using Helpers;

namespace BO;

public class ClosedCallInList
{

    public int Id { get; init; }
    public CallType CallType { get; init; }
    public string Address { get; init; }
    public DateTime OpeningTime { get; init; }
    public DateTime HndalStartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public CallClosureType ClosureType { get; init; }

    public override string ToString() => this.ToStringProperty();

}
