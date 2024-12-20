

using BO;

namespace BlApi;
public interface ICall
{



    int[] CallsAmount();

    IEnumerable<BO.CallInList> GetCallList(CallInListField? filterField, object? filterValue, CallInListField? sortField);

    BO.Call GetCallDetails(int callId);

    void UpdateCallDetails(BO.Call call);

    void DeleteCall(int callId);

    void AddCall(BO.Call call);

    IEnumerable<BO.ClosedCallInList> GetClosedCallsByVolunteer(int volunteerId, CallType? callType, ClosedCallInListField? sortField);

    IEnumerable<BO.OpenCallInList> GetOpenCallsForVolunteer(int volunteerId, BO.CallType? callType, OpenCallInListField? sortField);

    void EndHandling(int volunteerId, int assignmentId);

    void CancelHandling(int volunteerId, int assignmentId);

    void ChoosingCallForHandling(int volunteerId, int callId);

}
