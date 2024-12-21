
using DalApi;

namespace Helpers;
using System.Text.Json;



/// <summary>
/// Manages operations related to calls, including status checks, validations, and time calculations.
/// </summary>
internal class CallManager
{
    /// <summary>
    /// Data access layer instance for interacting with the data source.
    /// </summary>
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;



    /// <summary>
    /// Performs a logical validation check on a given call.
    /// </summary>
    /// <param name="boCall">The call to validate.</param>
    /// <returns>
    /// True if the call passes validation (address exists and max time is after opening time, if applicable); otherwise, false.
    /// </returns>
    public static bool LogicalCheck(BO.Call boCall)//#
    {

        if (!Tools.AddressExists(boCall.Address) || (boCall.MaxTime != null && (boCall.MaxTime <= boCall.OpeningTime)))
            return false;

        return true;

    }



    /// <summary>
    /// Retrieves the current status of a call based on its ID.
    /// </summary>
    /// <param name="doCallId">The ID of the call.</param>
    /// <returns>The status of the call as a <see cref="BO.StatusOfCall"/>.</returns>
    /// <exception cref="Exception">Thrown if the call does not exist.</exception>
    public static BO.StatusOfCall GetCallStatus(int doCallId)//????
    {
        DO.Call doCall = _dal.Call.Read(doCallId) ?? throw new Exception($"Call with ID={doCallId} does not exists");
        DO.Assignment? lastAssingment = AssignmentManager.GetLastAssingment(doCallId);


        // If there is no assignment or all assignments are canceled
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

        else //there is an Assingment
        {

            if (ClockManager.Now > doCall.CallTime + _dal.Config.RiskRange && ClockManager.Now < doCall.TotalTime)
                return BO.StatusOfCall.InProgressAtRisk;
            else if (ClockManager.Now > doCall.TotalTime)
                return BO.StatusOfCall.Closed;
            else
                return BO.StatusOfCall.InProgress;
        }



    }


    /// <summary>
    /// Calculates the total handling time of the last assignment for a given call.
    /// </summary>
    /// <param name="callId">The ID of the call.</param>
    /// <returns>The total handling time as a <see cref="TimeSpan"/>, or null if no assignment exists.</returns>
    public static TimeSpan? GetHandlingTime(int callId)
    {
        return AssignmentManager.GetLastAssingment(callId)?.AssignmentTotalTime - AssignmentManager.GetLastAssingment(callId)?.AssignmentTime;
        // return _dal.Call.Read(callId)?.TotalTime - _dal.Call.Read(callId)?.CallTime;

    }


    /// <summary>
    /// Calculates the remaining time until the call's total time is reached.
    /// </summary>
    /// <param name="callId">The ID of the call.</param>
    /// <returns>
    /// The remaining time as a <see cref="TimeSpan"/> if the current time is before the call's total time; otherwise, null.
    /// </returns>
    public static TimeSpan? GetTimeLeft(int callId)
    {
        if ((_dal.Call.Read(callId)!.TotalTime > ClockManager.Now))
            return _dal.Call.Read(callId)!.TotalTime - ClockManager.Now;

        return null;
    }

}