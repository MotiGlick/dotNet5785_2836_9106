

using Helpers;
using System;

namespace BO;


/// <summary>
/// Represents a volunteer, including personal details, location, and performance statistics.
/// </summary>
public class Volunteer
{
    /// <summary>
    /// Gets the  id of the volunteer.
    /// </summary>
    public int Id { get; init; } 

    /// <summary>
    /// Gets the name of the volunteer.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets or sets the phone number of the volunteer.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the email address of the volunteer.
    /// </summary>
    public string Email { get; set; }


    /// <summary>
    /// Gets or sets the password of the volunteer. This may be null if no password is set.
    /// </summary>
    public string? Password { get; set; }


    /// <summary>
    /// Gets or sets the address of the volunteer. This may be null if no address is provided.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the volunteer's location. This may be null if not set.
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the volunteer's location. This may be null if not set.
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Gets or sets the  job assigned to the volunteer.
    /// </summary>
    public job Duty { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the volunteer is currently active.
    /// </summary>
    public Boolean IsActive { get; set; }

    /// <summary>
    /// Gets or sets the maximum distance the volunteer is willing to travel.
    /// This may be null if no limit is set.
    /// </summary>
    public double? MaximumDistance { get; set; }

    /// <summary>
    /// Gets or sets the type of distance used for calculations .
    /// </summary>
    public DistanceType? Distance { get; set; }//set->(תוספת)

    /// <summary>
    /// Gets the total number of calls the volunteer has handled.
    /// </summary>
    public int HandeledCalls { get; init; }

    /// <summary>
    /// Gets the total number of calls the volunteer has canceled.
    /// </summary>
    public int CanceledCalls { get; init; }

    /// <summary>
    /// Gets the total number of calls that have expired for the volunteer.
    /// </summary>
    public int ExpiredCalls { get; init; }

    /// <summary>
    /// Gets or sets the call currently being handled by the volunteer, if any.
    /// </summary>
    public BO.CallInProgress? CallInProgress { get; set; }


    /// <summary>
    /// Returns a string that represents the current object, including its properties.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString() => this.ToStringProperty();


}
