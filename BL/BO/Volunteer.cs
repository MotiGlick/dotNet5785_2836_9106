

using Helpers;

namespace BO;

public class Volunteer
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public job Duty { get; set; }
    public Boolean IsActive { get; set; }
    public double? MaximumDistance { get; set; }
    public DistanceType? Distance { get; set; }//set->(תוספת)
    public int HandeledCalls { get; init; }////init???
    public int CanceledCalls { get; init; }////init???
    public int ExpiredCalls { get; init; }////init???

    public BO.CallInProgress? CallInProgress { get; set; }//???->init


   public override string ToString() => this.ToStringProperty();


}
