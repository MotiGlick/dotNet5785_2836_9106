

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

        DO.Volunteer doVolunteer = new(boVolunteer.Id,boVolunteer.Name, boVolunteer.PhoneNumber, boVolunteer.Email,,,,,, boVolunteer.IsActive);
        try
        {
            _dal.Volunteer.Create(boVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
        }
    }
    /*
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
