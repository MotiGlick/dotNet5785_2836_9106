

using Helpers;

namespace BO;

public class OpenCallInList
{

    public int Id { get; init; }

    public CallType CallType { get; init; }

    public  string? Description { get; init; }
    public string Address { get; init; }
    public DateTime OpeningTime { get; init; }
    public DateTime? MaxTime { get; init; }

    public double DistanceFromVolunteer { get; init; }

    public override string ToString() => this.ToStringProperty();

}
