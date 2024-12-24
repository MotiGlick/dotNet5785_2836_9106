

namespace BlApi;


/// <summary>
/// Interface for the Business Logic (BL) layer.
/// Provides access to various domain-specific managers.
/// </summary>
public interface IBl 
{
    /// <summary>
    /// Gets the volunteer management logic.
    /// </summary>
    IVolunteer Volunteer { get; }

    /// <summary>
    /// Gets the call management logic.
    /// </summary>
    ICall Call { get; }

    /// <summary>
    /// Gets the admin management logic.
    /// </summary>
    IAdmin Admin { get; }


}
