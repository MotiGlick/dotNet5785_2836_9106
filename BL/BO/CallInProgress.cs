
using DO;
using System.Data;

namespace BO;

public class CallInProgress
{

    public int Id { get; init; }//AssignmentId

    public int CallId { get; init; }

    public CallType CallType { get; init; }

    public string? Description { get; init; }

    public string Address { get; init; }

    public DateTime OpeningTime { get; init; }

    public DateTime? MaxTime { get; init; }

    public DateTime HndalStartTime { get; init; }

    public double DistanceFromVolunteer { get; init; }

    public StatusOfCall status { get; set; }


}
