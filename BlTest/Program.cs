using DO;
using BO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System;

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
                        case BO.VolunteerMenu.enterSystem:
                            Console.WriteLine("Enter volunteer id:");
                            int id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter password (if nedded):");
                            string? password = Console.ReadLine();

                            DO.VolunteerRole role = s_bl.Volunteer.Login(id, password);
                            Console.WriteLine($"Login successful. Role: {role.ToString()}");


                            break;

                        case BO.VolunteerMenu.AddVolunteer:
                            var newVolunteer = GetVolunteerDetailsFromUser(null);
                            s_bl.Volunteer.AddVolunteer(newVolunteer);
                            Console.WriteLine("Volunteer added successfully.");
                            break;
                        case BO.VolunteerMenu.UpdateVolunteer:
                            Console.WriteLine("Enter your id!");
                            int updateId = int.Parse(Console.ReadLine());
                            var volunteerToUpdate = GetVolunteerDetailsFromUser(null);
                            try
                            {
                                s_bl.Volunteer.UpdateVolunteer(updateId, volunteerToUpdate);
                                Console.WriteLine("Volunteer updated successfully.");
                            }
                            catch (BlDoesNotExistException ex)
                            {
                                Console.WriteLine($"Update failed: {ex.Message}");
                            }
                            catch (BlNotValidException ex)
                            {
                                Console.WriteLine($"Update failed: {ex.Message}");
                            }
                            break;
                        case BO.VolunteerMenu.DeleteVolunteer:
                            Console.WriteLine("Enter volunteer id to delete:");
                            if (int.TryParse(Console.ReadLine(), out int deleteId))
                            {

                                s_bl.Volunteer.DeleteVolunteer(deleteId);
                                Console.WriteLine("Volunteer deleted successfully.");


                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid volunteer ID.");
                            }
                            break;
                        case BO.VolunteerMenu.GetVolunteerDetails:
                            Console.WriteLine("Enter volunteer id to get details:");
                            int detailsId = int.Parse(Console.ReadLine());

                            BO.Volunteer volunteerDetails = s_bl.Volunteer.VolunteerDetails(detailsId);
                            PrintVolunteerDetails(volunteerDetails);

                            break;
                        case BO.VolunteerMenu.GetAllVolunteers:
                            TestReadAllVolunteers();
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
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
        string input1, input2;
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
            Console.WriteLine("10 - Get call info");
            Console.WriteLine("11 - Amount of calls on every status");
            Console.WriteLine("0 - Exit");
            Console.Write("Please enter your choice: \n");

            input1 = Console.ReadLine();

            if (Enum.TryParse(input1, out choice))
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
                                //TO-DO check how to give the volunteer the id of the assignment
                            
                      
                      
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

                            Console.WriteLine("Choose how to filter the list");
                            Console.WriteLine("1  - get all calls");
                            Console.WriteLine("filter calls by call type:");
                            Console.WriteLine("2  - ProvideTransportation calls");
                            Console.WriteLine("3  - MedicalAssistance calls");
                            Console.WriteLine("4  - EmotionalSupport calls");
                            Console.WriteLine("5  - HomeCare calls");
                            Console.WriteLine("6  - HospitalVisit calls");
                            Console.WriteLine("7  - DeliverMedicine calls");
                            Console.WriteLine("8  - CleaningAssistance calls");
                            Console.WriteLine("filter calls by their status:");
                            Console.WriteLine("9  - open calls");
                            Console.WriteLine("10 - calls in progress");
                            Console.WriteLine("11 - closed calls");
                            Console.WriteLine("12 - expired calls");
                            Console.WriteLine("13 - calls open at risk ");
                            Console.WriteLine("14 - calls in progress at risk");
                            Console.WriteLine("15 - filter calls by amount of assigns");
                            input1 = Console.ReadLine();

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
                            input2 = Console.ReadLine();

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
                                        filterField = BO.CallInListField.Id;
                                        break;
                                    case 2:
                                        filterField = BO.CallInListField.CallId;
                                        break;
                                    case 3:
                                        filterField = BO.CallInListField.CallType;
                                        break;
                                    case 4:
                                        filterField = BO.CallInListField.OpeningTime;
                                        break;
                                    case 5:
                                        filterField = BO.CallInListField.TimeLeft;
                                        break;
                                    case 6:
                                        filterField = BO.CallInListField.LastVolunteerName;
                                        break;
                                    case 7:
                                        filterField = BO.CallInListField.HandlingTime;
                                        break;
                                    case 8:
                                        filterField = BO.CallInListField.status;
                                        break;
                                    case 9:
                                        filterField = BO.CallInListField.AmountOfAssigns;
                                        break;

                                }
                                IEnumerable<BO.CallInList> callLists = s_bl.Call.GetCallList(filterField, filterValue, sortField);


                                foreach (var call in callLists)
                                {
                                    Console.WriteLine($"Assignment id: {call.Id}");
                                    Console.WriteLine($"Call id: {call.CallId}");
                                    Console.WriteLine($"all type: {call.CallType}");
                                    Console.WriteLine($"time opened: {call.OpeningTime}");
                                    Console.WriteLine($"time to complete: {call.TimeLeft}");
                                    Console.WriteLine($"Name of last volunteer who chose this call: {call.LastVolunteerName}");
                                    Console.WriteLine($"Call status: {call.status}");
                                    Console.WriteLine($"number of assignments this call had: {call.AmountOfAssigns}");

                                }
                            }
                            break;



                        case BO.CallMenuOptions.GetOpenCallsForVolunteer:


                            Console.WriteLine("Enter the volunteer id for getting the open calls:");
                            volunteerId = int.Parse(Console.ReadLine());

                            Console.WriteLine("Choose how to filter the list");
                            Console.WriteLine("1  - get all calls");
                            Console.WriteLine("filter calls by call type:");
                            Console.WriteLine("2  - ProvideTransportation calls");
                            Console.WriteLine("3  - MedicalAssistance calls");
                            Console.WriteLine("4  - EmotionalSupport calls");
                            Console.WriteLine("5  - HomeCare calls");
                            Console.WriteLine("6  - HospitalVisit calls");
                            Console.WriteLine("7  - DeliverMedicine calls");
                            Console.WriteLine("8  - CleaningAssistance calls");
                            input1 = Console.ReadLine();

                            Console.WriteLine("Choose how to order the list:");
                            Console.WriteLine("1 - order by call id");
                            Console.WriteLine("2 - order by call type");
                            Console.WriteLine("3 - order by call address");
                            Console.WriteLine("4 - order by time opened");
                            Console.WriteLine("5 - order by time left"); // OpenCallInListField.MaxTime => call.MaxTime(יכול להיות טעות),
                            Console.WriteLine("6 - order by call distance from volunteer");
                            input2 = Console.ReadLine();


                            if (int.TryParse(input1, out filterChoice) && int.TryParse(input2, out sortChoice))
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
                                foreach (var call in openCallsInList)
                                {
                                    Console.WriteLine($"Call id: {call.Id}");
                                    Console.WriteLine($"Call type: {call.CallType}");
                                    Console.WriteLine($"Call description: {call.Description}");
                                    Console.WriteLine($"Call address: {call.Address}");
                                    Console.WriteLine($"Time opened: {call.OpeningTime}");
                                    Console.WriteLine($"Total time to complete:  {call.MaxTime}");
                                    Console.WriteLine($"Call distance:  {call.DistanceFromVolunteer}");

                                }
                            }
                            break;



                        case BO.CallMenuOptions.GetClosedCallsByVolunteer:
                            Console.WriteLine("Enter the volunteer id for getting the closed calls:");
                            volunteerId = int.Parse(Console.ReadLine());

                            Console.WriteLine("Choose how to filter the list");
                            Console.WriteLine("1  - get all calls");
                            Console.WriteLine("filter calls by call type:");
                            Console.WriteLine("2  - ProvideTransportation calls");
                            Console.WriteLine("3  - MedicalAssistance calls");
                            Console.WriteLine("4  - EmotionalSupport calls");
                            Console.WriteLine("5  - HomeCare calls");
                            Console.WriteLine("6  - HospitalVisit calls");
                            Console.WriteLine("7  - DeliverMedicine calls");
                            Console.WriteLine("8  - CleaningAssistance calls");
                            input1 = Console.ReadLine();

                            Console.WriteLine("Choose how to order the list:");
                            Console.WriteLine("1 - order by call id");
                            Console.WriteLine("2 - order by call type");
                            Console.WriteLine("3 - order by call address");
                            Console.WriteLine("4 - order by time opened");
                            Console.WriteLine("5 - order by Hndal Start time"); // OpenCallInListField.MaxTime => call.MaxTime(יכול להיות טעות),
                            Console.WriteLine("6 - order by call end time");
                            Console.WriteLine("7 - order by call closure type");
                            input2 = Console.ReadLine();


                            if (int.TryParse(input1, out filterChoice) && int.TryParse(input2, out sortChoice))
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
                                foreach (var call in closedCallInLists)
                                {

                                    Console.WriteLine($"Call id: {call.Id}");
                                    Console.WriteLine($"Call type: {call.CallType}");
                                    Console.WriteLine($"Call address: {call.Address}");
                                    Console.WriteLine($"Time opened: {call.OpeningTime}");
                                    Console.WriteLine($"Time assigned: {call.HndalStartTime}");
                                    Console.WriteLine($"Time completed: {call.EndTime}");
                                    Console.WriteLine($"End handeling type: {call.ClosureType}");

                                }
                            }
                            break;

                        case BO.CallMenuOptions.GetCallDetails:

                            Console.WriteLine("Enter call id:");
                            callId = int.Parse(Console.ReadLine());
                                BO.Call boCall = s_bl.Call.GetCallDetails(callId);

                                Console.WriteLine($"ID: {boCall.Id}");
                                Console.WriteLine($"CallType: {boCall.CallType}");
                                Console.WriteLine($"Description: {boCall.Description}");
                                Console.WriteLine($"Address: {boCall.Address}");
                                Console.WriteLine($"Latitude: {boCall.Latitude}");
                                Console.WriteLine($"Longitude: {boCall.Longitude}");
                                Console.WriteLine($"OpeningTime: {boCall.OpeningTime}");
                                Console.WriteLine($"MaxTime: {boCall.MaxTime}");
                                Console.WriteLine($"Status: {boCall.status}");
                                Console.WriteLine($"Status: {boCall.status}");
                                Console.WriteLine(" CallAssignedInList:");
                                PrintCallAssignedInList(boCall);
                            

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
                catch (BlAssignmentIsNotActive ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    break;
                }
                catch (BlDoesNotExistException ex)
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
                switch (choice)
                {
                    case BO.AdminMenu.Exit:
                        break;
                    case BO.AdminMenu.AdvanceClock:
                        Console.WriteLine("Enter time unit to advance (seconds, minutes, hours):");
                        if (Enum.TryParse(Console.ReadLine(), out BO.TimeUnit timeUnit))
                        {
                            try
                            {
                                s_bl.Admin.ForwardClock(timeUnit);
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Invalid argument: {ex.Message}");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Operation failed: {ex.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid time unit.");
                        }
                        break;
                    case BO.AdminMenu.GetMaxRange:
                        try
                        {
                            TimeSpan riskRange = s_bl.Admin.GetMaxRange();
                            Console.WriteLine($"Risk range: {riskRange}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Operation failed: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        break;
                    case BO.AdminMenu.GetClock:
                        try
                        {
                            DateTime Time = s_bl.Admin.GetClock();
                            Console.WriteLine($"Current time: {Time}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Operation failed: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        break;
                    case BO.AdminMenu.InitializeDB:
                        try
                        {
                            s_bl.Admin.InitializeDB();
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Operation failed: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        break;
                    case BO.AdminMenu.ResetDB:
                        try
                        {
                            s_bl.Admin.ResetDB();
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Operation failed: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        break;
                    case BO.AdminMenu.SetMaxRange:
                        Console.WriteLine("Enter new risk range (in format hh:mm:ss):");
                        if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan newRiskRange))
                        {
                            try
                            {
                                s_bl.Admin.SetMaxRange(newRiskRange);
                                Console.WriteLine("Risk range updated successfully.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Invalid argument: {ex.Message}");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Operation failed: {ex.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid risk range format.");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        } while (choice != BO.AdminMenu.Exit);
    }

    private static void TestReadAllVolunteers()
    {
        Console.WriteLine("Would you like to filter by activity status? (yes/no)");
        string filterInput = Console.ReadLine();
        bool? isActiveFilter = null;
        if (filterInput.Equals("yes", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter activity status (true for active, false for inactive):");
            if (bool.TryParse(Console.ReadLine(), out bool isActive))
            {
                isActiveFilter = isActive;
            }
        }

        Console.WriteLine("Choose a property to sort by:");
        Console.WriteLine("1. Id");
        Console.WriteLine("2. FullName");
        Console.WriteLine("3. IsActive");
        Console.WriteLine("4. NumCallsTreated");
        Console.WriteLine("5. NumCallsCancelled");
        Console.WriteLine("6. NumOfCallsExpired");
        Console.WriteLine("7. AssignedCallId");
        Console.WriteLine("0. No sorting");

        BO.VolunteerProperty? sortByProperty = null;
        if (int.TryParse(Console.ReadLine(), out int sortOption))
        {
            sortByProperty = sortOption switch
            {
                1 => BO.VolunteerProperty.Id,
                2 => BO.VolunteerProperty.FullName,
                3 => BO.VolunteerProperty.IsActive,
                4 => BO.VolunteerProperty.NumCallsTreated,
                5 => BO.VolunteerProperty.NumCallsCancelled,
                6 => BO.VolunteerProperty.NumOfCallsExpired,
                7 => BO.VolunteerProperty.AssignedCallId,
                _ => null
            };
        }

        try
        {
            var allVolunteers = s_bl.Volunteer.ReadAll(isActiveFilter, sortByProperty);
            foreach (var volunteer in allVolunteers)
            {
                PrintVolunteerInListDetails(volunteer);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Get all volunteers failed: {ex.Message}");
        }
    }

    private static void PrintCallAssignedInList(BO.Call BOCall)
    {
        if (BOCall.CurrentCallAssignments.Count() == 0) Console.WriteLine("No assignments exists for this call");
        foreach (BO.CallAssignInList call in BOCall.CurrentCallAssignments)
        {
            Console.WriteLine($"Volunteer id: {call.VolunteerId}");
            Console.WriteLine($"Volunteer name: {call.VolunteerName}");
            Console.WriteLine($"Time opened: {call.TimeOpened}");
            Console.WriteLine($"Time completed: {call.TimeCompleted}");
            Console.WriteLine($"Type of end care: {call.typeOfEndCare}");

        }
    }



    static BO.Call CreateBoCallFromInput(int? callId)
    {
        if (callId == null)
        {
            callId = 0;//dosent matter wich value becuse the ID will be aoutomaticly generated
        }
        int convertedId = (int)callId;
        Console.WriteLine("Enter call type (food, firstAid, weapons, fuel):");
        BO.CallType callType = Enum.Parse<BO.CallType>(Console.ReadLine(), true);

        Console.WriteLine("Enter call description (optional):");
        string? callDescription = Console.ReadLine();

        Console.WriteLine("Enter call address:");
        string callAddress = Console.ReadLine();

        Console.WriteLine("Enter time opened (yyyy-MM-dd HH:mm:ss):");
        DateTime timeOpened = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Enter time to complete (yyyy-MM-dd HH:mm:ss) (optional):");
        string timeToCompleteInput = Console.ReadLine();
        DateTime? timeToComplete = string.IsNullOrEmpty(timeToCompleteInput) ? (DateTime?)null : DateTime.Parse(timeToCompleteInput);
        return new BO.Call()
        {
            Id = convertedId,
            CallType = callType,
            Description = callDescription,
            Address = callAddress,
            OpeningTime = timeOpened,
            MaxTime = timeToComplete,
            status = StatusOfCall.Open,
            CallAssignsList = null
        };

    }
    public static void PrintVolunteerDetails(BO.Volunteer volunteer)
    {
        Console.WriteLine($"Volunteer id: {volunteer.Id}");
        Console.WriteLine($"Full name: {volunteer.fullName}");
        Console.WriteLine($"Phone number: {volunteer.phoneNumber}");
        Console.WriteLine($"Email: {volunteer.eMail}");
        Console.WriteLine($"Is active: {volunteer.IsActive}");
        Console.WriteLine($"Current address: {volunteer.currentFullAddress}");
        Console.WriteLine($"Latitude: {volunteer.Latitude}");
        Console.WriteLine($"Longitude: {volunteer.Longitude}");
        Console.WriteLine($"Role: {volunteer.role}");
        Console.WriteLine($"Max distance: {volunteer.maxDistance}");
        Console.WriteLine($"Distance type: {volunteer.destinationType}");
        Console.WriteLine($"Number of calls treated: {volunteer.caredCalls}");
        Console.WriteLine($"Number of calls cancelled: {volunteer.cancelledCalls}");
        Console.WriteLine($"Number of calls expired: {volunteer.expiredCalls}");
        Console.WriteLine($"Assigned call id: {volunteer.callInProgres?.CallId}");
        if (volunteer.callInProgres != null)
        {
            Console.WriteLine("Current call in progress:");
            Console.WriteLine($"Call id: {volunteer.callInProgres.CallId}");
            Console.WriteLine($"Call type: {volunteer.callInProgres.CallType}");
            Console.WriteLine($"Call description: {volunteer.callInProgres.CallDescription}");
            Console.WriteLine($"Call address: {volunteer.callInProgres.CallAddress}");
            Console.WriteLine($"Time opened: {volunteer.callInProgres.TimeOpened}");
            Console.WriteLine($"Time to complete: {volunteer.callInProgres.TimeToComplete}");
            Console.WriteLine($"Time assigned: {volunteer.callInProgres.TimeAssigned}");
            Console.WriteLine($"Call distance: {volunteer.callInProgres.CallDistance}");
            Console.WriteLine($"Call status: {volunteer.callInProgres.Status}");
        }

    }
    public static void PrintVolunteerInListDetails(BO.VolunteerInList volunteer)
    {
        Console.WriteLine($"Volunteer id: {volunteer.Id}");
        Console.WriteLine($"Full name: {volunteer.FullName}");
        Console.WriteLine($"Is active: {volunteer.IsActive}");
        Console.WriteLine($"Number of calls treated: {volunteer.NumCallsTreated}");
        Console.WriteLine($"Number of calls cancelled: {volunteer.NumCallsCancelled}");
        Console.WriteLine($"Number of calls expired: {volunteer.NumOfCallsExpired}");
        Console.WriteLine($"Assigned call id: {volunteer.AssignedCallId}");
        Console.WriteLine($"Call type: {volunteer.callType}");
    }

    //before we start the program, we need to make sure that the statuses of the call ar syncronized 
    //with the assignment TypeOfEndCare enum.
    private static void VerifyStatusValidation()
    {

    }

    /// <summary>
    /// Prompts the user to enter volunteer details and returns a Volunteer object.
    /// </summary>
    /// <param name="idToUpdate">The ID of the volunteer to update, or null to create a new volunteer.</param>
    /// <returns>A Volunteer object with the entered details.</returns>
    public static BO.Volunteer GetVolunteerDetailsFromUser(int? idToUpdate)
    {
        Console.WriteLine("Enter Volunteer Details:");
        int id = idToUpdate ?? 0;
        if (idToUpdate == null)
        {
            Console.Write("ID: ");
            int.TryParse(Console.ReadLine(), out id);
        }

        Console.Write("Full Name: ");
        string fullName = Console.ReadLine();

        Console.Write("Phone Number: ");
        string phoneNumber = Console.ReadLine();

        Console.Write("Email: ");
        string eMail = Console.ReadLine();

        Console.Write("Is Active (true/false): ");
        bool.TryParse(Console.ReadLine(), out bool isActive);

        Console.Write("Role (0 for Manager, 1 for Volunteer): ");
        int.TryParse(Console.ReadLine(), out int roleInt);
        BO.VolunteerRole role = (VolunteerRole)roleInt;

        Console.Write("Destination Type (0 for WalkingDistance, 1 for DrivingDistance, 2 for FlightDistance): ");
        int.TryParse(Console.ReadLine(), out int destinationTypeInt);
        TypeOfDist destinationType = (TypeOfDist)destinationTypeInt;

        Console.Write("Password (optional, press Enter to skip): ");
        string? password = Console.ReadLine();

        Console.Write("Current Full Address (optional, press Enter to skip): ");
        string? currentFullAddress = Console.ReadLine();

        Console.Write("Max Distance (optional, press Enter to skip): ");
        string maxDistanceInput = Console.ReadLine();
        double.TryParse(maxDistanceInput, out double maxDistance);

        BO.Volunteer volunteer = new BO.Volunteer
        {
            Id = id,
            fullName = fullName,
            phoneNumber = phoneNumber,
            eMail = eMail ?? "",
            IsActive = isActive,
            role = role,
            destinationType = destinationType,
            password = password,
            currentFullAddress = currentFullAddress,
            maxDistance = maxDistance == 0 ? (double?)null : maxDistance
        };
        return volunteer;
    }


}