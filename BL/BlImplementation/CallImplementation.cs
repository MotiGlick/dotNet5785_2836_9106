

namespace BlImplementation;
using BlApi;
using Helpers;
using System;
using System.Collections.Generic;

internal class CallImplementation : ICall
{

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddCall(BO.Call boCall)
    {

        if (CallManager.logicaCheck(boCall))
        {
            DO.Call doCall = new(boCall.Id, (DO.CallType)boCall.CallType, boCall.Description, 
                boCall.Address, (double)boCall.Latitude, (double)boCall.Longitude, boCall.OpeningTime, boCall.MaxTime);
            try
            {
                _dal.Call.Create(doCall);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw ex;
                // throw new BO.BlAlreadyExistsException($"Student with ID={boStudent.Id} already exists", ex);
            }
        }

    }

    public int[] CallsAmount()
    {
        //// שימוש ב-LINQ 
        //var grouped =
        //    from call in calls
        //    group call by call.Status into statusGroup
        //    select new { Status = statusGroup.Key, Count = statusGroup.Count() };

        //// מציאת הסטטוס המקסימלי (לצורך יצירת מערך בגודל המתאים)
        //int maxStatus = calls.Any() ? calls.Max(call => call.Status) : 0;

        //// יצירת מערך ומילוי הכמויות לפי הסטטוסים
        //int[] result = new int[maxStatus + 1];
        //foreach (var group in grouped)
        //{
        //    result[group.Status] = group.Count;
        //}

        //return result;
    }

    public void CancelHandling(int volunteerId, int assignmentId)
    {
        throw new NotImplementedException();
    }

    public void ChoosingCallForHandling(int volunteerId, int callId)
    {
        throw new NotImplementedException();
    }

    public void DeleteCall(int callId)
    {
        throw new NotImplementedException();
    }

    public void EndHandling(int volunteerId, int assignmentId)
    {
        throw new NotImplementedException();
    }

    public BO.Call GetCallDetails(int callId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.CallInList> GetCallList(Enum? filterField, object? filterValue, Enum? sortField)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.ClosedCallInList> GetClosedCallsByVolunteer(int volunteerId, Enum? callType, Enum? sortField)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.OpenCallInList> GetOpenCallsForVolunteer(int volunteerId, Enum? callType, Enum? sortField)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallDetails(BO.Call boCall)
    {
        if (CallManager.logicaCheck(boCall))
        {
            DO.Call doCall = new(boCall.Id, (DO.CallType)boCall.CallType, boCall.Description,
                boCall.Address, (double)boCall.Latitude, (double)boCall.Longitude, boCall.OpeningTime, boCall.MaxTime);
            try
            {
                _dal.Call.Update(doCall);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw ex;
                // throw new BO.BlAlreadyExistsException($"Student with ID={boStudent.Id} already exists", ex);
            }
        }
    }
}
