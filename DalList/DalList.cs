using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DalApi;
namespace Dal;



/// <summary>
/// DalList class provides implementations for the IDal interface.
/// It manages the CRUD operations for Volunteer, Call, Assignment, and Config entities.
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();
    public ICall Call { get; } = new CallImplementation();
    public IAssignment Assignment { get; } = new AssignmentImplementation();
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Resets the database by deleting all records from Volunteer, Call, Assignment, and resetting Config.
    /// </summary>
    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}

