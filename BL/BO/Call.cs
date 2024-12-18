
namespace BO;

public class Call
{

    public int Id { get; init; }

    public CallType CallType { get; init; }

    public string? Description { get; init; }

    public string Address { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public DateTime OpeningTime { get; init; }

    public DateTime? MaxTime { get; set; }


    public StatusOfCall status { get; set; }

    public List<BO.CallAssignInList>? CallAssignsList { get; set; }//???->init





}
