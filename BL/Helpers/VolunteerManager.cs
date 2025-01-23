

using BO;
using DalApi;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Helpers;

internal class VolunteerManager
{
    internal static ObserverManager Observers = new(); //stage 5 


    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// A method that checks the correctness of all values ​​in terms of format
    /// </summary>
    /// <param name="volunteer"></param>
    /// <returns></returns>
    public static bool formatCheckToVolunteer(BO.Volunteer volunteer)
    {

        // בדיקת ת.ז (האם מספרי)
        if (!int.TryParse(volunteer.Id.ToString(), out int id) || id <= 0) throw new BO.BlNotValidException("Incorrect ID");

        // בדיקת שם מלא (אורך נדרש)
        if (string.IsNullOrWhiteSpace(volunteer.Name) || volunteer.Name.Length < 2 || volunteer.Name.Length > 50)
            throw new BO.BlNotValidException("Incorrect name\n name must have at least 2 letters");

        // בדיקת טלפון סלולרי (אורך ומבנה תקין)
        if (string.IsNullOrWhiteSpace(volunteer.PhoneNumber) ||
          !Regex.IsMatch(volunteer.PhoneNumber, @"^\d{10}$"))
            throw new BO.BlNotValidException("Incorrect phone number\n phone number must have 10 letters and start with 0");


        //בדיקת מרחק מרבי(אם קיים)
        if (volunteer.MaximumDistance != 0 && (!double.TryParse(volunteer.MaximumDistance.ToString(), out double maxDistance) || maxDistance <= 0)) throw new BO.BlNotValidException("Invalid maximum distance.\nThe value must be a positive number."); 

        if (string.IsNullOrWhiteSpace(volunteer.Email))
            throw new BO.BlNotValidException("Incorrect email");

        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(volunteer.Email, emailPattern))
            throw new BO.BlNotValidException("Incorrect email");

        // בדיקת כתובת מלאה (אורך נדרש אם קיים)
        if (volunteer.Address != "" && (volunteer.Address.Length < 5 || volunteer.Address.Length > 100) && volunteer.Address != null)
            throw new BO.BlNotValidException("Invalid address.\nThe address format is incorrect.\nPlease use the following format:\n street name street number, city name.\nFor example: Dizengoff 99, Tel Aviv.");


