

using BO;
using DO;
using System;
using System.Collections.Generic;

namespace Helpers;


/// <summary>
/// Manages operations related to assignments, including retrieving and processing assignment details.
/// </summary>
internal class AssignmentManager
{
    /// <summary>
    /// Data access layer instance for interacting with the data source.
    /// </summary>
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// Retrieves a list of call assignments filtered by a specific call ID.
    /// </summary>
    /// <param name="CallId">The ID of the call to filter assignments by. Can be null to retrieve all assignments.</param>
    /// <returns>A list of <see cref="BO.CallAssignInList"/> containing assignment details.</returns>
    public static List<BO.CallAssignInList> GetCallAssignList(int? CallId)
    {

        return (from assignment in _dal.Assignment.ReadAll()
                where assignment.CallId == CallId
                let VolunteerName = _dal.Volunteer.Read(assignment.VolunteerId)!.Name
                let HndalEndTime = assignment.AssignmentTotalTime
                let ClosureType = assignment.type
                select new BO.CallAssignInList
                {
                    VolunteerId = assignment.VolunteerId,
                    VolunteerName = VolunteerName,
                    HndalStartTime = assignment.AssignmentTime,
                    HndalEndTime = HndalEndTime,
                    ClosureType = (BO.CallClosureType)ClosureType,

                }).ToList();

    }


    /// <summary>
    /// Retrieves the start time of an assignment for a given call and volunteer.
    /// </summary>
    /// <param name="boCallId">The ID of the call.</param>
    /// <param name="VolunteerId">The ID of the volunteer.</param>
    /// <returns>The start time of the assignment.</returns>
    public static DateTime GetAssignmentStartTime(int boCallId, int VolunteerId)
    {

        return GetCallAssignList(boCallId).FirstOrDefault(v => v.VolunteerId == VolunteerId).HndalStartTime;

    }


    /// <summary>
    /// Retrieves the end time of an assignment for a given call and volunteer.
    /// </summary>
    /// <param name="boCallId">The ID of the call.</param>
    /// <param name="VolunteerId">The ID of the volunteer.</param>
    /// <returns>The end time of the assignment.</returns>
    /// <exception cref="Exception">Thrown if the end time cannot be found.</exception>
    public static DateTime GetAssignmentEndTime(int boCallId, int VolunteerId)
    {

        var assignList = GetCallAssignList(boCallId);
        return assignList.FirstOrDefault(v => v.VolunteerId == VolunteerId).HndalEndTime ?? throw new Exception("ClosureType not foand");

    }


    /// <summary>
    /// Retrieves the closure type of an assignment for a given call and volunteer.
    /// </summary>
    /// <param name="boCallId">The ID of the call.</param>
    /// <param name="VolunteerId">The ID of the volunteer.</param>
    /// <returns>The closure type of the assignment.</returns>
    /// <exception cref="Exception">Thrown if the closure type cannot be found.</exception>
    public static BO.CallClosureType GetAssignmentClosureType(int boCallId, int VolunteerId)
    {

        return GetCallAssignList(boCallId).FirstOrDefault(v => v.VolunteerId == VolunteerId).ClosureType ?? throw new Exception("ClosureType not foand");

    }

    /// <summary>
    /// Retrieves the ID of the last assignment for a specific call.
    /// </summary>
    /// <param name="doCallId">The ID of the call.</param>
    /// <returns>The ID of the last assignment, or null if no assignment exists.</returns>
    public static int? GetLastAssingmentId(int doCallId)//#
    {

        if (GetLastAssingment(doCallId) == null)
            return null;
        else
            return GetLastAssingment(doCallId)!.Id;

    }

    /// <summary>
    /// Retrieves the name of the volunteer in the last assignment for a specific call.
    /// </summary>
    /// <param name="doCallId">The ID of the call.</param>
    /// <returns>The name of the last volunteer.</returns>
    public static string? GetLastVolunteerName(int doCallId)//#
    {

        return GetCallAssignList(doCallId).OrderByDescending(Assin => Assin.HndalStartTime).First().VolunteerName;

    }

    /// <summary>
    /// Retrieves the ID of the volunteer in the last assignment for a specific call.
    /// </summary>
    /// <param name="doCallId">The ID of the call.</param>
    /// <returns>The ID of the last volunteer.</returns>
    public static int? GetLastVolunteerId(int doCallId)//#
    {

        return GetCallAssignList(doCallId).OrderByDescending(Assin => Assin.HndalStartTime).First().VolunteerId;

    }

    /// <summary>
    /// Retrieves the last assignment for a specific call.
    /// </summary>
    /// <param name="doCallId">The ID of the call.</param>
    /// <returns>The last assignment, or null if no assignment exists.</returns>
    public static DO.Assignment? GetLastAssingment(int doCallId)//#
    {

        return _dal.Assignment.ReadAll().FirstOrDefault(assin => assin.VolunteerId == GetLastVolunteerId(doCallId));

    }

    //public static BO.CallAssignInList? GetLastAssingmentInCallAssignList(int doCallId)//#
    //{

    //    return GetCallAssignList(doCallId).OrderByDescending(Assin => Assin.HndalStartTime).First();

    //}
}
