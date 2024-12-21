
using Helpers;

namespace BO;



/// <summary>
/// Represents a call in the bl with information such as the call's ID, type, description,
/// location (address, latitude, and longitude), opening time, max time, status, and assignments list.
/// </summary>
public class Call
{

    /// <summary>
    /// Gets the unique identifier of the call.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the type of the call.
    /// </summary>
    public CallType CallType { get; init; }



    /// <summary>
    /// Gets the description of the call (can be null).
    /// </summary>
    public string? Description { get; init; }


    /// <summary>
    /// Gets or sets the address of the call.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the call's location.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the call's location.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Gets the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; init; }

    /// <summary>
    /// Gets or sets the maximum time allowed for the call (nullable).
    /// </summary>
    public DateTime? MaxTime { get; set; }

    /// <summary>
    /// Gets or sets the status of the call (e.g., open, in progress, closed).
    /// </summary>
    public StatusOfCall status { get; set; }


    /// <summary>
    /// Gets or sets the list of assignments related to the call (nullable).
    /// </summary>
    public List<BO.CallAssignInList>? CallAssignsList { get; set; }//???->init

    /// <summary>
    /// Returns a string representation of the call with its properties.
    /// </summary>
    /// <returns>A string representation of the call.</returns>
    public override string ToString() => this.ToStringProperty();


}
