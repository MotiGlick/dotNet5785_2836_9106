

namespace BlImplementation;

using BlApi;
using System.Xml;


/// <summary>
/// Implementation of the IBl interface.
/// This class provides access to various domain-specific managers (Volunteer, Call, and Admin).
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// Gets the volunteer management logic.
    /// </summary>
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();


    /// <summary>
    /// Gets the call management logic.
    /// </summary>
    public ICall Call { get; } = new CallImplementation();

    /// <summary>
    /// Gets the admin management logic.
    /// </summary>
    public IAdmin Admin { get; }=new IAdminImplementation();



}