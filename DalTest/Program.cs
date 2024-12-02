using Dal;
using DalApi;
using DO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DalTest
{
    internal class Program
    {

        //static readonly IDal s_dal = new DalList();
        static readonly IDal s_dal = new DalXml(); //stage 3
        static void Main(string[] args)
        {

            int choice = 0;
            do
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Submenu for  Volunteer");
                Console.WriteLine("2 - Submenu for Call");
                Console.WriteLine("3 - Submenu for Assignment");
                Console.WriteLine("4 - Initialize Data");
                Console.WriteLine("5 - Show All Data");
                Console.WriteLine("6 - Submenu for Configuration");
                Console.WriteLine("7 - Reset Database");


                choice = int.Parse(Console.ReadLine() ?? "0");
                MainMenuOption selectedOption = (MainMenuOption)choice; // Convert the choice to enum


                switch (selectedOption)
                {
                    case MainMenuOption.Exit:
                        Console.WriteLine("Exiting...");
                        break;

                    case MainMenuOption.VolunteerMenu:                    
                            volunteerMenu();                     
                        break;

                    case MainMenuOption.CallMenu:                    
                        callMenu();
                        break;

                    case MainMenuOption.AssignmentMenu:                      
                        AssignmentMenu();
                        break;

                    case MainMenuOption.InitializeData:
                        Initialization.Do(s_dal);
                        break;

                    case MainMenuOption.ShowAllData:                     
                        showData("All");
                        break;

                    case MainMenuOption.ConfigurationMenu:                        
                        ConfigSubMenu();
                        break;

                    case MainMenuOption.ResetDatabase:                       
                        resetData("All");
                        break;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }

            } while (choice != 0); 




            /// <summary>
            /// Submenu for handling volunteer actions.
            /// </summary>
            static void volunteerMenu()
            {
                bool exitSubMenu = false;


                do
                {
                    Console.WriteLine("Submenu Options:");
                    Console.WriteLine("0 - Exit Submenu");
                    Console.WriteLine($"1 - Create a new volunteer");
                    Console.WriteLine($"2 - Display an volunteer by ID");
                    Console.WriteLine($"3 - Display all volunteers");
                    Console.WriteLine($"4 - Update an existing volunteer");
                    Console.WriteLine($"5 - Delete an existing volunteer");
                    Console.WriteLine($"6 - Delete all volunteers");
                    Console.Write("Please enter your choice: \n");
                    int choice = int.Parse(Console.ReadLine() ?? "0");

                    SubMenuOption selectedOption = (SubMenuOption)choice;// Convert the choice to enum


                    try
                    {
                        switch (selectedOption)
                        {
                            case SubMenuOption.Exit:
                                Console.WriteLine("Exiting Submenu...");
                                exitSubMenu = true;
                                break;

                            case SubMenuOption.Create:
                                Console.WriteLine($"Creating a new volunteer...");
                                createEntity("Volunteer");
                                break;

                            case SubMenuOption.Read:
                                Console.WriteLine($"Enter volunteer ID to display:");
                                int id = int.Parse(Console.ReadLine() ?? "0");
                                Console.WriteLine(s_dal.Volunteer.Read(id));
                                break;

                            case SubMenuOption.ReadAll:                                
                                showData("Volunteer");
                                break;


                            case SubMenuOption.Update:
                                updateEntity("Volunteer");
                                break;

                            case SubMenuOption.Delete:
                                Console.WriteLine($"Enter volunteer ID to delete:");
                                id = int.Parse(Console.ReadLine() ?? "0");
                                s_dal.Volunteer.Delete(id);
                                break;

                            case SubMenuOption.DeleteAll:                             
                                resetData("Volunteer");
                                break;

                            default:
                                Console.WriteLine("Invalid Option");
                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                } while (!exitSubMenu);
            }



            /// <summary>
            /// Submenu for handling call actions.
            /// </summary>
            static void callMenu()
            {
                bool exitSubMenu = false;

                do
                {
                    Console.WriteLine("Submenu Options:");
                    Console.WriteLine("0 - Exit Submenu");
                    Console.WriteLine($"1 - Create a new call");
                    Console.WriteLine($"2 - Display an call by ID");
                    Console.WriteLine($"3 - Display all calls");
                    Console.WriteLine($"4 - Update an existing call");
                    Console.WriteLine($"5 - Delete an existing call");
                    Console.WriteLine($"6 - Delete all calls");
                    Console.Write("Please enter your choice: \n");
                    int choice = int.Parse(Console.ReadLine() ?? "0");

                    SubMenuOption selectedOption = (SubMenuOption)choice;// Convert the choice to enum


                    try
                    {
                        switch (selectedOption)
                        {
                            case SubMenuOption.Exit:
                                Console.WriteLine("Exiting Submenu...");
                                exitSubMenu = true;
                                break;

                            case SubMenuOption.Create:                             
                                createEntity("Call");
                                break;

                            case SubMenuOption.Read:
                                Console.WriteLine($"Enter call ID to display:");
                                int id = int.Parse(Console.ReadLine() ?? "0");
                                Console.WriteLine(s_dal.Call.Read(id));
                                break;

                            case SubMenuOption.ReadAll:                              
                                showData("Call");
                                break;

                            case SubMenuOption.Update:                                
                                updateEntity("Call");
                                break;

                            case SubMenuOption.Delete:
                                Console.WriteLine($"Enter call ID to delete:");
                                id = int.Parse(Console.ReadLine() ?? "0");
                                s_dal.Call.Delete(id);
                                break;

                            case SubMenuOption.DeleteAll:                              
                                resetData("Call");
                                break;

                            default:
                                Console.WriteLine("Invalid Option");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                } while (!exitSubMenu);
            }




            /// <summary>
            /// Submenu for handling Assignment actions.
            /// </summary>
            static void AssignmentMenu()
            {
                bool exitSubMenu = false;

                do
                {
                    Console.WriteLine("Submenu Options:");
                    Console.WriteLine("0 - Exit Submenu");
                    Console.WriteLine($"1 - Create a new assignment");
                    Console.WriteLine($"2 - Display an assignment by ID");
                    Console.WriteLine($"3 - Display all assignments");
                    Console.WriteLine($"4 - Update an existing assignment");
                    Console.WriteLine($"5 - Delete an existing assignment");
                    Console.WriteLine($"6 - Delete all assignments");
                    Console.Write("Please enter your choice: \n");
                    int choice = int.Parse(Console.ReadLine() ?? "0");

                    SubMenuOption selectedOption = (SubMenuOption)choice;// Convert the choice to enum


                    try
                    {
                        switch (selectedOption)
                        {
                            case SubMenuOption.Exit:
                                Console.WriteLine("Exiting Submenu...");
                                exitSubMenu = true;
                                break;

                            case SubMenuOption.Create:                             
                                createEntity("Assignment");
                                break;


                            case SubMenuOption.Read:
                                Console.WriteLine($"Enter assignment ID to display:");
                                int id = int.Parse(Console.ReadLine() ?? "0");
                                Console.WriteLine(s_dal.Assignment.Read(id));
                                break;

                            case SubMenuOption.ReadAll:                               
                                showData("Assignment");
                                break;

                            case SubMenuOption.Update:
                                updateEntity("Assignment");
                                break;

                            case SubMenuOption.Delete:
                                Console.WriteLine($"Enter assignment ID to delete:");
                                id = int.Parse(Console.ReadLine() ?? "0");
                                s_dal.Assignment.Delete(id);
                                break;
                                                                        
                            case SubMenuOption.DeleteAll:                             
                                resetData("Assignment");
                                break;

                            default:
                                Console.WriteLine("Invalid Option");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                } while (!exitSubMenu);
            }


            /// <summary>
            /// Displays the configuration submenu for managing system settings and configuration variables.
            /// Allows the user to perform various actions such as advancing the system clock, 
            /// displaying or setting configuration values, and resetting them.
            /// </summary>
            static void ConfigSubMenu()
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("Configuration Submenu:");
                    Console.WriteLine("0 - Exit Submenu");
                    Console.WriteLine("1 - Advance system clock by one minute");
                    Console.WriteLine("2 - Advance system clock by one hour");
                    Console.WriteLine("3 - Advance system clock by one day");
                    Console.WriteLine("4 - Advance system clock by one month");
                    Console.WriteLine("5 - Advance system clock by one year");
                    Console.WriteLine("6 - Display current system clock value");
                    Console.WriteLine("7 - Set a new value for a configuration variable");
                    Console.WriteLine("8 - Display current value of a configuration variable");
                    Console.WriteLine("9 - Reset values for all configuration variables");
                    Console.Write("Please enter your choice: \n");

                    int choice = int.Parse(Console.ReadLine() ?? "0");
                    ConfigMenu selectedOption = (ConfigMenu)choice;


                    switch (selectedOption)
                    {
                        case ConfigMenu.Exit:
                            exit = true;
                            break;

                        case ConfigMenu.AdvanceClockMinute:
                            s_dal.Config.Clock = s_dal.Config.Clock.AddMinutes(1);
                            Console.WriteLine("Advanced system clock by one minute.");
                            break;

                        case ConfigMenu.AdvanceClockHour:
                            s_dal.Config.Clock = s_dal.Config.Clock.AddHours(1);
                            Console.WriteLine("Advanced system clock by one hour.");
                            break;

                        case ConfigMenu.AdvanceClockDay:
                            s_dal.Config.Clock = s_dal.Config.Clock.AddDays(1);
                            Console.WriteLine("Advanced system clock by one day.");
                            break;
                        case ConfigMenu.AdvanceClockMonth:
                            s_dal.Config.Clock = s_dal.Config.Clock.AddMonths(1);
                            Console.WriteLine("Advanced system clock by one month.");
                            break;
                        case ConfigMenu.AdvanceClockYear:
                            s_dal.Config.Clock = s_dal.Config.Clock.AddYears(1);
                            Console.WriteLine("Advanced system clock by one year.");
                            break;

                        case ConfigMenu.DisplayCurrentClock:
                            Console.WriteLine($"Current system clock: {s_dal.Config.Clock}");
                            break;

                        case ConfigMenu.SetNewConfigValue:
                            SetNewConfig();
                            break;

                        case ConfigMenu.DisplayCurrentConfigValue:
                            ShowConfig();
                            break;

                        case ConfigMenu.ResetAllConfigValues:
                            resetData("config");
                            Console.WriteLine("All configuration values have been reset.");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }

                }
            }
        

            /// <summary>
            /// Displays data for a specific entity type: Volunteer, Call, Assignment, or all of them.
            /// </summary>
            /// <param name="data">The type of data to display ( "Volunteer", "Call", "Assignment", "All").</param>
            static void showData(string data)
            {
                bool flag1 = false, flag2 = false, flag3 = false;

                if (data == "Volunteer")
                    flag1 = true;
                if (data == "Call")
                    flag2 = true;
                if (data == "Assignment")
                    flag3 = true;
                if (data == "All")
                {
                    flag1 = true;
                    flag2 = true;
                    flag3 = true;
                }

                if (flag1 == true)
                {
                    Console.WriteLine("Volunteers:");
                    foreach (var item in s_dal.Volunteer.ReadAll())
                    {
                        Console.WriteLine(item + "\n");
                    }
                }

                if (flag2 == true)
                {
                    Console.WriteLine("Calls:");
                    foreach (var item in s_dal.Call.ReadAll())
                    {
                        Console.WriteLine(item + "\n");
                    }
                }
                if (flag3 == true)
                {
                    Console.WriteLine("Assignment:");
                    foreach (var item in s_dal.Assignment.ReadAll())
                    {
                        Console.WriteLine(item + "\n");
                    }
                }
            }


            /// <summary>
            /// Resets data for specific entities: Volunteer, Call, Assignment, config, or All.
            /// </summary>
            /// <param name="data">The type of data to reset (e.g., "Volunteer", "Call", "Assignment", "config", "All").</param>
            static void resetData(string data)
            {
                bool flag1 = false, flag2 = false, flag3 = false, flag4 = false;

                if (data == "Volunteer")
                    flag1 = true;
                if (data == "Call")
                    flag2 = true;
                if (data == "Assignment")
                    flag3 = true;
                if (data == "config")
                    flag4 = true;
                if (data == "All")
                {
                    flag1 = true;
                    flag2 = true;
                    flag3 = true;
                    flag4 = true;
                }

                if (flag1)
                    s_dal.Volunteer.DeleteAll();

                if (flag2)
                    s_dal.Call.DeleteAll();

                if (flag3)
                    s_dal.Assignment.DeleteAll();

                if (flag4)
                    s_dal.Config.Reset();
            }


            /// <summary>
            /// Sets a new value for a configuration variable (Clock, RiskRange).
            /// </summary>
            static void SetNewConfig()
            {
                Console.Write("Enter your choice \n1 - Clock \n2 - RiskRange \n");
                string? choice = Console.ReadLine();
                Console.Write("Enter the new value: ");
                string newValue = Console.ReadLine()!;

                if (choice == "1")
                {
                    DateTime newClockValue = DateTime.Parse(newValue);
                    s_dal.Config.Clock = newClockValue;
                    Console.WriteLine($"Clock set to '{newValue}'.");
                }
                else if (choice == "2")
                {
                    TimeSpan newRiskRangeValue = TimeSpan.Parse(newValue);
                    s_dal.Config.RiskRange = newRiskRangeValue;
                    Console.WriteLine($"RiskRange set to '{newValue}'.");
                }
                
            }


            /// <summary>
            /// Displays the current values of the configuration variables: Clock, RiskRange, AssignmentId, and CallId.
            /// </summary>
            static void ShowConfig()
            {

                Console.Write("configuration variabl to Display \n1 - clock \n2 - RiskRange \n3 - AssignmentId \n4 - CallId \n");
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine($"Clock : {s_dal.Config.Clock}");
                }
                else if (choice == "2")
                {
                    Console.WriteLine($"RiskRange : {s_dal.Config.RiskRange}");
                }
                else if (choice == "3")
                {
                    Console.WriteLine($"AssignmentId : {s_dal.Config.NextAssignmentId}");
                }
                else if (choice == "4")
                {
                    Console.WriteLine($"AssignmentId : {s_dal.Config.NextCallId}");
                }
            }


            /// <summary>
            /// update of an entity based on user input.
            /// </summary>
            /// <param name="entity">The type of entity to be processed ("Volunteer", "Call", "Assignment").</param>
            static void updateEntity(string entity)
            {
                if (entity == "Volunteer")
                {

                    Console.WriteLine($"Enter volunteer ID to update:");
                    int id = int.Parse(Console.ReadLine() ?? "0");
                    Volunteer? unUpdated = s_dal.Volunteer.Read(id);
                    if (unUpdated != null)
                    {
                        Console.WriteLine(unUpdated);
                    }
                    Console.WriteLine("Please enter the following details for update the Volunteer:");

                    Console.Write("Name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Phone Number: ");
                    string phoneNumber = Console.ReadLine() ?? "";

                    Console.Write("Email: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Password (optional): ");
                    string? password = Console.ReadLine();

                    Console.Write("Address (optional): ");
                    string? address = Console.ReadLine();

                    Console.Write("Latitude (optional): ");
                    double? latitude = double.TryParse(Console.ReadLine(), out var latValue) ? latValue : null;

                    Console.Write("Longitude (optional): ");
                    double? longitude = double.TryParse(Console.ReadLine(), out var lonValue) ? lonValue : null;

                    Console.Write("Maximum Distance (optional): ");
                    double? maximumDistance = double.TryParse(Console.ReadLine(), out var distValue) ? distValue : null;

                    Console.Write("Duty (Volunteer, manager): ");
                    job duty = Enum.TryParse(Console.ReadLine(), out job parsedDuty) ? parsedDuty : job.volunteer;

                    Console.Write("Is Active (true/false): ");
                    bool isActive = bool.TryParse(Console.ReadLine(), out var activeValue) && activeValue;

                    Console.Write("Distance Type (Air, Road): ");
                    distanceType distance = Enum.TryParse(Console.ReadLine(), out distanceType parsedDistance) ? parsedDistance : distanceType.Air;
                    
                    s_dal.Volunteer.Update(new Volunteer(id, name, phoneNumber, email, password, address, latitude, longitude, maximumDistance, duty, isActive, distance));


                }

                if (entity == "Assignment")
                {
                    Console.WriteLine($"Enter Assignment ID to update:");
                    int id = int.Parse(Console.ReadLine() ?? "0");
                    Assignment? unUpdated = s_dal.Assignment.Read(id);
                    if (unUpdated != null)
                    {
                        Console.WriteLine(unUpdated);

                    }
                    Console.WriteLine("Please enter the following details for the Assignment:");

                    Console.Write("Call ID: ");
                    int callId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Volunteer ID: ");
                    int volunteerId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Assignment Time (yyyy-mm-dd hh:mm:ss): ");
                    DateTime assignmentTime = DateTime.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Assignment Total Time (optional, yyyy-mm-dd hh:mm:ss): ");
                    DateTime? assignmentTotalTime = DateTime.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Call Status (HandledOnTime, SelfCancellation, ManagerCancellation, ExpiredCancellation): ");
                    CallStatus? type = Enum.TryParse(Console.ReadLine(), out CallStatus parsedType) ? parsedType : null;


                    s_dal.Assignment.Update(new(0, callId, volunteerId, assignmentTime, assignmentTotalTime, type));


                }

                if (entity == "Call")
                {
                    Console.WriteLine($"Enter call ID to update:");
                    int id = int.Parse(Console.ReadLine() ?? "0");
                    Call? unUpdated = s_dal.Call.Read(id);
                    if (unUpdated != null)
                    {
                        Console.WriteLine(unUpdated);
                    }
                    Console.WriteLine("Please enter the following details for the Call:");

                    Console.Write("Call Type: \n1 - ProvideTransportation \n2 - Emergency \n3 - ProvideTransportation \n4 - MedicalAssistance \n5 -  EmotionalSupport \n6 - HomeCare \n7 - HospitalVisit \n8 - DeliverMedicine \n9 - CleaningAssistance\n");
                    int.TryParse(Console.ReadLine(), out int typeIndex);
                    CallType type = Enum.IsDefined(typeof(CallType), typeIndex - 1) ? (CallType)(typeIndex - 1) : CallType.ProvideTransportation;

                    Console.Write("Call Address: ");
                    string callAddress = Console.ReadLine() ?? "";

                    Console.Write("Description (optional): ");
                    string? description = Console.ReadLine();

                    Console.Write("Latitude: ");
                    double latitude = double.TryParse(Console.ReadLine(), out var latValue) ? latValue : 0.0;

                    Console.Write("Longitude: ");
                    double longitude = double.TryParse(Console.ReadLine(), out var lonValue) ? lonValue : 0.0;

                    Console.Write("Call Time (yyyy-mm-dd hh:mm:ss): ");
                    DateTime callTime = DateTime.TryParse(Console.ReadLine(), out var parsedCallTime) ? parsedCallTime : DateTime.Now;

                    Console.Write("Total Time (optional, yyyy-mm-dd hh:mm:ss): ");
                    DateTime? totalTime = DateTime.TryParse(Console.ReadLine(), out var parsedTotalTime) ? parsedTotalTime : (DateTime?)null;

                    s_dal.Call.Update(new(0, type, callAddress, description, latitude, longitude, callTime, totalTime));

                }

            }

            /// <summary>
            /// creation of an entity based on user input.
            /// </summary>
            /// <param name="entity">The type of entity to be processed ("Volunteer", "Call", "Assignment").</param>
            static void createEntity(string entity)
            {
                if (entity == "Assignment")
                {


                    Console.WriteLine("Please enter the following details for the Assignment:");

                    Console.Write("Call ID: ");
                    int callId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Volunteer ID: ");
                    int volunteerId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Assignment Time (yyyy-mm-dd hh:mm:ss): ");
                    DateTime assignmentTime = DateTime.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Assignment Total Time (optional, yyyy-mm-dd hh:mm:ss): ");
                    DateTime? assignmentTotalTime = DateTime.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Call Status (HandledOnTime, SelfCancellation, ManagerCancellation, ExpiredCancellation): ");
                    CallStatus? type = Enum.TryParse(Console.ReadLine(), out CallStatus parsedType) ? parsedType : null;

                    s_dal.Assignment.Create(new(0, callId, volunteerId, assignmentTime, assignmentTotalTime, type));

                }

                if (entity == "Volunteer")
                {
                    Console.WriteLine("Please enter the following details for the Volunteer:");

                    Console.Write("ID: ");
                    int id = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Phone Number: ");
                    string phoneNumber = Console.ReadLine() ?? "";

                    Console.Write("Email: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Password (optional): ");
                    string? password = Console.ReadLine();

                    Console.Write("Address (optional): ");
                    string? address = Console.ReadLine();

                    Console.Write("Latitude (optional): ");
                    double? latitude = double.TryParse(Console.ReadLine(), out var latValue) ? latValue : null;

                    Console.Write("Longitude (optional): ");
                    double? longitude = double.TryParse(Console.ReadLine(), out var lonValue) ? lonValue : null;

                    Console.Write("Maximum Distance (optional): ");
                    double? maximumDistance = double.TryParse(Console.ReadLine(), out var distValue) ? distValue : null;

                    Console.Write("Duty (Volunteer, manager): ");
                    job duty = Enum.TryParse(Console.ReadLine(), out job parsedDuty) ? parsedDuty : job.volunteer;

                    Console.Write("Is Active (true/false): ");
                    bool isActive = bool.TryParse(Console.ReadLine(), out var activeValue) && activeValue;

                    Console.Write("Distance Type (Air, Road): ");
                    distanceType distance = Enum.TryParse(Console.ReadLine(), out distanceType parsedDistance) ? parsedDistance : distanceType.Air;

                    s_dal.Volunteer.Create(new Volunteer(id, name, phoneNumber, email, password, address, latitude, longitude, maximumDistance, duty, isActive, distance));

                }

                if (entity == "Call")
                {

                    Console.WriteLine("Please enter the following details for the Call:");

                    Console.Write("Call Type: \n1 - ProvideTransportation \n2 - Emergency \n3 - ProvideTransportation \n4 - MedicalAssistance \n5 -  EmotionalSupport \n6 - HomeCare \n7 - HospitalVisit \n8 - DeliverMedicine \n9 - CleaningAssistance\n");
                    int.TryParse(Console.ReadLine(), out int typeIndex);
                    CallType type = Enum.IsDefined(typeof(CallType), typeIndex - 1) ? (CallType)(typeIndex - 1) : CallType.ProvideTransportation;

                    Console.Write("Call Address: ");
                    string callAddress = Console.ReadLine() ?? "";

                    Console.Write("Description (optional): ");
                    string? description = Console.ReadLine();

                    Console.Write("Latitude: ");
                    double latitude = double.TryParse(Console.ReadLine(), out var latValue) ? latValue : 0.0;

                    Console.Write("Longitude: ");
                    double longitude = double.TryParse(Console.ReadLine(), out var lonValue) ? lonValue : 0.0;

                    Console.Write("Call Time (yyyy-mm-dd hh:mm:ss): ");
                    DateTime callTime = DateTime.TryParse(Console.ReadLine(), out var parsedCallTime) ? parsedCallTime : DateTime.Now;

                    Console.Write("Total Time (optional, yyyy-mm-dd hh:mm:ss): ");
                    DateTime? totalTime = DateTime.TryParse(Console.ReadLine(), out var parsedTotalTime) ? parsedTotalTime : (DateTime?)null;

                    s_dal.Call.Create(new(0, type, callAddress, description, latitude, longitude, callTime, totalTime));

                }

            }

        }
    }
}

















