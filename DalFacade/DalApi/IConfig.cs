

namespace DalApi;



/// <summary>
/// interface for Config objects in the DAL
/// </summary>

public interface IConfig
{
    DateTime Clock { get; set; }
    int NextCallId { get; }

    int NextAssignmentId { get; }

    TimeSpan RiskRange { get; set; }
    void Reset();
}
