
using DalApi;

namespace Helpers;


using System.Text.Json;
//using BO;

internal class CallManager
{

    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;




    public static bool LogicalCheck(BO.Call boCall)//#
    {

        if (!Tools.AddressExists(boCall.Address) || (boCall.MaxTime != null && (boCall.MaxTime <= boCall.OpeningTime)))
            return false;

        return true;

    }




    public static BO.StatusOfCall GetCallStatus(int doCallId)//????
    {
        DO.Call doCall = _dal.Call.Read(doCallId) ?? throw new Exception($"Call with ID={doCallId} does not exists");
        DO.Assignment? lastAssingment = AssignmentManager.GetLastAssingment(doCallId);


        //if there is no Assingment
        if (AssignmentManager.GetCallAssignList(doCallId) == null
            || AssignmentManager.GetCallAssignList(doCallId).All(assign => assign.ClosureType == BO.CallClosureType.SelfCancellation || assign.ClosureType == BO.CallClosureType.ManagerCancellation))
        {
            if (ClockManager.Now > doCall.CallTime + _dal.Config.RiskRange && ClockManager.Now < doCall.TotalTime)
                return BO.StatusOfCall.OpenAtRisk;
            else if (ClockManager.Now > doCall.TotalTime)
                return BO.StatusOfCall.Expired;
            else
                return BO.StatusOfCall.Open;

        }

        else //there is Assingment
        {

            if (ClockManager.Now > doCall.CallTime + _dal.Config.RiskRange && ClockManager.Now < doCall.TotalTime)
                return BO.StatusOfCall.InProgressAtRisk;
            else if (ClockManager.Now > doCall.TotalTime)
                return BO.StatusOfCall.Closed;
            else
                return BO.StatusOfCall.InProgress;
        }



    }



    public static TimeSpan? GetHandlingTime(int callId)
    {
        return AssignmentManager.GetLastAssingment(callId)?.AssignmentTotalTime - AssignmentManager.GetLastAssingment(callId)?.AssignmentTime;
        // return _dal.Call.Read(callId)?.TotalTime - _dal.Call.Read(callId)?.CallTime;

    }

    public static TimeSpan? GetTimeLeft(int callId)
    {
        if ((_dal.Call.Read(callId)!.TotalTime > ClockManager.Now))
            return _dal.Call.Read(callId)!.TotalTime - ClockManager.Now;

        return null;
    }

}