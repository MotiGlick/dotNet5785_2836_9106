
namespace BO;

public class VolunteerInList
{

    public int Id { get; init; }

    public string Name { get; init; }

    public bool IsActive { get; init; }

    public int TotalHandledCalls { get; init; }


    public int TotalCanceledCalls { get; init; }


    public int TotalExpiredCalls { get; init; }


    public int? CurrentCallId { get; init; }


    public TypeOfCall CurrentCallType { get; init; }
}



