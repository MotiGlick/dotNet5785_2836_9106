using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DalApi;
namespace Dal;

//stage 3
/// <summary>
/// This class provides the implementation of the IDal interface, using XML as the data storage.
/// </summary>
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    /// <summary>
    /// Provides access to volunteer-related data operations.
    /// </summary>
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();

    /// <summary>
    /// Provides access to assignment-related data operations.
    /// </summary>
    public IAssignment Assignment { get; } = new AssignmentImplementation();

    /// <summary>
    /// Provides access to call-related data operations.
    /// </summary>
    public ICall Call { get; } = new CallImplementation();

    /// <summary>
    /// Provides access to configuration-related data operations.
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Resets the entire database by deleting all records (volunteers, calls, assignments) and resetting configurations.
    /// This method clears all stored data and restores default settings.
    /// </summary>
    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}

