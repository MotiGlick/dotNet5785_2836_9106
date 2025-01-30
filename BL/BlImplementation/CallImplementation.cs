
namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using System.Text.RegularExpressions;


/// <summary>
/// Implementation of the ICall interface.
/// Provides methods for managing calls in the system.
/// </summary>
internal class CallImplementation : BlApi.ICall
{

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;



    #region Stage 4
    /// <summary>
    /// Adds a new call to the system.
    /// Validates the call and creates a corresponding record in the database.
    /// </summary>
    /// <param name="boCall">The call object to be added.</param>
    /// <exception cref="Exception">Thrown if the call is invalid or already exists.</exception>
    public void AddCall(BO.Call boCall)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        try
        {

            if (CallManager.LogicalCheck(boCall))
            {
                DO.Call doCall = new(
                boCall.Id,
                (DO.CallType)boCall.CallType,
                boCall.Address,
                boCall.Description,
                null, // Latitude - ייחושב בהמשך
                null, // Longitude - ייחושב בהמשך
                boCall.OpeningTime,
                boCall.MaxTime
            );
                lock (AdminManager.BlMutex) //stage 7
                {
                    _dal.Call.Create(doCall);
                }
                _ = updateCoordinatesForCall(doCall, boCall.Address);
            }
            

            CallManager.Observers.NotifyListUpdated(); //stage 5

        }
        catch (BO.BlNotValidException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new BO.BlAlreadyExistsException($"Call with ID={boCall.Id} already exists", ex);

        }


    }
    /// <summary>
    /// Asynchronously updates the coordinates of a call after retrieving them from an external service.
    /// </summary>
    /// <param name="doCall">The call object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task updateCoordinatesForCall(DO.Call doCall, string address)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7
        // חישוב קואורדינטות
        double? latitude = await Tools.GetLatitude(address);
        double? longitude = await Tools.GetLongitude(address);

        doCall = doCall with { Latitude = latitude, Longitude = longitude };
        lock (AdminManager.BlMutex) //stage 7
            _dal.Call.Update(doCall);

        CallManager.Observers.NotifyListUpdated();
    }



    /// <summary>
    /// Retrieves the number of calls for each call type.
    /// </summary>
    /// <returns>An array where each index represents a call type and its count.</returns>
    public int[] CallsAmount()//#
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7
        lock (AdminManager.BlMutex) //stage 7
        {
            // יצירת מערך בגודל Enum
            int[] result = new int[Enum.GetValues(typeof(BO.CallType)).Length];

            var grouped = _dal.Call.ReadAll().GroupBy(call => call.type);//קבוצות לפי סוג

            foreach (var group in grouped)
            {
                result[(int)group.Key] = group.Count();
            }

            return result;
        }

    }




    /// <summary>
    /// Cancels the handling of a call by a volunteer.
    /// Allows self or manager cancellation of an assignment if valid.
    /// </summary>
    /// <param name="volunteerId">ID of the volunteer attempting the cancellation.</param>
    /// <param name="assignmentId">ID of the assignment to cancel.</param>
    /// <exception cref="Exception">Thrown if the volunteer cannot cancel the assignment.</exception>
    public void CancelHandling(int volunteerId, int assignmentId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        DO.Volunteer doVolunteer;
        lock (AdminManager.BlMutex) //stage 7
            doVolunteer = _dal.Volunteer.Read(volunteerId) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteerId} does not exists");
        DO.Assignment doAssignment;
        lock (AdminManager.BlMutex) //stage 7
            doAssignment = _dal.Assignment.Read(assignmentId) ?? throw new BO.BlDoesNotExistException($"Assignmen with ID={assignmentId}  does not exists");


        if ((doAssignment.VolunteerId == volunteerId || doVolunteer.duty == DO.job.manager)
            && (CallManager.GetCallStatus(doAssignment.CallId) == BO.StatusOfCall.InProgressAtRisk
            || CallManager.GetCallStatus(doAssignment.CallId) == BO.StatusOfCall.InProgress))
        {
            try
            {
                // Update assignment with cancellation closure type based on who cancels
                if (doAssignment.VolunteerId == volunteerId)
                    lock (AdminManager.BlMutex) //stage 7
                        _dal.Assignment.Update(new(assignmentId, doAssignment.CallId, doAssignment.VolunteerId, doAssignment.AssignmentTime, AdminManager.Now, DO.CallClosureType.SelfCancellation));
                else
                    lock (AdminManager.BlMutex) //stage 7
                        _dal.Assignment.Update(new(assignmentId, doAssignment.CallId, doAssignment.VolunteerId, doAssignment.AssignmentTime, AdminManager.Now, DO.CallClosureType.ManagerCancellation));

                CallManager.Observers.NotifyItemUpdated(doAssignment.CallId);  //stage 5
                CallManager.Observers.NotifyListUpdated();  //stage 5
                VolunteerManager.Observers.NotifyItemUpdated(volunteerId);  //stage 5
            }
            catch (Exception ex)
            {
                throw new BO.BlDoesNotExistException($"Assignment with ID={assignmentId} does Not exist", ex);
            }

        }
        else
        {
            throw new BO.BlNotAlowedException($"Volunteer with ID={volunteerId} is not allow to cancel Assignment with ID={assignmentId}");
        }


    }


    /// <summary>
    /// Assigns a call to a volunteer for handling.
    /// Only open or open-at-risk calls can be selected.
    /// </summary>
    /// <param name="volunteerId">ID of the volunteer choosing the call.</param>
    /// <param name="callId">ID of the call to assign.</param>
    /// <exception cref="Exception">Thrown if the call is not open or open-at-risk.</exception>
    public void ChoosingCallForHandling(int volunteerId, int callId)
    {
        //AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        DO.Volunteer doVolunteer;
        lock (AdminManager.BlMutex) //stage 7
            doVolunteer = _dal.Volunteer.Read(volunteerId) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteerId} does not exists");
        DO.Call doCall;
        lock (AdminManager.BlMutex) //stage 7
            doCall = _dal.Call.Read(callId) ?? throw new BO.BlDoesNotExistException($"Call with ID={callId}  does not exists");

        if (CallManager.GetCallStatus(callId) != BO.StatusOfCall.Open && CallManager.GetCallStatus(callId) != BO.StatusOfCall.OpenAtRisk)
            throw new BlNotAlowedException($"Call with ID={callId} does not open for hanling");


        // Create an assignment for the volunteer and call
        else
        {
            lock (AdminManager.BlMutex) //stage 7
                _dal.Assignment.Create(new(0, callId, volunteerId, AdminManager.Now, null, null));

            VolunteerManager.Observers.NotifyItemUpdated(volunteerId); //stage 6
            CallManager.Observers.NotifyItemUpdated(callId); //stage 5
            BO.Call doCaljl = GetCallDetails(callId) ?? throw new BO.BlDoesNotExistException($"Call with ID={callId}  does not exists");
            CallManager.Observers.NotifyListUpdated(); //stage 6
        }



    }


    /// <summary>
    /// Deletes a call from the system.
    /// Calls can only be deleted if no assignments exist and the status is open or open-at-risk.
    /// </summary>
    /// <param name="callId">ID of the call to delete.</param>
    /// <exception cref="Exception">Thrown if the call cannot be deleted due to assignments or status.</exception>
    public void DeleteCall(int callId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        DO.Call doCall;//    
        lock (AdminManager.BlMutex) //stage 7
            doCall = _dal.Call.Read(callId) ?? throw new BO.BlDoesNotExistException($"Call with ID={callId} does Not exist");//Exception 
                                                                                                                             // Check if there are no assignments and call is open or open-at-risk
        if (AssignmentManager.GetCallAssignList(callId).Count == 0 &&
           (CallManager.GetCallStatus(callId) == BO.StatusOfCall.Open ||
            CallManager.GetCallStatus(callId) == BO.StatusOfCall.OpenAtRisk))
        {
            lock (AdminManager.BlMutex) //stage 7
                _dal.Call.Delete(callId);
            CallManager.Observers.NotifyListUpdated(); //stage 5
        }

        else
            throw new BO.BlNotAlowedException($"Call with ID={callId} is not allow to delete");

    }


    /// <summary>
    /// Ends the handling of a call by a volunteer.
    /// Marks the assignment as handled on time.
    /// </summary>
    /// <param name="volunteerId">ID of the volunteer ending the handling.</param>
    /// <param name="assignmentId">ID of the assignment to end.</param>
    /// <exception cref="Exception">Thrown if the volunteer is not authorized to end the assignment.</exception>
    public void EndHandling(int volunteerId, int assignmentId)
    {
        //AdminManager.ThrowOnSimulatorIsRunning();//stage 7 

        DO.Volunteer doVolunteer;
        lock (AdminManager.BlMutex) //stage 7
            doVolunteer = _dal.Volunteer.Read(volunteerId) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteerId} does not exists");
        DO.Assignment doAssignment;
        lock (AdminManager.BlMutex) //stage 7
            doAssignment = _dal.Assignment.Read(assignmentId) ?? throw new BO.BlDoesNotExistException($"Call with ID={assignmentId}  does not exists");

        if (doAssignment.VolunteerId == volunteerId
           && (CallManager.GetCallStatus(doAssignment.CallId) != BO.StatusOfCall.InProgress
            || CallManager.GetCallStatus(doAssignment.CallId) != BO.StatusOfCall.InProgressAtRisk))
        {
            try
            {
                // Update assignment status to "HandledOnTime"
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Assignment.Update(new(assignmentId, doAssignment.CallId, doAssignment.VolunteerId, doAssignment.AssignmentTime, AdminManager.Now, DO.CallClosureType.HandledOnTime));

                CallManager.Observers.NotifyListUpdated(); //stage 5
                CallManager.Observers.NotifyItemUpdated(doAssignment.CallId); //stage 5
                VolunteerManager.Observers.NotifyItemUpdated(volunteerId); //stage 5
            }

            catch (Exception ex)
            {
                throw new BO.BlDoesNotExistException($"Assignment with ID={assignmentId} does Not exist", ex);
            }
        }
        else
        {
            throw new BO.BlNotAlowedException($"Volunteer with ID={volunteerId} is not allow to end Assignment with ID={assignmentId}");
        }

    }



    /// <summary>
    /// Retrieves the details of a specific call.
    /// </summary>
    /// <param name="callId">ID of the call to retrieve.</param>
    /// <returns>The call details.</returns>
    /// <exception cref="Exception">Thrown if the call does not exist.</exception>
    public BO.Call GetCallDetails(int callId)
    {

        // AdminManager.ThrowOnSimulatorIsRunning();//stage 7 
        DO.Call doCall;
        lock (AdminManager.BlMutex) //stage 7
            doCall = _dal.Call.Read(callId) ?? throw new BO.BlDoesNotExistException($"Call with ID={callId} does Not exist");//Exception

        return new BO.Call
        {
            Id = doCall.Id,
            CallType = (BO.CallType)doCall.type,
            Description = doCall.Description,
            Address = doCall.CallAddress,
            Latitude = doCall.Latitude,
            Longitude = doCall.Longitude,
            OpeningTime = doCall.CallTime,
            MaxTime = doCall.TotalTime,
            status = CallManager.GetCallStatus(callId),
            CallAssignsList = AssignmentManager.GetCallAssignList(callId)
        };
    }




    /// <summary>
    /// Retrieves a list of calls with optional filtering and sorting.
    /// </summary>
    /// <param name="filterField">Field to filter the calls by (optional).</param>
    /// <param name="filterValue">Value to filter by (optional).</param>
    /// <param name="sortField">Field to sort the calls by (optional).</param>
    /// <returns>A list of calls matching the filter criteria.</returns>
    public IEnumerable<BO.CallInList> GetCallList(CallInListField? filterField = null, object? filterValue = null, CallInListField? sortField = null)
    {

        lock (AdminManager.BlMutex) // stage 7
        {
            var listOfCalls = _dal.Call.ReadAll().Select
            (call => new BO.CallInList
            {
                Id = AssignmentManager.GetLastAssingmentId(call.Id),
                CallId = call.Id,
                CallType = (BO.CallType)call.type,
                OpeningTime = call.CallTime,
                TimeLeft = CallManager.GetTimeLeft(call.Id),
                LastVolunteerName = AssignmentManager.GetLastVolunteerName(call.Id),
                HandlingTime = CallManager.GetHandlingTime(call.Id),
                status = CallManager.GetCallStatus(call.Id),
                AmountOfAssigns = AssignmentManager.GetCallAssignList(call.Id).Count,
            })
             .Distinct()
            .Where(call => filterField switch
            {
                //CallInListField.Id => call.Id == (int?)filterValue,
                // CallInListField.CallId => call.CallId == (int?)filterValue,
                CallInListField.CallType => call.CallType == (BO.CallType)filterValue!,
                //CallInListField.OpeningTime => call.OpeningTime == (DateTime?)filterValue,
                // CallInListField.TimeLeft => call.TimeLeft == (TimeSpan?)filterValue,
                //CallInListField.LastVolunteerName => call.LastVolunteerName == (string?)filterValue,
                //CallInListField.HandlingTime => call.HandlingTime == (TimeSpan?)filterValue,
                CallInListField.status => call.status == (BO.StatusOfCall)filterValue!,
                CallInListField.AmountOfAssigns => call.AmountOfAssigns == (int?)filterValue,
                _ => true
            })
          .OrderBy<BO.CallInList, object?>(call => sortField switch
          {
              CallInListField.AssignmentId => call.Id,
              CallInListField.CallId => call.CallId,
              CallInListField.CallType => (int)call.CallType,
              CallInListField.OpeningTime => call.OpeningTime,
              CallInListField.TimeLeft => call.TimeLeft,
              CallInListField.LastVolunteerName => call.LastVolunteerName,
              CallInListField.HandlingTime => call.HandlingTime,
              CallInListField.status => (int)call.status,
              _ => call.Id
          });
            return listOfCalls;
        }

    }



    /// <summary>
    /// Retrieves a list of closed calls handled by a specific volunteer.
    /// </summary>
    /// <param name="volunteerId">ID of the volunteer.</param>
    /// <param name="callTypeFilter">Optional filter by call type.</param>
    /// <param name="sortField">Optional field to sort the results by.</param>
    /// <returns>List of closed calls handled by the volunteer.</returns>
    public IEnumerable<BO.ClosedCallInList> GetClosedCallsByVolunteer(int volunteerId, BO.CallType? callTypeFilter, ClosedCallInListField? sortField)
    {

        lock (AdminManager.BlMutex) // stage 7
        {
            return GetCallList()
             .Where(call => AssignmentManager.GetCallAssignList(call.CallId).Any(assign => assign.VolunteerId == volunteerId))
             .Select(call => new ClosedCallInList
             {
                 Id = (int)call.Id!,
                 CallType = call.CallType,
                 Address = _dal.Call.Read(call.CallId)!.CallAddress,
                 OpeningTime = call.OpeningTime,
                 HndalStartTime = AssignmentManager.GetAssignmentStartTime(call.CallId, volunteerId),
                 EndTime = AssignmentManager.GetAssignmentEndTime(call.CallId, volunteerId),
                 ClosureType = AssignmentManager.GetAssignmentClosureType(call.CallId, volunteerId)
             })
            .Where(call => callTypeFilter == null ? true : call.CallType == callTypeFilter)
            .OrderBy<BO.ClosedCallInList, object?>((call => sortField switch
            {
                ClosedCallInListField.Id => call.Id,
                ClosedCallInListField.CallType => (int)call.CallType,
                ClosedCallInListField.Address => call.Address,
                ClosedCallInListField.OpeningTime => call.OpeningTime,
                ClosedCallInListField.HndalStartTime => call.HndalStartTime,
                ClosedCallInListField.EndTime => call.EndTime,
                ClosedCallInListField.ClosureType => call.ClosureType,
                _ => call.Id
            }));
        }


    }


    /// <summary>
    /// Retrieves a list of open calls assigned to a specific volunteer.
    /// </summary>
    /// <param name="volunteerId">ID of the volunteer.</param>
    /// <param name="callType">Optional filter by call type.</param>
    /// <param name="sortField">Optional field to sort the results by.</param>
    /// <returns>List of open calls assigned to the volunteer.</returns>
    public IEnumerable<BO.OpenCallInList> GetOpenCallsForVolunteer(int volunteerId, BO.CallType? callType, OpenCallInListField? sortField)
    {

        lock (AdminManager.BlMutex) // stage 7
        {
            IEnumerable<BO.CallInList> listOfCalls;

            if (callType != null)
                listOfCalls = GetCallList(CallInListField.CallType, callType);
            else
                listOfCalls = GetCallList();

            return listOfCalls
               .Where(call => call.status == BO.StatusOfCall.Open || call.status == BO.StatusOfCall.OpenAtRisk)
               .Select(call => new BO.OpenCallInList
               {
                   Id = call.CallId,
                   CallType = call.CallType,
                   Description = _dal.Call.Read(call.CallId)!.Description,
                   Address = _dal.Call.Read(call.CallId)!.CallAddress,
                   OpeningTime = call.OpeningTime,
                   MaxTime = _dal.Call.Read(call.CallId)!.TotalTime,
                   DistanceFromVolunteer = Tools.CalculateDistance(_dal.Volunteer.Read(volunteerId), _dal.Call.Read(call.CallId))
               })
            .OrderBy<BO.OpenCallInList, object?>(call => sortField switch
            {
                OpenCallInListField.Id => call.Id,
                OpenCallInListField.CallType => (int)call.CallType,
                OpenCallInListField.Address => call.Address,
                OpenCallInListField.OpeningTime => call.OpeningTime,
                OpenCallInListField.MaxTime => call.MaxTime,
                OpenCallInListField.DistanceFromVolunteer => call.DistanceFromVolunteer,
                _ => call.Id
            });
        }
    }

    /// <summary>
    /// Updates the details of an existing call in the system.
    /// </summary>
    /// <param name="boCall">The updated call information.</param>
    /// <exception cref="Exception">Thrown if the call is invalid or already exists.</exception>
    public void UpdateCallDetails(BO.Call boCall)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        try
        {
            // Validate the call using the LogicalCheck method from CallManager
            if (CallManager.LogicalCheck(boCall))
            {
                //throw new BO.BlNotValidException("Call is not valid");
               DO.Call doCall = new(
               boCall.Id,
               (DO.CallType)boCall.CallType,
               boCall.Address,
               boCall.Description,
               null, // Latitude - ייחושב בהמשך
               null, // Longitude - ייחושב בהמשך
               boCall.OpeningTime,
               boCall.MaxTime
           );
                lock (AdminManager.BlMutex) //stage 7
                {
                    _dal.Call.Update(doCall);
                }
                _ = updateCoordinatesForCall(doCall, boCall.Address);
                // Update the call in the data layer
                //lock (AdminManager.BlMutex) // stage 7
                //    _dal.Call.Update(
                //    new
                //    (
                //       boCall.Id,
                //       (DO.CallType)boCall.CallType,
                //       boCall.Address,
                //       boCall.Description,
                //       Tools.GetLatitude(boCall.Address),
                //       Tools.GetLongitude(boCall.Address),
                //       boCall.OpeningTime,
                //       boCall.MaxTime
                //    ));
            }
            CallManager.Observers.NotifyItemUpdated(boCall.Id);  //stage 5
            CallManager.Observers.NotifyListUpdated();  //stage 5

        }
        catch (BO.BlNotValidException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={boCall.Id} already exists", ex);
        }

    }

    #endregion Stage 4

    #region Stage 5
    public void AddObserver(Action listObserver) =>
    CallManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    CallManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    CallManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    CallManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5


    #region Stage 6
    public bool CanBeDeleted(int callId)
    {
        if (AssignmentManager.GetCallAssignList(callId).Count == 0 &&
           (CallManager.GetCallStatus(callId) == BO.StatusOfCall.Open ||
            CallManager.GetCallStatus(callId) == BO.StatusOfCall.OpenAtRisk))
            return true;

        return false;
    }

    #endregion Stage 6



}

