using Dal;
using DalApi;
using DO;
using System.Net;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {
        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation();

        public enum MainMenuOption
        {
            Exit,               // יציאה מהתפריט הראשי
            SubMenu1,           // הצגת תת-תפריט עבור ישות 1
            SubMenu2,           // הצגת תת-תפריט עבור ישות 2
            SubMenu3,           // הצגת תת-תפריט עבור ישות 3
            InitializeData,     // ביצוע אתחול של הנתונים
            ShowAllData,        // הצגת כל הנתונים
            ConfigurationMenu,  // הצגת תת-תפריט עבור ישות תצורה
            ResetDatabase       // איפוס בסיס נתונים ואיפוס נתוני התצורה
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


        static void Main(string[] args)
        {
            try
            {
                int choice;

                do
                {
                    Console.WriteLine("Main Menu:");
                    Console.WriteLine("0. Exit");
                    Console.WriteLine("1. Show Submenu for Entity 1");
                    Console.WriteLine("2. Show Submenu for Entity 2");
                    Console.WriteLine("3. Show Submenu for Entity 3");
                    Console.WriteLine("4. Initialize Data");
                    Console.WriteLine("5. Show All Data");
                    Console.WriteLine("6. Entity Configuration");
                    Console.WriteLine("7. Reset Database");

                    bool isValidChoice;
                    do
                    {
                        string? input = Console.ReadLine();
                        isValidChoice = int.TryParse(input, out choice);

                        if (!isValidChoice)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                    }
                    while (!isValidChoice);

                    // המרת הבחירה לערך ב-enum
                    MainMenuOption selectedOption = (MainMenuOption)choice;

                    // switch שמתאים לפעולה של כל אפשרות
                    switch (selectedOption)
                    {
                        case MainMenuOption.Exit:
                            Console.WriteLine("Exiting...");
                            break;

                        case MainMenuOption.SubMenu1:
                            Console.WriteLine("Showing Submenu for Entity 1...");
                            ShowSubMenu(1);
                            break;

                        case MainMenuOption.SubMenu2:
                            Console.WriteLine("Showing Submenu for Entity 2...");
                            ShowSubMenu(2);
                            break;

                        case MainMenuOption.SubMenu3:
                            Console.WriteLine("Showing Submenu for Entity 3...");
                            ShowSubMenu(3);
                            break;

                        case MainMenuOption.InitializeData:
                            Console.WriteLine("Initializing Data...");
                            break;

                        case MainMenuOption.ShowAllData:
                            Console.WriteLine("Showing All Data...");
                            break;

                        case MainMenuOption.ConfigurationMenu:
                            Console.WriteLine("Showing Entity Configuration...");
                            break;

                        case MainMenuOption.ResetDatabase:
                            Console.WriteLine("Resetting Database...");
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
                Console.WriteLine("An error occurred: " + ex.Message);
            }












            //תת תפריט עבור ישויות
            static void ShowSubMenu(int x)
            {
                bool exitSubMenu = false;

                do
                {
                    Console.WriteLine("Submenu Options:");
                    Console.WriteLine("0. Exit Submenu");
                    Console.WriteLine("1. Create a new object");
                    Console.WriteLine("2. Display an object by ID");
                    Console.WriteLine("3. Display all objects");
                    Console.WriteLine("4. Update an existing object");
                    Console.WriteLine("5. Delete an existing object");
                    Console.WriteLine("6. Delete all objects");

                    // קריאת בחירת המשתמש ולוודא שהיא תקינה
                    int choice;
                    bool isValidChoice;
                    do
                    {
                        Console.Write("Please enter your choice: ");
                        string? input = Console.ReadLine();
                        isValidChoice = int.TryParse(input, out choice);

                        if (!isValidChoice || choice < 0 || choice > 6)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number (0-6).");
                            isValidChoice = false;
                        }
                    } while (!isValidChoice);

                    // המרת הבחירה לערך ב-enum
                    SubMenuOption selectedOption = (SubMenuOption)choice;

                    // switch עם טיפול חריגות
                    switch (selectedOption)
                    {
                        case SubMenuOption.Exit:
                            Console.WriteLine("Exiting Submenu...");
                            exitSubMenu = true;
                            break;

                        case SubMenuOption.Create:
                            try
                            {
                                Console.WriteLine("Creating a new object...");
                                if (x == 1)
                                    CreateVolunteerFromInput();
                                else if (x == 2)
                                    CreateCallFromInput();
                                else if (x == 3)
                                    CreateAssignmentFromInput();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred while creating the object: " + ex.Message);
                            }
                            break;

                        case SubMenuOption.Read:
                            try
                            {
                                Console.WriteLine("Enter object ID to display:");
                                string id = Console.ReadLine();
                                if (x == 1)
                                    s_dalVolunteer.Read(id);
                                else if (x == 2)
                                    s_dalCall.Read(id);///////////////////
                                else if (x == 3)
                                    s_dalAssignment.Read(id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred while reading the object: " + ex.Message);
                            }
                            break;

                        case SubMenuOption.ReadAll:
                            try
                            {
                                Console.WriteLine("Displaying all objects...");
                                if (x == 1)
                                    s_dalVolunteer.ReadAll();
                                else if (x == 2)
                                    s_dalCall.ReadAll();
                                else if (x == 3)
                                    s_dalAssignment.ReadAll();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred while displaying all objects: " + ex.Message);
                            }
                            break;

                        case SubMenuOption.Update:
                            try
                            {
                                Console.WriteLine("Enter object ID to update:");
                                int id = ChackId();
                                if (x == 1)
                                    s_dalVolunteer.Update(id);
                                else if (x == 2)
                                    s_dalCall.Update(id);
                                else if (x == 3)
                                    s_dalAssignment.Update(id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred while updating the object: " + ex.Message);
                            }
                            break;

                        case SubMenuOption.Delete:
                            try
                            {

                                if (x == 1)
                                    s_dalVolunteer.Delete();
                                else if (x == 2)
                                    s_dalCall.Delete();
                                else if (x == 3)
                                    s_dalAssignment.Delete();
                            }
                            }
                            catch (Exception ex)
                            {
                        Console.WriteLine("An error occurred while deleting the object: " + ex.Message);
                    }
                    break;

                        case SubMenuOption.DeleteAll:
                        try
                        {
                            Console.WriteLine("Deleting all objects...");
                            if (x == 1)
                                s_dalVolunteer.DeleteAll();
                            else if (x == 2)
                                s_dalCall.DeleteAll();
                            else if (x == 3)
                                s_dalAssignment.DeleteAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while deleting all objects: " + ex.Message);
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                    }

                } while (!exitSubMenu);
            }





















            static void CreateCallFromInput()
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



        static void CreateVolunteerFromInput()
        {

            Console.WriteLine("Please enter the following details for the Volunteer:");
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



            Console.Write("Duty (Volunteer, Coordinator, etc.): ");
            job duty = Enum.TryParse(Console.ReadLine(), out job parsedDuty) ? parsedDuty : job.volunteer;

            Console.Write("Is Active (true/false): ");
            bool isActive = bool.TryParse(Console.ReadLine(), out var activeValue) && activeValue;
            s_dalCall.Create(new(0, type, callAddress, description, latitude, longitude, callTime, totalTime));

            Console.Write("Distance Type (Air, Road): ");
            distanceType distance = Enum.TryParse(Console.ReadLine(), out distanceType parsedDistance) ? parsedDistance : distanceType.Air;
            s_dalVolunteer.Create(new Volunteer(id, name, phoneNumber, email, password, address, latitude, longitude, maximumDistance, duty, isActive, distance));



        }


        static void CreateAssignmentFromInput()
        {

            Console.Write("Call ID: ");
            int callId = int.TryParse(Console.ReadLine(), out var parsedCallId) ? parsedCallId : 0;

            Console.Write("Volunteer ID: ");
            int volunteerId = int.TryParse(Console.ReadLine(), out var parsedVolunteerId) ? parsedVolunteerId : 0;

            Console.Write("Assignment Time (yyyy-mm-dd hh:mm:ss): ");
            DateTime assignmentTime = DateTime.TryParse(Console.ReadLine(), out var parsedAssignmentTime) ? parsedAssignmentTime : DateTime.Now;

            Console.Write("Assignment Total Time (optional, yyyy-mm-dd hh:mm:ss): ");
            DateTime? assignmentTotalTime = DateTime.TryParse(Console.ReadLine(), out var parsedTotalTime) ? parsedTotalTime : (DateTime?)null;

            Console.Write("Call Status (HandledOnTime, SelfCancellation, ManagerCancellation, ExpiredCancellation): ");
            CallStatus? type = Enum.TryParse(Console.ReadLine(), out CallStatus parsedType) ? parsedType : null;

            s_dalAssignment.Create(new(0, callId, volunteerId, assignmentTime, assignmentTotalTime, type));

        }


    }
}
//}














