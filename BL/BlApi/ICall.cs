

namespace BlApi;
public interface ICall
{



    int[] callsAmount();

    IEnumerable<BO.CallInList> GetCallList( Enum? filterField, object? filterValue, Enum? sortField ;;

    BO.Call GetCallDetails(int callId);

    void UpdateCallDetails(BO.Call call);

    void DeleteCall(int callId);

    void addCall(BO.Call call);

    IEnumerable<BO.ClosedCallInList> GetClosedCallsByVolunteer(int volunteerId, Enum? callType , Enum? sortField );

    IEnumerable<BO.OpenCallInList> GetOpenCallsForVolunteer(int volunteerId, Enum? callType , Enum? sortField );

    void EndHandling(int volunteerId, int assignmentId);

    void CancelHandling(int volunteerId, int assignmentId);

    void ChoosingCallForHandling(int volunteerId, int callId);

}
