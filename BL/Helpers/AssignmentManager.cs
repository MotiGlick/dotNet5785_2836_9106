

using BO;
using DO;
using System.Collections.Generic;

namespace Helpers;

internal class AssignmentManager
{
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

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



    public static DateTime GetAssignmentStartTime(int boCallId, int VolunteerId)
    {

        var assignList = GetCallAssignList(boCallId);
        return assignList.FirstOrDefault(v => v.VolunteerId == VolunteerId).HndalStartTime;

    }

    public static DateTime GetAssignmentEndTime(int boCallId, int VolunteerId)
    {

        var assignList = GetCallAssignList(boCallId);
        return assignList.FirstOrDefault(v => v.VolunteerId == VolunteerId).HndalEndTime ?? throw new Exception("ClosureType not foand");

    }


    public static BO.CallClosureType GetAssignmentClosureType(int boCallId, int VolunteerId)
    {

        return GetCallAssignList(boCallId).FirstOrDefault(v => v.VolunteerId == VolunteerId).ClosureType ?? throw new Exception("ClosureType not foand");

    }

    public static int? GetLastAssingmentId(int doCallId)//#
    {
       
        if (GetLastAssingment(doCallId) == null)
            return null;
        else
            return GetLastAssingment(doCallId)!.Id;

    }
    public static string? GetLastVolunteerName(int doCallId)//#
    {

        return GetCallAssignList(doCallId).OrderByDescending(Assin => Assin.HndalStartTime).First().VolunteerName;

    }

    public static int? GetLastVolunteerId(int doCallId)//#
    {

        return GetCallAssignList(doCallId).OrderByDescending(Assin => Assin.HndalStartTime).First().VolunteerId;

    }


    public static DO.Assignment? GetLastAssingment(int doCallId)//#
    {

        return _dal.Assignment.ReadAll().FirstOrDefault(assin => assin.VolunteerId == GetLastVolunteerId(doCallId));

    }

    public static BO.CallAssignInList? GetLastAssingmentInCallAssignList(int doCallId)//#
    {

        return GetCallAssignList(doCallId).OrderByDescending(Assin => Assin.HndalStartTime).First();

    }
}
