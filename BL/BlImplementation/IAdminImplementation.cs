
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



    #region Stage 4
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Advances the system clock by a specified time unit.
    /// </summary>
    /// <param name="unit">The time unit by which to advance the clock (Minute, Hour, Day, Month, Year).</param>
    public void ForwardClock(BO.TimeUnit unit)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7
        lock (AdminManager.BlMutex) //stage 7
        {
            switch (unit)
            {
                case BO.TimeUnit.Minute:
                    AdminManager.UpdateClock(AdminManager.Now.AddMinutes(1));
                    break;
                case BO.TimeUnit.Hour:
                    AdminManager.UpdateClock(AdminManager.Now.AddHours(1));
                    break;
                case BO.TimeUnit.Day:
                    AdminManager.UpdateClock(AdminManager.Now.AddDays(1));
                    break;
                case BO.TimeUnit.Month:
                    AdminManager.UpdateClock(AdminManager.Now.AddMonths(1));
                    break;
                case BO.TimeUnit.Year:
                    AdminManager.UpdateClock(AdminManager.Now.AddYears(1));

                    break;
                default:
                    break;
            }
        }
        
    }


    /// <summary>
    /// Retrieves the current system clock time from the database.
    /// </summary>
    /// <returns>The current system clock time.</returns>
    public DateTime GetClock()
    {
        lock (AdminManager.BlMutex) //stage 7
            return AdminManager.Now;
    }  



    /// <summary>
    /// Sets the maximum risk range (TimeSpan) in the database.
    /// </summary>
    /// <param name="maxRange">The new maximum risk range.</param>
    public void SetMaxRange(TimeSpan maxRange)
    {
        lock (AdminManager.BlMutex)//stage 7
            AdminManager.MaxRange = maxRange;
    }


    /// <summary>
    /// Retrieves the maximum risk range (TimeSpan) from the database.
    /// </summary>
    /// <returns>The maximum risk range.</returns>
    public TimeSpan GetMaxRange()
    {
        lock (AdminManager.BlMutex)//stage 7
            return AdminManager.MaxRange;
    }


    /// <summary>
    /// Initializes the database by resetting it and running the initialization process.
    /// Updates the system clock to the current time.
    /// </summary>
    public void InitializeDB()
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7
        lock (AdminManager.BlMutex) //stage 7
        {
            ResetDB();
            DalTest.Initialization.Do();
            AdminManager.UpdateClock(AdminManager.Now);
            //AdminManager.MaxRange = AdminManager.MaxRange;
            AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
            AdminManager.InitializeDB(); //stage 7
        }
         
    }

    /// <summary>
    /// Resets the database and updates the system clock to the current time.
    /// </summary>
    public void ResetDB()
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7
        lock (AdminManager.BlMutex) //stage 7
        {
            _dal.ResetDB();
            AdminManager.UpdateClock(AdminManager.Now);
            //AdminManager.MaxRange = AdminManager.MaxRange;
            AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
            AdminManager.ResetDB(); //stage 7
        }
    }

#endregion Stage 4

    #region Stage 5
    public void AddClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers += clockObserver;
    public void RemoveClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers -= clockObserver;
    public void AddConfigObserver(Action configObserver) =>
   AdminManager.ConfigUpdatedObservers += configObserver;
    public void RemoveConfigObserver(Action configObserver) =>
    AdminManager.ConfigUpdatedObservers -= configObserver;
    #endregion Stage 5

    #region Stage 7
    public void StartSimulator(int interval)  //stage 7
    {
        AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
        AdminManager.Start(interval); //stage 7
    }

    public void StopSimulator()
    => AdminManager.Stop(); //stage 7
    #endregion Stage 7
}

