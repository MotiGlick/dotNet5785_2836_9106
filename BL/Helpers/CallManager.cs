
using DalApi;

namespace Helpers;
using System.Text.Json;



/// <summary>
/// Manages operations related to calls, including status checks, validations, and time calculations.
/// </summary>
internal class CallManager
{

    internal static ObserverManager Observers = new(); //stage 5 


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

        if (boCall.Address.Length < 5 || boCall.Address.Length > 100)
            throw new BO.BlNotValidException("Invalid address.\nThe address format is incorrect.\nPlease use the following format:\n street name street number, city name.\nFor example: Dizengoff 99, Tel Aviv.");

        //if (!Tools.AddressExists(boCall.Address))
        //    throw new BO.BlNotValidException("Invalid address.\nThe address format is incorrect.\nPlease use the following format:\n street name street number, city name.\nFor example: Dizengoff 99, Tel Aviv.");

        if (boCall.MaxTime != null)
            if (boCall.MaxTime <= boCall.OpeningTime)
                throw new BO.BlNotValidException("Invalid date");


        return true;


    }



    /// <summary>
    /// Retrieves the current status of a call based on its ID.
    /// </summary>
    /// <param name="doCallId">The ID of the call.</param>
    /// <returns>The status of the call as a <see cref="BO.StatusOfCall"/>.</returns>
    /// <exception cref="Exception">Thrown if the call does not exist.</exception>
    public static BO.StatusOfCall GetCallStatus(int doCallId)
    {
        lock (AdminManager.BlMutex) //stage 7
        {
            DO.Call doCall = _dal.Call.Read(doCallId) ?? throw new Exception($"Call with ID={doCallId} does not exist");


            var now = AdminManager.Now;
            var callAssignments = AssignmentManager.GetCallAssignList(doCallId);
            var riskRangeEnd = doCall.CallTime + _dal.Config.RiskRange;
            var lastAssignment = AssignmentManager.GetLastAssingment(doCallId);
            var lastAssignmentClosureType = lastAssignment != null ? AssignmentManager.GetCallClosureType(lastAssignment.Id) : null;

            // If there is no assignment or all assignments are canceled
            if (lastAssignment == null || ((lastAssignment != null) && lastAssignmentClosureType == BO.CallClosureType.ManagerCancellation ||
              lastAssignmentClosureType == BO.CallClosureType.SelfCancellation))
            {
                if (now > riskRangeEnd && now < doCall.TotalTime)
                    return BO.StatusOfCall.OpenAtRisk;
                else if (now > doCall.TotalTime)
                    return BO.StatusOfCall.Expired;
                else
                    return BO.StatusOfCall.Open;
            }
            else if (lastAssignment != null && lastAssignmentClosureType == BO.CallClosureType.HandledOnTime)
            {
                return BO.StatusOfCall.Closed;
            }
            else if (lastAssignment != null && now > riskRangeEnd && now < doCall.TotalTime)
                return BO.StatusOfCall.InProgressAtRisk;
            else if (now > doCall.TotalTime || lastAssignmentClosureType == BO.CallClosureType.ExpiredCancellation)/////
                return BO.StatusOfCall.Expired;


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
        //lock (AdminManager.BlMutex) //stage 7
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
        lock (AdminManager.BlMutex) //stage 7
        {
            if ((_dal.Call.Read(callId)!.TotalTime > AdminManager.Now))
                return _dal.Call.Read(callId)!.TotalTime - AdminManager.Now;

            return null;
        }

    }

}