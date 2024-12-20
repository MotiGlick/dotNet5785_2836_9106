

using DO;
using Helpers;
using Microsoft.VisualBasic.FileIO;
using System.Net;

namespace BO;






public class CallInList 
{
    public int? Id { get; init; }

    public int CallId { get; init; }

    public CallType CallType { get; init; }

    public DateTime OpeningTime { get; init; }

    public TimeSpan? TimeLeft { get; init; }

    public string? LastVolunteerName { get; set; }

    public TimeSpan? HandlingTime { get; init; }

    public StatusOfCall status { get; init; }

    public int AmountOfAssigns { get; init; }

    public override string ToString() => this.ToStringProperty();
}
