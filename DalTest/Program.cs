using Dal;
using DalApi;
using DO;

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
                                else if(x == 3)
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
                                // קריאה לפונקציה להצגת האובייקט לפי מזהה (למשל: ReadObjectById())
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
                                // קריאה לפונקציה להצגת כל האובייקטים (למשל: ReadAllObjects())
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
                                // קריאה לפונקציה לעדכון האובייקט (למשל: UpdateObject())
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred while updating the object: " + ex.Message);
                            }
                            break;

                        case SubMenuOption.Delete:
                            try
                            {
                                Console.WriteLine("Enter object ID to delete:");
                                // קריאה לפונקציה למחיקת האובייקט לפי מזהה (למשל: DeleteObjectById())
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
                                // קריאה לפונקציה למחיקת כל האובייקטים (למשל: DeleteAllObjects())
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
            }


            static void CreateAssignmentFromInput()
            {

            }

            static void CreateVolunteerFromInput()
            {
                Console.WriteLine("Please enter the following details for the Volunteer:");

                Console.Write("ID: ");
                int id = int.Parse(Console.ReadLine());

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

                Console.Write("Duty (Volunteer, Coordinator, etc.): ");
                job duty = Enum.TryParse(Console.ReadLine(), out job parsedDuty) ? parsedDuty : job.volunteer;

                Console.Write("Is Active (true/false): ");
                bool isActive = bool.TryParse(Console.ReadLine(), out var activeValue) && activeValue;

                Console.Write("Distance Type (Air, Road): ");
                distanceType distance = Enum.TryParse(Console.ReadLine(), out distanceType parsedDistance) ? parsedDistance : distanceType.Air;
                s_dalVolunteer.Create(new Volunteer(id, name, phoneNumber, email, password, address, latitude, longitude, maximumDistance, duty, isActive, distance));


            }
        }
    }
}



























