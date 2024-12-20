

namespace BlImplementation;

using BlApi;
using System.Xml;

internal class Bl : IBl
{

    public IVolunteer Volunteer { get; } = new VolunteerImplementation();
    public ICall Call { get; } = new CallImplementation();
    public IAdmin Admin { get; }=new IAdminImplementation();



}