

using DO;
using Helpers;
using Microsoft.VisualBasic.FileIO;
using System.Net;

namespace BO;





/// <summary>
/// Represents a call in a list, including its details such as ID, type, timing, 
/// status, and volunteer-related information.
/// </summary>
public class CallInList 
{

    /// <summary>
    /// Gets the Id of the assignment in the list. 
    /// This value can be null if the ID is not assigned.
    /// </summary>
    public int? Id { get; init; }


    /// <summary>
    /// Gets the unique identifier of the call.
    /// </summary>
    public int CallId { get; init; }


    /// <summary>
    /// Gets the type of the call.
    /// </summary>
    public CallType CallType { get; init; }



    /// <summary>
    /// Gets the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; init; }

    /// <summary>
    /// Gets the time left for handling the call, if applicable.
    /// </summary>
    public TimeSpan? TimeLeft { get; init; }


    /// <summary>
    /// Gets or sets the name of the last volunteer who handled the call.
    /// This value can be null if no volunteer has handled the call.
    /// </summary>
    public string? LastVolunteerName { get; set; }

    /// <summary>
    /// Gets the total handling time of the call, if available.
    /// </summary>
    public TimeSpan? HandlingTime { get; init; }

    /// <summary>
    /// Gets the current status of the call.(Open, Closed...)
    /// </summary>
    public StatusOfCall status { get; init; }

    /// <summary>
    /// Gets the total number of assignments made for the call.
    /// </summary>
    public int AmountOfAssigns { get; init; }

    public override string ToString() => this.ToStringProperty();
}
