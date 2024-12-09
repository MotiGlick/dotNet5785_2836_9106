

using Microsoft.VisualBasic.FileIO;

namespace BO;

public class CallInList
{
    public int? Id { get; init; }

    public int CallId { get; init; }

    public TypeOfCall CallType { get; init; }

    public DateTime OpeningTime { get; init; }

    public TimeSpan? TimeLeft { get; init; }

    public string? LastVolunteerName { get; init; }

    public TimeSpan? HandlingTime { get; init; }

    public StatusOfCall status { get; init; }

    public int AmountOfAssigns { get; init; }





}