        return true;
    }
    /// <summary>
    /// A method that checks the logical correctness of all values.
    /// </summary>
    /// <param name="volunteer"></param>
    /// <returns></returns>
    public static bool logicCheckToVolunteer(BO.Volunteer volunteer)
    {

        if(!Tools.IsPasswordGood(volunteer.Password))
            throw new BO.BlNotValidException("Password too weak \nThe password must contain an uppercase letter, a lowercase letter and 6 digits ");

        //if (!IsValidId(volunteer.Id))
        //    throw new BO.BlNotValidException("Incorrect ID");


        if (!IsValidPhoneNumber(volunteer.PhoneNumber))
        {
            throw new BO.BlNotValidException("Incorrect phone number\n phone number must have 10 letters and start with 0"); // מספר טלפון לא תקין
        }
        if (volunteer.Address != "")
            if (!Tools.AddressExists(volunteer.Address))
                throw new BO.BlNotValidException("Invalid address.\nThe address format is incorrect.\nPlease use the following format:\n street name street number, city name.\nFor example: Dizengoff 99, Tel Aviv.");

        return true;
    }
    /// <summary>
    /// A method that checks whether the ID card is valid
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static bool IsValidId(int id)
    {
        // הפיכת מספר תעודת הזהות למחרוזת כדי לטפל בספרות
        string idStr = id.ToString();

        // בדיקה אם האורך שגוי (למשל פחות מ-9 ספרות)
        if (idStr.Length != 9)
        {
            return false;
        }

        int sum = 0;

        for (int i = 0; i < idStr.Length; i++)
        {
            int num = int.Parse(idStr[i].ToString());

            if (i % 2 == 1) // מכפלה של הספרה במקום האי-זוגי
            {
                num *= 2;
                if (num > 9) num -= 9; // אם המספר גדול מ-9, מחסרים 9
            }

            sum += num;
        }

        return sum % 10 == 0; // בדיקת ספרת ביקורת
    }
    /// <summary>
    /// Phone number validation function
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // בדיקה אם המספר ריק, אם הוא לא באורך 10, או אם אינו מתחיל ב-0
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.StartsWith("0"))
        {
            return false;
        }

        // בדיקה שכל התווים הם ספרות
        if (!phoneNumber.All(char.IsDigit))
        {
            return false;
        }

        return true; // מספר הטלפון תקין
    }


    /// <summary>
    /// A function that receives an id and checks whether there is a call in the care of that volunteer. Otherwise - null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.CallInProgress or null</returns>
    public static BO.CallInProgress? CreateCallInProgress(int id)
    {
        
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == id);
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();


        if (!assignments.Any())
            return null;

 
        var latestAssignment = assignments.OrderByDescending(a => a.AssignmentTime).FirstOrDefault();

        if (latestAssignment == null)
            return null;

        var relatedCall = calls.FirstOrDefault(c => c.Id == latestAssignment.CallId);
        if (relatedCall == null ||
            (CallManager.GetCallStatus(relatedCall.Id) != BO.StatusOfCall.InProgress &&
             CallManager.GetCallStatus(relatedCall.Id) != BO.StatusOfCall.InProgressAtRisk))
        {
            return null;
        }

        return new BO.CallInProgress
        {
            Id = latestAssignment.Id,
            CallId = latestAssignment.CallId,
            CallType = (BO.CallType)relatedCall.type,
            Description = relatedCall.Description,
            Address = relatedCall.CallAddress,
            OpeningTime = relatedCall.CallTime,
            MaxTime = relatedCall.TotalTime,
            HndalStartTime = latestAssignment.AssignmentTime,
            DistanceFromVolunteer = Tools.CalculateDistance(
                _dal.Volunteer.Read(latestAssignment.VolunteerId),
                relatedCall
            ),
            status = CallManager.GetCallStatus(relatedCall.Id)
        };
    }

    /// <summary>
    /// Retrieves the total number of calls handled on time by a specific volunteer.
    /// </summary>
    /// <param name="id">The ID of the volunteer.</param>
    /// <returns>The count of calls handled on time.</returns>
    public static int GetNumOfcheck(int id)
    {
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == id);
        int sum = assignments.Count(a => a.type == DO.CallClosureType.HandledOnTime);
        return sum;
    }
    /// <summary>
    /// Retrieves the total number of calls canceled by the volunteer themselves.
    /// </summary>
    /// <param name="id">The ID of the volunteer.</param>
    /// <returns>The count of self-canceled calls.</returns>
    public static int GetNumOfSellfCancellation(int id)
    {
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == id);
        int sum = assignments.Count(a => a.type == DO.CallClosureType.SelfCancellation);
        return sum;
    }
    /// <summary>
    /// Retrieves the total number of expired calls associated with a volunteer.
    /// </summary>
    /// <param name="id">The ID of the volunteer.</param>
    /// <returns>The count of expired calls.</returns>
    public static int GetNumOfExpired(int id)
    {
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == id);
        int sum = assignments.Count(a => a.type == DO.CallClosureType.ExpiredCancellation);
        return sum;
    }
    /// <summary>
    /// A function that receives an id if there is no call in its handling - will return a value of None, otherwise it will return the CallType
    /// </summary>
    /// <param name="volunteerId"></param>
    /// <returns></returns>
    public static BO.CallType GetCurrentCallType(int volunteerId)
    {
        // שליפת כל המשימות של המתנדב
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == volunteerId);

        // מציאת הקריאה הנוכחית (פעילה)
        var currentAssignment = assignments
            .Where(a => CallManager.GetCallStatus(a.CallId) is BO.StatusOfCall.InProgress or BO.StatusOfCall.Open or BO.StatusOfCall.OpenAtRisk)
            .OrderByDescending(a => a.AssignmentTime) // הכי עדכנית
            .FirstOrDefault();

        // אם אין משימה פעילה
        if (currentAssignment == null)
            return BO.CallType.None;

        // מציאת הקריאה התואמת
        var call = _dal.Call.Read(currentAssignment.CallId);
        return call != null ? (BO.CallType)call.type : BO.CallType.None;
    }
    /// <summary>
    /// Returns the CallId of the most recent open or active call for a given volunteer.
    /// If there is no open or active call, returns null.
    /// </summary>
    /// <param name="volunteerId">The volunteer's unique identifier (Volunteer ID) for which to find the most recent open or active call.</param>
    /// <returns>The CallId of the most recent open or active call, or null if none exists.</returns>
    public static int? GetOpenCallId(int volunteerId)
    {
        // Retrieve all assignments for the volunteer
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == volunteerId);

        // Find the most recent (active) call
        var currentAssignment = assignments
            .Where(a => CallManager.GetCallStatus(a.CallId) is BO.StatusOfCall.InProgress or BO.StatusOfCall.InProgressAtRisk)
            .OrderByDescending(a => a.AssignmentTime) // Most recent assignment
            .FirstOrDefault();

        // If no active assignment is found
        if (currentAssignment == null)
            return null;  // Alternatively, you could return another value, such as "" or a message like "No open call found"

        // If an active assignment exists, return its CallId
        return currentAssignment.CallId;
    }
    /// <summary>
    /// Determines whether a volunteer can be deleted based on their current and past assignments.
    /// </summary>
    /// <param name="id">The ID of the volunteer.</param>
    /// <returns>
    /// True if the volunteer can be deleted (no active or past assignments exist), otherwise false.
    /// </returns>
    public static bool CanDeleted(int id)
    {

        // שליפת כל המשימות של המתנדב
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == id);

        // בדיקה אם המתנדב מטפל כרגע או טיפל בעבר בכל קריאה
        bool hasActiveOrPastAssignments = assignments.Any(a =>
            CallManager.GetCallStatus(a.CallId) == BO.StatusOfCall.InProgress ||
             CallManager.GetCallStatus(a.CallId) == BO.StatusOfCall.InProgressAtRisk ||
            CallManager.GetCallStatus(a.CallId) == BO.StatusOfCall.Open);

        // אם יש משימה פעילה או קריאה שטופלה בעבר, לא ניתן למחוק
        if (hasActiveOrPastAssignments)
            return false;

        // אם לא נמצאו משימות פעילה, אפשר למחוק
        return true;
    }
}

