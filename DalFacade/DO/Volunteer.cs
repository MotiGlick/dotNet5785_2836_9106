

namespace DO;

/// <summary>
/// A volunteer entity represents a volunteer with all of their accessories.
/// </summary>
/// <param name="Id">Personal unique ID of the volunteer</param>
/// <param name="Name">name of the volunteer</param>
/// <param name="PhoneNumber">standard mobile phone</param>
/// <param name="Email">Valid email address</param>
/// <param name="pasword">Volunteer's password</param>
/// <param name="AddressVolunteer">Residential address of the volunteer.</param>
/// <param name="Latitude">Latitude coordinate of the volunteer's location.</param>
/// <param name="Longitude">Longitude coordinate of the volunteer's location.</param>
/// <param name="IsActive">Indicates if the volunteer is currently active.</param>
/// <param name="maximumDistace">Maximum distance the volunteer is willing to cover.</param>
/// <param name="duty">Job role of the volunteer.</param>
/// <param name="distance">Preferred distance type for duties</param>
public record Volunteer
(
    int Id,
    string Name,
    string PhoneNumber,
    string Email,
    string? pasword,
    string? AddressVolunteer,
    double? Latitude,
    double? Longitude,
    double? maximumDistace,
    job duty,
    bool IsActive = true,
    distanceType distance = distanceType.Air
)
{
        // Default constructor
        public Volunteer() : this(0, "", "", "", null, null, null, null, null, job.volunteer, true, distanceType.Air) { }
}
