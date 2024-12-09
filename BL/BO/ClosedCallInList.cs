

namespace BO;

public class ClosedCallInList
{

    public int Id { get; init; }
    public TypeOfCall CallType { get; init; }
    public string Address { get; init; }
    public DateTime OpeningTime { get; init; }
    public DateTime HndalStartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public StatusOfCall status { get; init; }

}
