

namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    
    public void Create(BO.Volunteer boVolunteer)
    {
        if (VolunteerManager.formatCheckToVolunteer(boVolunteer)&& VolunteerManager.logicCheckToVolunteer(boVolunteer))
        {
            DO.Volunteer doVolunteer = new DO.Volunteer()
            {
                Id = boVolunteer.Id,
                Name = boVolunteer.Name,
                PhoneNumber = boVolunteer.PhoneNumber,
                Email = boVolunteer.Email,
                pasword = boVolunteer.Password,
                AddressVolunteer = boVolunteer.Address,
                Latitude = boVolunteer.Latitude,
                Longitude = boVolunteer.Longitude,
                maximumDistace = boVolunteer.MaximumDistance,
                duty =,
                IsActive = boVolunteer.IsActive,
                distance = (DO.DistanceType)boVolunteer.Distance
            };

        };
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
    /// <summary>
    /// Deletes a volunteer if they are eligible to be deleted, based on their assignments and current status.
    /// If the volunteer cannot be deleted, an exception is thrown.
    /// </summary>
    /// <param name="id">The unique identifier of the volunteer to be deleted.</param>
    /// <exception cref="BO.DalDoesNotExistException">Thrown when the volunteer cannot be deleted, either because they are currently assigned to a call or do not exist in the database.</exception>

    public void Delete(int id)
    {
        try
        {
            if (VolunteerManager.CanDeleted(id)) {
                _dal.Volunteer.Delete(id);
            }
            else throw new BO.DalDoesNotExistException($"Volunteer with ID={id} not exists", ex);
        }
        catch (DO.DalDoesNotExistException ex) {
            throw new BO.DalDoesNotExistException($"Volunteer with ID={id} not exists", ex);
        }

    }
    /// <summary>
    /// Login method:
    /// Receives username and password
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pasward"></param>
    /// <returns >user role</returns>
    /// <exception cref="BO.BlInvalidCredentialsException"></exception>
    public BO.job EnterToTheSystem(string name, string? pasward)
    {
        
        DO.Volunteer? doVolunteer = _dal.Volunteer.ReadAll().FirstOrDefault(v => v.Name == name && v.pasword == pasward);

        if (doVolunteer != null)
        {
            return (BO.job)doVolunteer.duty;
        }
        else
        {
            throw new BO.BlInvalidCredentialsException("Invalid username or password.");
        }
    }

    public BO.Volunteer Read(int id)
    {
        try
        {
            DO.Volunteer? doVolunteer = _dal.Volunteer.Read(id);
            if (doVolunteer != null)
            {
                BO.Volunteer volunteer = new BO.Volunteer() {
                    Id = id,
                    Name= doVolunteer.Name,
                    PhoneNumber = doVolunteer.PhoneNumber,
                    Email = doVolunteer.Email,
                    Password = doVolunteer.pasword,
                    Address = doVolunteer.AddressVolunteer,
                    Latitude = doVolunteer.Latitude,
                    Longitude = doVolunteer.Longitude,
                    Duty = (BO.job)doVolunteer.duty,
                    IsActive = doVolunteer.IsActive,
                    MaximumDistance = doVolunteer.maximumDistace,
                    Distance = (BO.DistanceType)doVolunteer.distance,
                    HandeledCalls = VolunteerManager.GetNumOfcheck(id),
                    CanceledCalls = VolunteerManager.GetNumOfSellfCancellation(id),
                    ExpiredCalls = VolunteerManager.GetNumOfExpired(id),
                    CallInProgress = VolunteerManager.CreateCallInProgress(id)
                };
                return volunteer;
            }
            else throw new BO.BlAlreadyExistsException($"Volunteer with ID={id} not exists") ;
        }
        catch (DO.DalAlreadyExistsException ex) {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={id} not exists");
        }
    }
    /// <summary>
    /// Retrieves a list of volunteers with an optional filter based on their activity status and sorts the list by a specified field.
    /// If no filter or sorting field is provided, it defaults to sorting by volunteer ID.
    /// </summary>
    /// <param name="isActive">Optional filter to specify if the volunteer should be active (true) or inactive (false). If null, no filter is applied.</param>
    /// <param name="sortField">Optional field to sort the volunteer list by. If null, the list is sorted by volunteer ID by default.</param>
    /// <returns>A sorted list of volunteers (BO.VolunteerInList) based on the provided filters and sorting criteria.</returns>

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive, BO.VolunteerInListField? sortField)
    {
        // שליפת כל המתנדבים עם או בלי פילטר לפי הפעילות
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll(v =>
            isActive == null || v.IsActive == isActive);

        // המרת הישויות הלוגיות (DO.Volunteer) ל-BO.VolunteerInList
        IEnumerable<BO.VolunteerInList> volunteersList = volunteers.Select(v => new BO.VolunteerInList
        {
            Id = v.Id,
            Name = v.Name,
            IsActive = v.IsActive,
            TotalHandledCalls = VolunteerManager.GetNumOfcheck(v.Id),
            TotalCanceledCalls = VolunteerManager.GetNumOfSellfCancellation(v.Id),
            TotalExpiredCalls = VolunteerManager.GetNumOfExpired(v.Id),
            CurrentCallId = VolunteerManager.GetOpenCallId(v.Id),
            CurrentCallType = VolunteerManager.GetCurrentCallId(v.Id)

        });

        // מיון הרשימה לפי שדה המיון המבוקש
        if (sortField != null)
        {
            switch (sortField)
            {
                case BO.VolunteerInListField.Id:
                    volunteersList = volunteersList.OrderBy(v => v.Id);
                    break;
                case BO.VolunteerInListField.Name:
                    volunteersList = volunteersList.OrderBy(v => v.Name);
                    break;
                case BO.VolunteerInListField.IsActive:
                    volunteersList = volunteersList.OrderBy(v => v.IsActive);
                    break;
                case BO.VolunteerInListField.TotalHandledCalls:
                    volunteersList = volunteersList.OrderByDescending(v => v.TotalHandledCalls);
                    break;
                case BO.VolunteerInListField.TotalCanceledCalls:
                    volunteersList = volunteersList.OrderByDescending(v => v.TotalCanceledCalls);
                    break;
                case BO.VolunteerInListField.TotalExpiredCalls:
                    volunteersList = volunteersList.OrderByDescending(v => v.TotalExpiredCalls);
                    break;
                case BO.VolunteerInListField.CurrentCallId:
                    volunteersList = volunteersList.OrderBy(v => v.CurrentCallId);
                    break;
                case BO.VolunteerInListField.CurrentCallType:
                    volunteersList = volunteersList.OrderBy(v => v.CurrentCallType);
                    break;
            }
        }
        else
        {
            // מיון לפי ת.ז אם ערך המיון לא הועבר
            volunteersList = volunteersList.OrderBy(v => v.Id);
        }

        return volunteersList;
    }

    public void Update(BO.Volunteer boVolunteer, int id)
    {
        try
        {
            bool isManager = _dal.Volunteer.Read(id).duty == DO.job.manager;
            if (isManager || id == boVolunteer.Id) {//בודק אם הוא מנהל או שזה המתנדב עצמו
                if (VolunteerManager.formatCheckToVolunteer(boVolunteer) && VolunteerManager.logicCheckToVolunteer(boVolunteer))//בדיקה לוגית ותקינות
                {
                    DO.Volunteer doVolunteer = new DO.Volunteer() {
                        Id = boVolunteer.Id,
                        Name= boVolunteer.Name,
                        PhoneNumber = boVolunteer.PhoneNumber,
                        Email = boVolunteer.Email,
                        pasword = boVolunteer.Password,
                        AddressVolunteer = boVolunteer.Address,
                        Latitude = boVolunteer.Latitude,
                        Longitude = boVolunteer.Longitude,
                        maximumDistace = boVolunteer.MaximumDistance,
                        duty=,
                        IsActive = boVolunteer.IsActive,
                        distance = (DO.DistanceType)boVolunteer.Distance                    };
                    
                };
            }
            Update();
        }
        catch (DO.DalDoesNotExistException ex) { 
        }
    }
}
