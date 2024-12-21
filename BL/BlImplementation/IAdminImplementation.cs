
namespace BlImplementation;
using BlApi;
using DalApi;
using Helpers;
using System;



/// <summary>
/// Implementation of the IAdmin interface. This class provides methods for managing
/// the system's clock, initializing and resetting the database, and configuring the
/// maximum risk range for calls.
/// </summary>
internal class IAdminImplementation : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// Advances the system clock by a specified time unit.
    /// </summary>
    /// <param name="unit">The time unit by which to advance the clock (Minute, Hour, Day, Month, Year).</param>
    public void ForwardClock(BO.TimeUnit unit)
    {
        switch (unit)
        {
            case BO.TimeUnit.Minute:                
                ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
                break;
            case BO.TimeUnit.Hour:
                ClockManager.UpdateClock(ClockManager.Now.AddHours(1));
                break;
            case BO.TimeUnit.Day:
                ClockManager.UpdateClock(ClockManager.Now.AddDays(1));
                break;
            case BO.TimeUnit.Month:
                ClockManager.UpdateClock(ClockManager.Now.AddMonths(1));
                break;
            case BO.TimeUnit.Year:
                ClockManager.UpdateClock(ClockManager.Now.AddYears(1));

                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Retrieves the current system clock time from the database.
    /// </summary>
    /// <returns>The current system clock time.</returns>
    public DateTime GetClock()
    {
        return _dal.Config.Clock;
    }



    /// <summary>
    /// Retrieves the maximum risk range (TimeSpan) from the database.
    /// </summary>
    /// <returns>The maximum risk range.</returns>
    public TimeSpan GetMaxRange()
    {
        return _dal.Config.RiskRange;
    }



    /// <summary>
    /// Initializes the database by resetting it and running the initialization process.
    /// Updates the system clock to the current time.
    /// </summary>
    public void InitializeDB()
    {
        ResetDB();
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);
        
    }



    /// <summary>
    /// Resets the database and updates the system clock to the current time.
    /// </summary>
    public void ResetDB()
    {
        _dal.Config.Reset();
        ClockManager.UpdateClock(ClockManager.Now);
       
    }

    /// <summary>
    /// Sets the maximum risk range (TimeSpan) in the database.
    /// </summary>
    /// <param name="maxRange">The new maximum risk range.</param>
    public void SetMaxRange(TimeSpan maxRange)
    {
        _dal.Config.RiskRange=maxRange;

    }
}

