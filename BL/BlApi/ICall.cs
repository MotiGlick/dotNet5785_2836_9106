

using BO;
using Helpers;

namespace BlApi;



/// <summary>
/// Interface for managing call-related operations.
/// </summary>
public interface ICall : IObservable
{

    /// <summary>
    /// Retrieves the amount of calls in various categories.
    /// </summary>
    /// <returns>
    /// An array of integers where each element represents the count of calls in a specific category.
    /// </returns>
    int[] CallsAmount();



    /// <summary>
    /// Retrieves a list of calls based on filtering and sorting criteria.
    /// </summary>
    /// <param name="filterField">The field to filter by.</param>
    /// <param name="filterValue">The value to filter by.</param>
    /// <param name="sortField">The field to sort by.</param>
    /// <returns>An enumerable collection of calls matching the criteria.</returns>
    IEnumerable<BO.CallInList> GetCallList(CallInListField? filterField, object? filterValue, CallInListField? sortField);


    /// <summary>
    /// Retrieves detailed information about a specific call.
    /// </summary>
    /// <param name="callId">The ID of the call to retrieve.</param>
    /// <returns>A BO.Call object containing detailed information about the call.</returns>
    BO.Call GetCallDetails(int callId);


    /// <summary>
    /// Updates the details of a specific call.
    /// </summary>
    /// <param name="call">A BO.Call object containing the updated call details.</param>
    void UpdateCallDetails(BO.Call call);


    /// <summary>
    /// Deletes a call by its ID.
    /// </summary>
    /// <param name="callId">The ID of the call to delete.</param>
    void DeleteCall(int callId);


    /// <summary>
    /// Adds a new call to the system.
    /// </summary>
    /// <param name="call">A BO.Call object containing the call details to add.</param>
    void AddCall(BO.Call call);


    /// <summary>
    /// Retrieves a list of closed calls assigned to a specific volunteer, filtered and sorted by criteria.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="callType">The type of call to filter by (optional).</param>
    /// <param name="sortField">The field to sort by (optional).</param>
    /// <returns>An enumerable collection of closed calls matching the criteria.</returns>
    IEnumerable<BO.ClosedCallInList> GetClosedCallsByVolunteer(int volunteerId, CallType? callType, ClosedCallInListField? sortField);



    /// <summary>
    /// Retrieves a list of open calls available for a specific volunteer, filtered and sorted by criteria.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="callType">The type of call to filter by (optional).</param>
    /// <param name="sortField">The field to sort by (optional).</param>
    /// <returns>An enumerable collection of open calls matching the criteria.</returns>
    IEnumerable<BO.OpenCallInList> GetOpenCallsForVolunteer(int volunteerId, BO.CallType? callType, OpenCallInListField? sortField);


    /// <summary>
    /// Ends the handling of a specific assignment by a volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="assignmentId">The ID of the assignment.</param>
    void EndHandling(int volunteerId, int assignmentId);



    /// <summary>
    /// Cancels the handling of a specific assignment by a volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="assignmentId">The ID of the assignment.</param>
    void CancelHandling(int volunteerId, int assignmentId);


    /// <summary>
    /// Assigns a volunteer to handle a specific call.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="callId">The ID of the call to handle.</param>
    void ChoosingCallForHandling(int volunteerId, int callId);


    #region Stage 6
    public bool CanBeDeleted(int callId);

    #endregion Stage 6

}
