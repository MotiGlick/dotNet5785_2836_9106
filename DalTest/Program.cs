using Dal;
using DalApi;
using DO;
using System.Net;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DalTest
{
    internal class Program
    {
        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation();
        private static ICall? s_dalCall = new CallImplementation();
        private static IAssignment? s_dalAssignment = new AssignmentImplementation();
        private static IConfig? s_dalConfig = new ConfigImplementation();

        public enum MainMenuOption
        {
            Exit,                // יציאה מהתפריט הראשי
            VolunteerMenu,       // הצגת תת-תפריט עבור מתנדב 
            CallMenu,            // הצגת תת-תפריט עבור קריאה 
            AssignmentMenu,      // הצגת תת-תפריט עבור משימה 
            InitializeData,      // ביצוע אתחול של הנתונים
            ShowAllData,         // הצגת כל הנתונים
            ConfigurationMenu,   // הצגת תת-תפריט עבור ישות תצורה
            ResetDatabase        // איפוס בסיס נתונים ואיפוס נתוני התצורה
        }
        public enum SubMenuOption
        {
            Exit,         // יציאה מתת-התפריט
            Create,       // הוספת אובייקט חדש מטיפוס הישות לרשימה
            Read,         // תצוגת אובייקט ע"פ מזהה
            ReadAll,      // תצוגת רשימת כל האובייקטים מטיפוס הישות
            Update,       // עדכון נתוני אובייקט קיים
            Delete,       // מחיקת אובייקט קיים מהרשימה
            DeleteAll     // מחיקת כל האובייקטים ברשימה
        }

        public enum ConfigMenu
        {
            Exit,
            AdvanceClockMinute,
            AdvanceClockHour,
            AdvanceClockDay,
            DisplayCurrentClock,
            SetNewConfigValue,
            DisplayCurrentConfigValue,
            ResetAllConfigValues
        }







        static void Main(string[] args)
        {
            try
            {
                int choice;

                do
                {
                    Console.WriteLine("Main Menu:");
                    Console.WriteLine("0. Exit");
                    Console.WriteLine("1. Show Submenu for  Volunteer");
                    Console.WriteLine("2. Show Submenu for Call");
                    Console.WriteLine("3. Show Submenu for Assignment");
                    Console.WriteLine("4. Initialize Data");
                    Console.WriteLine("5. Show All Data");
                    Console.WriteLine("6. Entity Configuration");
                    Console.WriteLine("7. Reset Database");


                    choice = int.Parse(Console.ReadLine() ?? "0");
                    MainMenuOption selectedOption = (MainMenuOption)choice; // Convert the choice to enum


                    switch (selectedOption)
                    {
                        case MainMenuOption.Exit:
                            Console.WriteLine("Exiting...");
                            break;

                        case MainMenuOption.VolunteerMenu:
                            Console.WriteLine("Showing Submenu for Volunteer...");
                            ShowSubMenu("Volunteer");
                            break;

                        case MainMenuOption.CallMenu:
                            Console.WriteLine("Showing Submenu for Call...");
                            ShowSubMenu("Call");
                            break;

                        case MainMenuOption.AssignmentMenu:
                            Console.WriteLine("Showing Submenu for  Assignment...");
                            ShowSubMenu("Assignment");
                            break;

                        case MainMenuOption.InitializeData:
                            Console.WriteLine("Initializing Data...");
                            Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                            break;

                        case MainMenuOption.ShowAllData:
                            Console.WriteLine("Showing All Data...");
                            showData("All");
                            break;

                        case MainMenuOption.ConfigurationMenu:
                            Console.WriteLine("Showing Submenu for  Configuration...");
                            DisplayConfigSubMenu();
                            break;

                        case MainMenuOption.ResetDatabase:
                            Console.WriteLine("Resetting Database...");
                            resetData("All");
                            break;

                        default:
                            Console.WriteLine("Invalid Option");
                            break;
                    }

                } while (choice != 0); // התנאי ליציאה מהלולאה

            }
            catch (Exception ex)
            {
                // טיפול בחריגה
                Console.WriteLine(ex.Message);
            }





            //תת תפריט עבור ישויות
            static void ShowSubMenu(string entity)
            {
                bool exitSubMenu = false;

                do
                {
                    Console.WriteLine("Submenu Options:");
                    Console.WriteLine("0. Exit Submenu");
                    Console.WriteLine($"1. Create a new {entity}");
                    Console.WriteLine($"2. Display an {entity} by ID");
                    Console.WriteLine($"3. Display all {entity}s");
                    Console.WriteLine($"4. Update an existing {entity}");
                    Console.WriteLine($"5. Delete an existing {entity}");
                    Console.WriteLine($"6. Delete all {entity}s");
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

                                Console.WriteLine($"Creating a new {entity}...");
                                CreatEntityFromInput(entity);

                                break;

                            case SubMenuOption.Read:

                                Console.WriteLine($"Enter {entity} ID to display:");
                                int id = int.Parse(Console.ReadLine() ?? "0");

                                if (entity == "Volunteer")
                                    Console.WriteLine(s_dalVolunteer.Read(id));
                                else if (entity == "Call")
                                    Console.WriteLine(s_dalCall.Read(id));
                                else if (entity == "Assignment")
                                    Console.WriteLine(s_dalAssignment.Read(id));
                                break;

                            case SubMenuOption.ReadAll:

                                Console.WriteLine($"Displaying all {entity}s...");

                                if (entity == "Volunteer")
                                    showData("Volunteer");

                                else if (entity == "Call")
                                    showData("Call");

                                else if (entity == "Assignment")
                                    showData("Assignment");
                                break;

                            case SubMenuOption.Update:
                                Console.WriteLine($"Enter {entity} ID to update:");
                                id = int.Parse(Console.ReadLine() ?? "0");
                                if (entity == "Volunteer")
                                {
                                    Volunteer? unUpdated = s_dalVolunteer.Read(id);
                                    s_dalVolunteer.Update(unUpdated);
                                }
                                else if (entity == "Call")
                                {
                                    Call? unUpdated = s_dalCall.Read(id);
                                    s_dalCall.Update(unUpdated);
                                }
                                else if (entity == "Assignment")
                                {
                                    Assignment unUpdated = s_dalAssignment.Read(id);
                                    s_dalAssignment.Update(unUpdated);
                                }
                                break;

                            case SubMenuOption.Delete:
                                Console.WriteLine($"Enter {entity} ID to delete:");
                                id = int.Parse(Console.ReadLine() ?? "0");
                                if (entity == "Volunteer")
                                    s_dalVolunteer.Delete(id);
                                else if (entity == "Call")
                                    s_dalCall.Delete(id);
                                else if (entity == "Assignment")
                                    s_dalAssignment.Delete(id);
                                break;

                            case SubMenuOption.DeleteAll:
                                Console.WriteLine("Deleting all objects...");
                                if (entity == "Volunteer")
                                    resetData("Volunteer");
                                else if (entity == "Call")
                                    resetData("Call");
                                else if (entity == "Assignment")
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






            static void DisplayConfigSubMenu()
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nConfiguration Submenu:");
                    Console.WriteLine("0 - Exit Submenu");
                    Console.WriteLine("1 - Advance system clock by one minute");
                    Console.WriteLine("2 - Advance system clock by one hour");
                    Console.WriteLine("3 - Advance system clock by one day");
                    Console.WriteLine("4 - Display current system clock value");
                    Console.WriteLine("5 - Set a new value for a configuration variable");
                    Console.WriteLine("6 - Display current value of a configuration variable");
                    Console.WriteLine("7 - Reset values for all configuration variables");
                    Console.Write("Please enter your choice: ");

                    int choice = int.Parse(Console.ReadLine() ?? "0");
                    ConfigMenu selectedOption = (ConfigMenu)choice;


                    switch (selectedOption)
                    {
                        case ConfigMenu.Exit:
                            exit = true;
                            break;

                        case ConfigMenu.AdvanceClockMinute:
                            s_dalConfig.Clock = s_dalConfig.Clock.AddMinutes(1);
                            Console.WriteLine("Advanced system clock by one minute.");
                            break;

                        case ConfigMenu.AdvanceClockHour:
                            s_dalConfig.Clock = s_dalConfig.Clock.AddHours(1);
                            Console.WriteLine("Advanced system clock by one hour.");
                            break;

                        case ConfigMenu.AdvanceClockDay:
                            s_dalConfig.Clock = s_dalConfig.Clock.AddDays(1);
                            Console.WriteLine("Advanced system clock by one day.");
                            break;

                        case ConfigMenu.DisplayCurrentClock:
                            Console.WriteLine($"Current system clock: {s_dalConfig.Clock}");
                            break;

                        case ConfigMenu.SetNewConfigValue:
                            //Console.Write("Enter the name of the configuration variable: ");
                            //string configVarName = Console.ReadLine();
                            //Console.Write("Enter the new value: ");
                            //string newValue = Console.ReadLine();
                            //s_dalConfig.Set(configVarName, newValue);
                            //Console.WriteLine($"Configuration variable '{configVarName}' set to '{newValue}'.");
                            break;

                        case ConfigMenu.DisplayCurrentConfigValue:
                            //Console.Write("Enter the name of the configuration variable to display: ");
                            //string varNameToDisplay = Console.ReadLine();
                            //Console.WriteLine($"Current value of '{varNameToDisplay}': {s_dalConfig.Get(varNameToDisplay)}");
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
                    foreach (var item in s_dalVolunteer.ReadAll())
                    {
                        Console.WriteLine(item + "\n");
                    }
                }

                if (flag2 == true)
                {
                    Console.WriteLine("Calls:");
                    foreach (var item in s_dalCall.ReadAll())
                    {
                        Console.WriteLine(item + "\n");
                    }
                }
                if (flag3 == true)
                {
                    Console.WriteLine("Assignment:");
                    foreach (var item in s_dalAssignment.ReadAll())
                    {
                        Console.WriteLine(item + "\n");
                    }
                }

            }


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
                    flag3 = true;
                if (data == "All")
                {
                    flag1 = true;
                    flag2 = true;
                    flag3 = true;
                    flag4 = true;
                }

                if (flag1)
                    s_dalVolunteer.DeleteAll();

                if (flag2)
                    s_dalCall.DeleteAll();

                if (flag3)
                    s_dalAssignment.DeleteAll();

                if (flag4)
                    s_dalConfig.Reset();

            }


            static void CreatEntityFromInput(string entity)
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

                    s_dalAssignment.Create(new(0, callId, volunteerId, assignmentTime, assignmentTotalTime, type));
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

                    s_dalVolunteer.Create(new Volunteer(id, name, phoneNumber, email, password, address, latitude, longitude, maximumDistance, duty, isActive, distance));


                }

                if (entity == "Call")
                {

                    Console.WriteLine("Please enter the following details for the Call:");

                    Console.Write("Call Type (ProvideTransportation, Emergency,ProvideTransportation, MedicalAssistance, EmotionalSupport, HomeCare, HospitalVisit, DeliverMedicine, CleaningAssistance): ");
                    CallType type = Enum.TryParse(Console.ReadLine(), out CallType parsedType) ? parsedType : CallType.ProvideTransportation;

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

                    s_dalCall.Create(new(0, type, callAddress, description, latitude, longitude, callTime, totalTime));


                }
            }





        }
    }
}















