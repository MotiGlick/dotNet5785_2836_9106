

namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
//using DalApi;
using DO;
using Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Xml.Linq;

internal class VolunteerImplementation : BlApi.IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;



    #region Stage 4
    /// <summary>
    /// Creates a new volunteer in the system.
    /// Validates the volunteer's data for proper formatting and logical correctness.
    /// If validation passes, the volunteer is added to the database.
    /// </summary>
    /// <param name="boVolunteer"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    /// Thrown if a volunteer with the same ID already exists in the database.
    /// </exception>
    public void Create(BO.Volunteer boVolunteer)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7
        try
        {
            if (VolunteerManager.formatCheckToVolunteer(boVolunteer) && VolunteerManager.logicCheckToVolunteer(boVolunteer))
            {
                DO.Volunteer doVolunteer = new DO.Volunteer()
                {
                    Id = boVolunteer.Id,
                    Name = boVolunteer.Name,
                    PhoneNumber = boVolunteer.PhoneNumber,
                    Email = boVolunteer.Email,
                    pasword = boVolunteer.Password,
                    AddressVolunteer = boVolunteer.Address,
                    Latitude = boVolunteer.Address == "" ? null : Tools.GetLatitude(boVolunteer.Address),
                    Longitude = boVolunteer.Address == "" ? null : Tools.GetLongitude(boVolunteer.Address),
                    maximumDistace = boVolunteer.MaximumDistance,
                    duty = (DO.job)boVolunteer.Duty,
                    IsActive = boVolunteer.IsActive,
                    distance = (DO.DistanceType)boVolunteer.Distance
                };
                _dal.Volunteer.Create(doVolunteer);
                VolunteerManager.Observers.NotifyListUpdated(); //stage 5 

            }
            else
                throw new BO.BlNotValidException("The volunteer data is not valid");

        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID:{boVolunteer.Id} already exists", ex);
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
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        try
        {
            if (VolunteerManager.CanDeleted(id))
            {
                _dal.Volunteer.Delete(id);
                VolunteerManager.Observers.NotifyListUpdated(); //stage 5 
            }
            else throw new BO.BlDeletionImpossible($"Volunteer with ID={id} can not be deleted because he/she handles call");
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDeletionImpossible($"Volunteer with ID={id} can not be deleted because :", ex);
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
    public BO.job EnterToTheSystem(string? name = null, string? pasward = null, int? id = null)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        DO.Volunteer? doVolunteer = null;

        if (id != null)
            doVolunteer = _dal.Volunteer.ReadAll().FirstOrDefault(v => v.Id == id && v.pasword == pasward);
        else if (name != null)
        {
            doVolunteer = _dal.Volunteer.ReadAll().FirstOrDefault(v => v.Name == name && v.pasword == pasward);
        }
        else
        {
            throw new BlNotValidException("Must provide either ID or Name and Password.");
        }

     
        if (doVolunteer != null)
        {
            return (BO.job)doVolunteer.duty;
        }
        else
        {
            throw new BlNotValidException("Invalid username or password.");
        }
    }
    /// <summary>
    /// Return volunteer by their ID.
    /// Converts the data from the data layer object to the business layer object and enriches it with additional derived properties.

    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.Volunteer containing the volunteer's details</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// Thrown if the volunteer with the specified ID does not exist in the database.
    /// </exception>
    public BO.Volunteer Read(int id)
    {
        
        DO.Volunteer? doVolunteer = _dal.Volunteer.Read(id);
        if (doVolunteer != null)
        {
            BO.Volunteer volunteer = new BO.Volunteer()
            {
                Id = id,
                Name = doVolunteer.Name,
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
        else throw new BO.BlDoesNotExistException($"Volunteer with ID={id} not exists");
    }

    /// <summary>
    /// Retrieves a list of volunteers with an optional filter based on their activity status and sorts the list by a specified field.
    /// If no filter or sorting field is provided, it defaults to sorting by volunteer ID.
    /// </summary>
    /// <param name="isActive">Optional filter to specify if the volunteer should be active (true) or inactive (false). If null, no filter is applied.</param>
    /// <param name="sortField">Optional field to sort the volunteer list by. If null, the list is sorted by volunteer ID by default.</param>
    /// <returns>A sorted list of volunteers (BO.VolunteerInList) based on the provided filters and sorting criteria.</returns>
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.VolunteerInListField? sortField = null, BO.CallType? filterValue = null)
    {


        // שליפת כל המתנדבים עם או בלי פילטר לפי הפעילות
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll(v => isActive == null ? true : v.IsActive == isActive);//||isActive == null 

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
            CurrentCallType = VolunteerManager.GetCurrentCallType(v.Id)

        }).Where(v => filterValue == null ? true : v.CurrentCallType == filterValue);

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
    /// <summary>
    /// Updates the details of an existing volunteer. 
    /// The update is subject to role-based conditions: only a manager can update any volunteer, 
    /// while a volunteer can only update their own information with certain restrictions on role changes.
    /// </summary>
    /// <param name="boVolunteer"></param>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// Thrown if the volunteer to be updated does not exist in the database.
    /// </exception>
    public void Update(BO.Volunteer boVolunteer, int id)
    {
        AdminManager.ThrowOnSimulatorIsRunning();//stage 7

        try
        {
            if (VolunteerManager.formatCheckToVolunteer(boVolunteer) && VolunteerManager.logicCheckToVolunteer(boVolunteer))//בדיקה לוגית ותקינות
            {
                bool isManager = _dal.Volunteer.Read(id).duty == DO.job.manager;
                DO.Volunteer oldDoVolunteer = _dal.Volunteer.Read(boVolunteer.Id);
                DO.Volunteer newDoVolunteer = new DO.Volunteer()
                {
                    Id = boVolunteer.Id,
                    Name = boVolunteer.Name,
                    PhoneNumber = boVolunteer.PhoneNumber,
                    Email = boVolunteer.Email,
                    pasword = boVolunteer.Password,
                    AddressVolunteer = boVolunteer.Address,
                    Latitude = boVolunteer.Address == "" ? null : Tools.GetLatitude(boVolunteer.Address),
                    Longitude = boVolunteer.Address == "" ? null : Tools.GetLongitude(boVolunteer.Address),
                    maximumDistace = boVolunteer.MaximumDistance,
                    duty = (DO.job)boVolunteer.Duty,
                    IsActive = boVolunteer.IsActive,
                    distance = (DO.DistanceType)boVolunteer.Distance
                };

                if (isManager)
                {// אם הוא מנהל 
                    _dal.Volunteer.Update(newDoVolunteer);
                    VolunteerManager.Observers.NotifyItemUpdated(newDoVolunteer.Id);  //stage 5
                    VolunteerManager.Observers.NotifyListUpdated();  //stage 5
                }
                else if (id == boVolunteer.Id)//אם זה המתנדב עצמו זה שמעדכן
                {
                    if (oldDoVolunteer.duty != newDoVolunteer.duty && oldDoVolunteer.duty == DO.job.manager)//אם מעדכנים את התפקיד אז זה רק אם הוא מנהל
                    {
                        _dal.Volunteer.Update(newDoVolunteer);
                        VolunteerManager.Observers.NotifyItemUpdated(newDoVolunteer.Id);  //stage 5
                        VolunteerManager.Observers.NotifyListUpdated();  //stage 5
                    }
                    else if (oldDoVolunteer.duty == newDoVolunteer.duty)
                    {
                        _dal.Volunteer.Update(newDoVolunteer);// אם הוא לא מנסה לעדכן את תפקיד 
                        VolunteerManager.Observers.NotifyItemUpdated(newDoVolunteer.Id);  //stage 5
                        VolunteerManager.Observers.NotifyListUpdated();  //stage 5
                    }
                }


            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with ID:{boVolunteer.Id} not exists", ex);
        }
        catch (BO.BlNotValidException ex)
        {
            throw ex;
        }
    }
    #endregion Stage 4

    #region Stage 5
    public void AddObserver(Action listObserver) =>
    VolunteerManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    VolunteerManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    VolunteerManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    VolunteerManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5




    #region Stage 6
    public  bool CanDeleted(int id) => VolunteerManager.CanDeleted(id);

    #endregion Stage 6



}




