using DO;
using BO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BlTest;


internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        BO.MainMenuOptions choice;
        do
        {

            Console.WriteLine("Main manu");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Volunteer entity menu");
            Console.WriteLine("2 - Call entity menu");
            Console.WriteLine("3 - Admin menu");
            Console.Write("Please enter your choice: \n");
            string input = Console.ReadLine();

            if (Enum.TryParse(input, out choice))
            {
                try
                {
                    switch (choice)
                    {

                        case BO.MainMenuOptions.Exit:
                            return;
                        case BO.MainMenuOptions.VolunteerMenu:

                            VolunteerManu();
                            break;
                        case BO.MainMenuOptions.CallMenu:

                            CallManu();
                            break;
                        case BO.MainMenuOptions.AdminMenu:

                            AdminManu();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }

        } while (choice != BO.MainMenuOptions.Exit);
    }

    private static void VolunteerManu()
    {
        BO.VolunteerMenu choice;
        do
        {
            Console.WriteLine("Volunteer Menu");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Add volunteer");
            Console.WriteLine("3 - Update volunteer");
            Console.WriteLine("4 - Delete volunteer");
            Console.WriteLine("5 - Get volunteer details");
            Console.WriteLine("6 - Get all volunteers");
            Console.Write("Please enter your choice: \n");

            string input = Console.ReadLine();
            if (Enum.TryParse(input, out choice))
            {
                try
                {
                    switch (choice)
                    {
                        case BO.VolunteerMenu.Exit:
                            break;
                        case BO.VolunteerMenu.EnterToTheSystem:

                            Console.WriteLine("Enter volunteer name:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter password (if nedded):");
                            string? password = Console.ReadLine();
                            BO.job jub = s_bl.Volunteer.EnterToTheSystem(name, password,null);
                            Console.WriteLine("Enter to the system successfully");
                            break;

                        case BO.VolunteerMenu.Create:

                            var newVolunteer = GetVolunteerDetails(null);
                            s_bl.Volunteer.Create(newVolunteer);
                            Console.WriteLine("Volunteer added successfully");
                            break;
                        case BO.VolunteerMenu.Update:

                            Console.WriteLine("Enter your id");
                            int updateId = int.Parse(Console.ReadLine());
                            var volunteerToUpdate = GetVolunteerDetails(null);
                            s_bl.Volunteer.Update(volunteerToUpdate, updateId);
                            Console.WriteLine("Volunteer updated successfully.");


                            break;
                        case BO.VolunteerMenu.Delete:

                            Console.WriteLine("Enter volunteer id to delete:");
                            int.TryParse(Console.ReadLine(), out int deleteId);
                            s_bl.Volunteer.Delete(deleteId);
                            Console.WriteLine("Volunteer deleted successfully");


                            break;
                        case BO.VolunteerMenu.Read:

                            Console.WriteLine("Enter volunteer id to get details:");
                            int volunteerId = int.Parse(Console.ReadLine());
                            BO.Volunteer volunteerDetails = s_bl.Volunteer.Read(volunteerId);
                            Console.WriteLine(volunteerDetails); ;

                            break;
                        case BO.VolunteerMenu.ReadAll:
                            GetVolunteersList();
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }


                }
                catch (BlDeletionImpossible ex)
                {
                    Console.WriteLine($"Delete failed: {ex.Message}");
                }
                catch (BlDoesNotExistException ex)
                {
                    Console.WriteLine($"Get details failed: {ex.Message}");
                }
                catch (BlNotValidException ex)
                {
                    Console.WriteLine($"Update failed: {ex.Message}");
                }
                catch (BlAlreadyExistsException ex)
                {
                    Console.WriteLine($"Add failed: {ex.Message}");
                }
            }
        } while (choice != BO.VolunteerMenu.Exit);

    }


    private static void CallManu()
    {
        int volunteerId, callId, assignmentId;
        string input;
        BO.CallMenuOptions choice;
        do
        {
            Console.WriteLine("Call Menu");
            Console.WriteLine("1 - choose call for Handl");
            Console.WriteLine("2 - End call");
            Console.WriteLine("3 - Cancel call");
            Console.WriteLine("4 - Update call details");
            Console.WriteLine("5 - Add a new call");
            Console.WriteLine("6 - Delete call");
            Console.WriteLine("7 - Get call list in database");
            Console.WriteLine("8 - Get open calls of a volunteer");
            Console.WriteLine("9 - Get all closed calls of a volunteer");
            Console.WriteLine("10 - Get call details");
            Console.WriteLine("11 - Amount of calls on every status");
            Console.WriteLine("0 - Exit");
            Console.Write("Please enter your choice: \n");

            input = Console.ReadLine();

            if (Enum.TryParse(input, out choice))
            {

                try
                {

                    switch (choice)
                    {
                        case BO.CallMenuOptions.Exit:
                            break;
                        case BO.CallMenuOptions.ChoosingCallForHandling:

                            Console.WriteLine("Enter the volunteer id:");
                            volunteerId = int.Parse(Console.ReadLine());

                            Console.WriteLine("Enter the call id to Handl:");
                            callId = int.Parse(Console.ReadLine());

                            s_bl.Call.ChoosingCallForHandling(volunteerId, callId);

                            break;

                        case BO.CallMenuOptions.EndHandling:

                            Console.WriteLine("Enter the volunteer id:");
                            volunteerId = int.Parse(Console.ReadLine());

                            Console.WriteLine("Enter the assignment id to end:");
                            assignmentId = int.Parse(Console.ReadLine());

                            s_bl.Call.EndHandling(volunteerId, assignmentId);


                            break;
                        case BO.CallMenuOptions.CancelHandling:

                            Console.WriteLine("Enter the volunteer id:");
                            volunteerId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the assignment id to cancel:");
                            assignmentId = int.Parse(Console.ReadLine());

                            s_bl.Call.CancelHandling(volunteerId, assignmentId);



                            break;

                        case BO.CallMenuOptions.UpdateCallDetails:

                            Console.WriteLine("Enter call id:");
                            callId = int.Parse(Console.ReadLine());
                            BO.Call BOCall = CreateBoCallFromInput(callId);
                            if (BOCall == null)
                            {
                                break;
                            }

                            s_bl.Call.UpdateCallDetails(BOCall);


                            break;
                        case BO.CallMenuOptions.AddCall:

                            BO.Call newBOCall = CreateBoCallFromInput(null);
                            s_bl.Call.AddCall(newBOCall);


                            break;
                        case BO.CallMenuOptions.DeleteCall:

                            Console.WriteLine("Enter the id of call to delete:");
                            callId = int.Parse(Console.ReadLine());

                            s_bl.Call.DeleteCall(callId);


                            break;
                        case BO.CallMenuOptions.GetCallList:
                            GetCallsList();
                            break;

                        case BO.CallMenuOptions.GetOpenCallsForVolunteer:

                            GetOpenCallsList();

                            break;

                        case BO.CallMenuOptions.GetClosedCallsByVolunteer:
                            GetClosedCallsList();

                            break;

                        case BO.CallMenuOptions.GetCallDetails:

                            Console.WriteLine("Enter call id:");
                            callId = int.Parse(Console.ReadLine());
                            BO.Call boCall = s_bl.Call.GetCallDetails(callId);
                            Console.WriteLine(boCall);



                            break;
                        case BO.CallMenuOptions.CallsAmount:
                            int[] amountOfCalls = s_bl.Call.CallsAmount();

                            Console.WriteLine("Number of calls for each status:");
                            Console.WriteLine($"Open calls: {amountOfCalls[0]}");
                            Console.WriteLine($"calls in progres: {amountOfCalls[1]}");
                            Console.WriteLine($"closed calls: {amountOfCalls[2]}");
                            Console.WriteLine($"expired calls: {amountOfCalls[3]}");
                            Console.WriteLine($"open at risk range calls: {amountOfCalls[4]}");
                            Console.WriteLine($"in progres at risk range calls: {amountOfCalls[5]}");
                            break;
                    }

                }
                catch (BlDoesNotExistException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (BlDeletionImpossible ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    break;
                }
                catch (BlNotValidException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    break;
                }

            }
        } while (choice != BO.CallMenuOptions.Exit);

    }





    private static void AdminManu()
    {
        string input;
        BO.AdminMenu choice;
        do
        {
            Console.WriteLine("Admin menu");
            Console.WriteLine("0 - exit");
            Console.WriteLine("1 - Advance clock");
            Console.WriteLine("2 - Get risk range");
            Console.WriteLine("3 - Get time ");
            Console.WriteLine("4 - Initialize database");
            Console.WriteLine("5 - Reset database");
            Console.WriteLine("6 - Update risk range");
            input = Console.ReadLine();

            if (Enum.TryParse(input, out choice))
            {

                try
                {

                    switch (choice)
                    {
                        case BO.AdminMenu.Exit:
                            break;
                        case BO.AdminMenu.AdvanceClock:

                            Console.WriteLine("Enter time unit to advance ( Minute, Hour, Day, Month, Year):");
                            if (Enum.TryParse(Console.ReadLine(), out BO.TimeUnit timeUnit))
                            {    s_bl.Admin.ForwardClock(timeUnit);
                                Console.WriteLine("time added successfully");
                            }
                            else
                                Console.WriteLine("Invalid time unit.");



                            break;
                        case BO.AdminMenu.GetMaxRange:

                            TimeSpan riskRange = s_bl.Admin.GetMaxRange();
                            Console.WriteLine($"Risk range: {riskRange}");


                            break;
                        case BO.AdminMenu.GetClock:

                            DateTime Time = s_bl.Admin.GetClock();
                            Console.WriteLine($"Current time: {Time}");


                            break;
                        case BO.AdminMenu.InitializeDB:

                            s_bl.Admin.InitializeDB();

                            break;
                        case BO.AdminMenu.ResetDB:
                            s_bl.Admin.ResetDB();
                            Console.WriteLine("Data Reset successfully");
                            break;

                        case BO.AdminMenu.SetMaxRange:
                            Console.WriteLine("Enter new risk range (in format hh:mm:ss):");
                            if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan newRiskRange))
                            {

                                s_bl.Admin.SetMaxRange(newRiskRange);
                                Console.WriteLine("Risk range updated successfully.");
                            }

                            else
                            {
                                Console.WriteLine("Invalid risk range format.");
                            }
                            break;
                    }


                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Invalid argument: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Operation failed: {ex.Message}");
                }


            }

        } while (choice != BO.AdminMenu.Exit);




    }


    /// <summary>
    /// Displays a menu to filter and sort a list of calls and outputs the results.
    /// </summary>
    /// <remarks>
    /// The method interacts with the user to get their choice for filtering and sorting.
    /// It then retrieves the list of calls from the business logic layer based on the selected options.
    /// </remarks>
    private static void GetCallsList()
    {
        Console.WriteLine("Choose how to filter the list");
        Console.WriteLine("1  - get all calls");
        Console.WriteLine("2  - ProvideTransportation calls");
        Console.WriteLine("3  - MedicalAssistance calls");
        Console.WriteLine("4  - EmotionalSupport calls");
        Console.WriteLine("5  - HomeCare calls");
        Console.WriteLine("6  - HospitalVisit calls");
        Console.WriteLine("7  - DeliverMedicine calls");
        Console.WriteLine("8  - CleaningAssistance calls");
        Console.WriteLine("9  - open calls");
        Console.WriteLine("10 - calls in progress");
        Console.WriteLine("11 - closed calls");
        Console.WriteLine("12 - expired calls");
        Console.WriteLine("13 - calls open at risk ");
        Console.WriteLine("14 - calls in progress at risk");
        Console.WriteLine("15 - filter calls by amount of assigns");
        string input1 = Console.ReadLine();

        Console.WriteLine("Choose how to order the list:");
        Console.WriteLine("1 - order by assignment Id");
        Console.WriteLine("2 - order by call Id");
        Console.WriteLine("3 - order by call type");
        Console.WriteLine("4 - order by time opened");
        Console.WriteLine("5 - order by time left");
        Console.WriteLine("6 - order by last volunteer name");
        Console.WriteLine("7 - order by handling time");
        Console.WriteLine("8 - order by status");
        Console.WriteLine("9 - order by amount of assigns");
        string input2 = Console.ReadLine();

        if (int.TryParse(input1, out int filterChoice) && int.TryParse(input2, out int sortChoice))
        {
            BO.CallInListField? filterField = null;
            object? filterValue = null;
            BO.CallInListField? sortField = null;
            switch (filterChoice)
            {
                case 1:
                    break;
                case 2:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.ProvideTransportation;
                    break;
                case 3:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.MedicalAssistance;
                    break;
                case 4:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.EmotionalSupport;
                    break;
                case 5:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.HomeCare;
                    break;
                case 6:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.HospitalVisit;
                    break;
                case 7:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.DeliverMedicine;
                    break;
                case 8:
                    filterField = BO.CallInListField.CallType;
                    filterValue = BO.CallType.CleaningAssistance;
                    break;
                case 9:
                    filterField = BO.CallInListField.status;
                    filterValue = BO.StatusOfCall.Open;
                    break;
                case 10:
                    filterField = BO.CallInListField.status;
                    filterValue = BO.StatusOfCall.Open;
                    break;
                case 11:
                    filterField = BO.CallInListField.status;
                    filterValue = BO.StatusOfCall.Open;
                    break;
                case 12:
                    filterField = BO.CallInListField.status;
                    filterValue = BO.StatusOfCall.Open;
                    break;
                case 13:
                    filterField = BO.CallInListField.status;
                    filterValue = BO.StatusOfCall.Open;
                    break;
                case 14:
                    filterField = BO.CallInListField.status;
                    filterValue = BO.StatusOfCall.Open;
                    break;
                case 15:
                    filterField = BO.CallInListField.AmountOfAssigns;
                    Console.WriteLine("Enter the amount of assigns:");
                    filterValue = int.Parse(Console.ReadLine());
                    break;
            }
            switch (sortChoice)
            {
                case 1:
                    sortField = BO.CallInListField.AssignmentId;
                    break;
                case 2:
                    sortField = BO.CallInListField.CallId;
                    break;
                case 3:
                    sortField = BO.CallInListField.CallType;
                    break;
                case 4:
                    sortField = BO.CallInListField.OpeningTime;
                    break;
                case 5:
                    sortField = BO.CallInListField.TimeLeft;
                    break;
                case 6:
                    sortField = BO.CallInListField.LastVolunteerName;
                    break;
                case 7:
                    sortField = BO.CallInListField.HandlingTime;
                    break;
                case 8:
                    sortField = BO.CallInListField.status;
                    break;
                case 9:
                    sortField = BO.CallInListField.AmountOfAssigns;
                    break;

            }
            IEnumerable<BO.CallInList> callList = s_bl.Call.GetCallList(filterField, filterValue, sortField);
            if (callList.Count() == 0 || callList == null)
            {
                Console.WriteLine("No  calls found.");
            }
            else
                foreach (BO.CallInList call in callList)
                {
                    Console.WriteLine(call);

                }
        }

    }


    /// <summary>
    /// Displays a menu to filter and sort a list of Open Calls and outputs the results.
    /// </summary>
    /// <remarks>
    /// The method interacts with the user to get their choice for filtering and sorting.
    /// It then retrieves the list of Open Calls from the business logic layer based on the selected options.
    /// </remarks>
    private static void GetOpenCallsList()
    {

        Console.WriteLine("Enter the volunteer id for getting the open calls:");
        int volunteerId = int.Parse(Console.ReadLine());

        Console.WriteLine("Choose how to filter the list");
        Console.WriteLine("1  - get all calls");
        Console.WriteLine("2  - ProvideTransportation calls");
        Console.WriteLine("3  - MedicalAssistance calls");
        Console.WriteLine("4  - EmotionalSupport calls");
        Console.WriteLine("5  - HomeCare calls");
        Console.WriteLine("6  - HospitalVisit calls");
        Console.WriteLine("7  - DeliverMedicine calls");
        Console.WriteLine("8  - CleaningAssistance calls");
        string input1 = Console.ReadLine();

        Console.WriteLine("Choose how to order the list:");
        Console.WriteLine("1 - order by call id");
        Console.WriteLine("2 - order by call type");
        Console.WriteLine("3 - order by call address");
        Console.WriteLine("4 - order by time opened");
        Console.WriteLine("5 - order by time left");
        Console.WriteLine("6 - order by call distance from volunteer");
        string input2 = Console.ReadLine();


        if (int.TryParse(input1, out int filterChoice) && int.TryParse(input2, out int sortChoice))
        {
            BO.CallType? callType = null;
            OpenCallInListField? sortField = null;
            switch (filterChoice)
            {
                case 1:
                    break;
                case 2:
                    callType = BO.CallType.ProvideTransportation;
                    break;
                case 3:
                    callType = BO.CallType.MedicalAssistance;
                    break;
                case 4:
                    callType = BO.CallType.EmotionalSupport;
                    break;
                case 5:
                    callType = BO.CallType.HomeCare;
                    break;
                case 6:
                    callType = BO.CallType.HospitalVisit;
                    break;
                case 7:
                    callType = BO.CallType.DeliverMedicine;
                    break;
                case 8:
                    callType = BO.CallType.CleaningAssistance;
                    break;

            }
            switch (sortChoice)
            {
                case 1:
                    sortField = OpenCallInListField.Id;
                    break;
                case 2:
                    sortField = OpenCallInListField.CallType;
                    break;
                case 3:
                    sortField = OpenCallInListField.Address;
                    break;
                case 4:
                    sortField = OpenCallInListField.OpeningTime;
                    break;
                case 5:
                    sortField = OpenCallInListField.MaxTime;
                    break;
                case 6:
                    sortField = OpenCallInListField.DistanceFromVolunteer;
                    break;


            }
            IEnumerable<BO.OpenCallInList> openCallsInList = s_bl.Call.GetOpenCallsForVolunteer(volunteerId, callType, sortField);
            if (openCallsInList.Count() == 0 || openCallsInList == null)
            {
                Console.WriteLine("No open calls found.");
            }
            else
                foreach (var call in openCallsInList)
                {
                    Console.WriteLine(call);

                }
        }

    }


    /// <summary>
    /// Displays a menu to filter and sort a list of Closed Calls and outputs the results.
    /// </summary>
    /// <remarks>
    /// The method interacts with the user to get their choice for filtering and sorting.
    /// It then retrieves the list of Closed Calls from the business logic layer based on the selected options.
    /// </remarks>
    private static void GetClosedCallsList()
    {

        Console.WriteLine("Enter the volunteer id for getting the closed calls:");
        int volunteerId = int.Parse(Console.ReadLine());

        Console.WriteLine("Choose how to filter the list");
        Console.WriteLine("1  - get all calls");
        Console.WriteLine("2  - ProvideTransportation calls");
        Console.WriteLine("3  - MedicalAssistance calls");
        Console.WriteLine("4  - EmotionalSupport calls");
        Console.WriteLine("5  - HomeCare calls");
        Console.WriteLine("6  - HospitalVisit calls");
        Console.WriteLine("7  - DeliverMedicine calls");
        Console.WriteLine("8  - CleaningAssistance calls");
        string input1 = Console.ReadLine();

        Console.WriteLine("Choose how to order the list:");
        Console.WriteLine("1 - order by call id");
        Console.WriteLine("2 - order by call type");
        Console.WriteLine("3 - order by call address");
        Console.WriteLine("4 - order by time opened");
        Console.WriteLine("5 - order by Hndal Start time"); // OpenCallInListField.MaxTime => call.MaxTime(יכול להיות טעות),
        Console.WriteLine("6 - order by call end time");
        Console.WriteLine("7 - order by call closure type");
        string input2 = Console.ReadLine();


        if (int.TryParse(input1, out int filterChoice) && int.TryParse(input2, out int sortChoice))
        {
            BO.CallType? callType = null;
            ClosedCallInListField? sortField = null;
            switch (filterChoice)
            {
                case 1:
                    break;
                case 2:
                    callType = BO.CallType.ProvideTransportation;
                    break;
                case 3:
                    callType = BO.CallType.MedicalAssistance;
                    break;
                case 4:
                    callType = BO.CallType.EmotionalSupport;
                    break;
                case 5:
                    callType = BO.CallType.HomeCare;
                    break;
                case 6:
                    callType = BO.CallType.HospitalVisit;
                    break;
                case 7:
                    callType = BO.CallType.DeliverMedicine;
                    break;
                case 8:
                    callType = BO.CallType.CleaningAssistance;
                    break;

            }
            switch (sortChoice)
            {
                case 1:
                    sortField = ClosedCallInListField.Id;
                    break;
                case 2:
                    sortField = ClosedCallInListField.CallType;
                    break;
                case 3:
                    sortField = ClosedCallInListField.Address;
                    break;
                case 4:
                    sortField = ClosedCallInListField.OpeningTime;
                    break;
                case 5:
                    sortField = ClosedCallInListField.HndalStartTime;
                    break;
                case 6:
                    sortField = ClosedCallInListField.EndTime;
                    break;
                case 7:
                    sortField = ClosedCallInListField.ClosureType;
                    break;

            }

            IEnumerable<BO.ClosedCallInList> closedCallInLists = s_bl.Call.GetClosedCallsByVolunteer(volunteerId, callType, sortField);
            if (closedCallInLists.Count() == 0 || closedCallInLists == null)
            {
                Console.WriteLine("No closed calls found.");
            }
            else
                foreach (var call in closedCallInLists)
                {
                    Console.WriteLine(call);

                }
        }

    }



    /// <summary>
    /// Displays a menu to filter and sort a list of volunteers and outputs the results.
    /// </summary>
    /// <remarks>
    /// The method interacts with the user to get their choice for filtering and sorting.
    /// It then retrieves the list of volunteers from the business logic layer based on the selected options.
    /// </remarks>
    private static void GetVolunteersList()
    {
        Console.WriteLine("Choose if to filter:");
        Console.WriteLine("1 - All volunteers");
        Console.WriteLine("2 - Only active volunteers");
        Console.WriteLine("3 - Only not active volunteers");

        string? filterInput = Console.ReadLine();

        Console.WriteLine("Choose how to sort:");
        Console.WriteLine("1 - by Id");
        Console.WriteLine("2 - by Name");
        Console.WriteLine("3 - by Activity");
        Console.WriteLine("4 - by Handled Calls");
        Console.WriteLine("5 - by Canceled Calls");
        Console.WriteLine("6 - by Expired Calls");
        Console.WriteLine("7 - by Current Call Id");
        Console.WriteLine("8 - by Current Call Type");
        Console.WriteLine("0 - No sorting");

        string? sortInput = Console.ReadLine();

        if (int.TryParse(filterInput, out int filterChoice) && int.TryParse(sortInput, out int sortChoice))
        {
            bool? isActiveFilter = null;
            BO.VolunteerInListField? sortByFild = null;
            switch (filterChoice)
            {
                case 0:
                case 1:
                    break;
                case 2:
                    isActiveFilter = true;
                    break;
                case 3:
                    isActiveFilter = false;
                    break;

            }
            switch (sortChoice)
            {
                case 0:
                    break;
                case 1:
                    sortByFild = BO.VolunteerInListField.Id;
                    break;
                case 2:
                    sortByFild = BO.VolunteerInListField.Name;
                    break;
                case 3:
                    sortByFild = BO.VolunteerInListField.IsActive;
                    break;
                case 4:
                    sortByFild = BO.VolunteerInListField.TotalHandledCalls;
                    break;
                case 5:
                    sortByFild = BO.VolunteerInListField.TotalCanceledCalls;
                    break;
                case 6:
                    sortByFild = BO.VolunteerInListField.TotalExpiredCalls;
                    break;
                case 7:
                    sortByFild = BO.VolunteerInListField.CurrentCallId;
                    break;
                case 8:
                    sortByFild = BO.VolunteerInListField.CurrentCallType;
                    break;

            }

            try
            {
                var allVolunteers = s_bl.Volunteer.ReadAll(isActiveFilter, sortByFild,null);
                foreach (var volunteer in allVolunteers)
                {
                    Console.WriteLine(volunteer); ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get all volunteers failed: {ex.Message}");
            }
        }

    }

    static BO.Call CreateBoCallFromInput(int? callId)
    {
        if (callId == null)
            callId = 0;

        int convertedId = (int)callId;
        Console.WriteLine("Enter call type from the list:");
        Console.WriteLine("ProvideTransportation");
        Console.WriteLine("MedicalAssistancen");
        Console.WriteLine("HomeCare");
        Console.WriteLine("HospitalVisit");
        Console.WriteLine("DeliverMedicine");
        Console.WriteLine("CleaningAssistance");

        BO.CallType callType = Enum.Parse<BO.CallType>(Console.ReadLine(), true);

        Console.WriteLine("Enter call description (optional):");
        string? callDescription = Console.ReadLine();

        Console.WriteLine("Enter call address:");
        string callAddress = Console.ReadLine();

        //Console.WriteLine("Enter time opened (yyyy-MM-dd HH:mm:ss):");
        //DateTime timeOpened = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Enter time to complete (yyyy-MM-dd HH:mm:ss) (optional):");
        string? timeToCompleteInput = Console.ReadLine();

        DateTime? timeToComplete = string.IsNullOrEmpty(timeToCompleteInput) ? (DateTime?)null : DateTime.Parse(timeToCompleteInput);
        return new BO.Call()
        {
            Id = convertedId,
            CallType = callType,
            Description = callDescription,
            Address = callAddress,
            OpeningTime = s_bl.Admin.GetClock(),
            MaxTime = timeToComplete,
            status = StatusOfCall.Open,
            CallAssignsList = null
        };

    }

    /// <summary>
    /// Prompts the user to enter volunteer details and returns a Volunteer object.
    /// </summary>
    /// <param name="oldId">The ID of the volunteer to update, or null while creating a new volunteer.</param>
    /// <returns>A Volunteer object with the entered details.</returns>
    public static BO.Volunteer GetVolunteerDetails(int? oldId)
    {
        Console.WriteLine("Enter Volunteer Details:");
        int id = oldId ?? 0;
        if (oldId == null)
        {
            Console.Write("ID: ");
            int.TryParse(Console.ReadLine(), out id);
        }

        Console.Write("Name (at least 2 letters):");
        string Name = Console.ReadLine();


        Console.Write("Password (optional): ");
        string? password = Console.ReadLine();

        Console.Write("Phone Number (10 numbers starting whith 0): ");
        string phoneNumber = Console.ReadLine();

        Console.Write("Email (in format - user@example.com): ");
        string email = Console.ReadLine();

        Console.Write("Is Active (true/false): ");
        bool.TryParse(Console.ReadLine(), out bool isActive);

        Console.Write("job (0 for Manager, 1 for Volunteer): ");
        int.TryParse(Console.ReadLine(), out int job);
        BO.job duty = (BO.job)job;

        Console.Write("Destination Type (0 for AirDistance, 1 for walkDistance, 2 for carDistance): ");
        int.TryParse(Console.ReadLine(), out int destinationTypeInt);
        BO.DistanceType destinationType = (BO.DistanceType)destinationTypeInt;


        Console.Write("Current Full Address (at least 5 letters)(optional): ");
        string? currentFullAddress = Console.ReadLine();

        Console.Write("Max Distance (optional): ");
        string? DistanceInput = Console.ReadLine();
        double.TryParse(DistanceInput, out double maxDistance);

        BO.Volunteer volunteer = new BO.Volunteer
        {
            Id = id,
            Name = Name,
            PhoneNumber = phoneNumber,
            Email = email ?? "",
            Password = password,
            Address = currentFullAddress,
            Latitude = null,
            Longitude = null,
            Duty = duty,
            IsActive = isActive,
            Distance = destinationType,
            MaximumDistance = maxDistance,
        };
        return volunteer;
    }


}
