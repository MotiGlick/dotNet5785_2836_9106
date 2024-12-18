

namespace BlImplementation;
using BlApi;
using Helpers;
using System;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Volunteer boVolunteer)
    {
        if (VolunteerManager.formatCheckToVolunteer(boVolunteer)&& VolunteerManager.logicCheckToVolunteer(boVolunteer))
        {
            DO.Volunteer doVolunteer = new(boVolunteer.Id, boVolunteer.Name, boVolunteer.PhoneNumber, boVolunteer.Email, boVolunteer.Password, boVolunteer.Address, boVolunteer.Latitude, boVolunteer.Longitude, , boVolunteer.IsActive);
            try
            {
                _dal.Volunteer.Create(doVolunteer);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
            }
        }
        
    }
    /*
     * 
     * public int Id { get; init; }
    public string Name { get; init; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Duty Duty { get; set; }
    public Boolean IsActive { get; set; }
    public double? MaximumDistance { get; set; }
    public TypeOfDistance? Distance { get; set; }//set->(תוספת)
    public int HandeledCalls { get; init; }////init???
    public int CanceledCalls { get; init; }////init???
    public int ExpiredCalls { get; init; }////init???

    public BO.CallInProgress? CallInProgress { get; set; }//???->init

      int Id,
    string Name,
    string PhoneNumber,
    string Email,
    string? pasword,
    string? AddressVolunteer,
    double? Latitude,
    double? Longitude,
    double? maximumDistace,
    job duty,
    bool IsActive = true,
    distanceType distance = distanceType.Air
)
{
    
    // Default constructor
    public Volunteer() : this(0, "", "", "", null, null, null, null, null, job.volunteer, true, distanceType.Air) { }
     */
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Duty enterSystem(int id, int passward)
    {
        throw new NotImplementedException();
    }

    public BO.Volunteer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive, Enum? sortField)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Volunteer boVolunreer, int id)
    {
        throw new NotImplementedException();
    }
}
