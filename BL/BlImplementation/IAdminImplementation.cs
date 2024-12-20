
namespace BlImplementation;
using BlApi;
using DalApi;
using Helpers;
using System;

internal class IAdminImplementation : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
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

    public DateTime GetClock()
    {
        return _dal.Config.Clock;
    }

    public TimeSpan GetMaxRange()
    {
        return _dal.Config.RiskRange;
    }

    public void InitializeDB()
    {
        ResetDB();
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);
        
    }

    public void ResetDB()
    {
        _dal.Config.Reset();
        ClockManager.UpdateClock(ClockManager.Now);
       
    }

    public void SetMaxRange(TimeSpan maxRange)
    {
        _dal.Config.RiskRange=maxRange;

    }
}

