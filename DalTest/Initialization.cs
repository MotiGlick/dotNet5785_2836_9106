namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;

/// <summary>
/// A static class responsible for initializing data in the system, including volunteers and a manager.
/// </summary>
public static class Initialization
{
    // Private static fields for different data access layers
    private static IVolunteer? s_dalVolunteer; // Data access layer for Volunteer (stage 1)
    private static ICall? s_dalCall; // Data access layer for Call (stage 1)
    private static IAssignment? s_dalAssignment; // Data access layer for Assignment (stage 1)
    private static IConfig? s_dalConfig; // Data access layer for general configuration (stage 1)

    // Random object for generating random values
    private static readonly Random s_rand = new();

    /// <summary>
    /// Method to create a list of volunteers with predefined details, including name, phone, email, address, and location.
    /// </summary>
    private static void creatVolunteer()
    {
        // Predefined volunteer data
        string[] VolunteerNames = {  "Noam Ben-David", "Lior Shmuel", "Neta Golan", "Aviv Mizrahi", "Yael Rubin",
           "Omer Sasson", "Tal Peretz", "Yonatan Elbaz", "Roni Malka", "Hodaya Avrahami",
            "Idan Azulai", "Maor Levi", "Shaked Kaplan", "Bar Nir", "Erez Goldstein" };

        string[] VolunteerPhones = {"0547864028", "0523345567", "0506789123", "0532234455", "0589475621",
            "0541234567", "0529876543", "0501122334", "0537894561", "0581237890",
            "0545566778", "0526677889", "0503344556", "0539988776", "0584455667"};

        string[] VolunteerEmails =  { "noam_benDavid@gmail.com", "lior_shmuel@gmail.com", "neta_golan@gmail.com", "aviv_mizrahi@gmail.com", "yael_rubin@gmail.com",
                "omer_sasson@gmail.com", "tal_peretz@gmail.com", "yonatan_elbaz@gmail.com", "roni_malka@gmail.com", "hodaya_avrahami@gmail.com",
                 "idan_azulai@gmail.com", "maor_levi@gmail.com", "shaked_kaplan@gmail.com", "bar_nir@gmail.com", "erez_goldstein@gmail.com" };

        string[] VolunteerAddresses ={ "10 King David St, Jerusalem", "15 Jaffa Rd, Jerusalem", "8 Agron St, Jerusalem", "3 Shlomzion HaMalka St, Jerusalem",
              "20 Yitzhak Kariv St, Jerusalem", "5 Emek Refaim St, Jerusalem", "2 HaPalmach St, Jerusalem", "12 Ben Yehuda St, Jerusalem",
              "7 Hillel St, Jerusalem", "18 Keren HaYesod St, Jerusalem", "25 HaNevi'im St, Jerusalem", "4 Rachel Imenu St, Jerusalem",
               "30 Hebron Rd, Jerusalem", "14 Haneviim St, Jerusalem", "6 Gaza St, Jerusalem"};

        string[] VolunteerPasswords ={ "83749205", "39482715", "52987412", "74859302", "10293847",
              "37482910", "95847120", "28374619", "49582037", "17649582",
              "82937465", "38475620", "62039487", "57102839", "91028475"};

        double[] VolunteerLatitudes ={ 31.7767, 31.7834, 31.7795, 31.7784, 31.7740,
                    31.7648, 31.7612, 31.7801, 31.7806, 31.7715,
                   31.7851, 31.7632, 31.7570, 31.7837, 31.7718};

        double[] VolunteerLongitudes ={ 35.2345, 35.2138, 35.2290, 35.2111, 35.2095,
               35.2211, 35.2256, 35.2119, 35.2213, 35.2280,
               35.2242, 35.2323, 35.2407, 35.2134, 35.2279};

        // Loop to create each volunteer and add them to the system
        for (int i = 0; i < VolunteerNames.Length; i++)
        {
            int id; // Unique identifier for the volunteer

            // Distance in meters from a central point
            double distanec;

            // Generate a unique volunteer ID
            do
                id = s_rand.Next(20000000, 40000000);
            while (s_dalVolunteer!.Read(id) != null);

            // Randomly generate the volunteer's distance
            distanec = (double)s_rand.Next(0, 100000);

            // Create and add the volunteer to the data access layer
            s_dalVolunteer!.Create(new(id, VolunteerNames[i], VolunteerPhones[i], VolunteerEmails[i],
                VolunteerPasswords[i], VolunteerAddresses[i], VolunteerLatitudes[i], VolunteerLongitudes[i],
                distanec, job.volunteer, true, distanceType.Air));
        }

        {
            // Create a manager with unique ID and specific details
            int id;
            do
                id = s_rand.Next(20000000, 40000000);
            while (s_dalVolunteer!.Read(id) != null);

            // Add the manager to the data access layer
            s_dalVolunteer!.Create(new(id, "Moti glick", "0527540619", "moti_glick@gmail.com",
                    "00324366", " 21 HaVaad HaLeumi St, Jerusalem", 31.7853, 35.2137, 100, job.manager, true, distanceType.Air));
        }
    }
    /// <summary>
    /// Method to create assignments, associating volunteers with calls.
    /// </summary>
    private static void creatAssignment()
    {
        // Retrieve lists of all calls and volunteers
        List<Call> CList = s_dalCall.ReadAll();
        List<Volunteer> VList = s_dalVolunteer.ReadAll();

        // Loop for creating assignments for volunteers who handled many calls
        for (int i = 0; i < 20; i++)
        {
            int randNum = s_rand.Next(0, 5); // Random index to select a volunteer from the first 5 volunteers

            // Get start and end times for the call
            DateTime CallStartTime = CList[i].CallTime;
            DateTime? CallEndTime = CList[i].TotalTime;

            // Generate random assignment start and end times between call start and end
            DateTime AssignmentTime = GetRandomDate(CallStartTime, CallEndTime);
            DateTime AssignmentTotalTime = GetRandomDate(AssignmentTime, CallEndTime);

            // Create an assignment and add it to the data access layer
            s_dalAssignment!.Create(new(0, CList[i].Id, VList[randNum].Id, AssignmentTime, AssignmentTotalTime, (CallStatus)randNum));
        }

        // Loop for creating assignments for volunteers who handled fewer calls
        for (int i = 20; i < 45; i++)
        {
            int randNum = s_rand.Next(6, 13); // Random index to select a volunteer from a different range (6 to 13)

            // Get start and end times for the call
            DateTime CallStartTime = CList[i].CallTime;
            DateTime? CallEndTime = CList[i].TotalTime;

            // Generate random assignment start and end times between call start and end
            DateTime AssignmentTime = GetRandomDate(CallStartTime, CallEndTime);
            DateTime AssignmentTotalTime = GetRandomDate(AssignmentTime, CallEndTime);

            // Create an assignment and add it to the data access layer
            s_dalAssignment!.Create(new(0, CList[i].Id, VList[randNum].Id, AssignmentTime, AssignmentTotalTime, (CallStatus)randNum));
        }
    }

    /// <summary>
    /// Generates a random date between a specified start and end date.
    /// </summary>
    /// <param name="start">The start date of the range.</param>
    /// <param name="end">The end date of the range (nullable).</param>
    /// <returns>A random DateTime between start and end, or start if end is null.</returns>
    static DateTime GetRandomDate(DateTime start, DateTime? end)
    {
        // If end is null, return start
        if (!end.HasValue)
        {
            return start;
        }

        // Calculate the timespan between the start and end dates
        TimeSpan timeSpan = end.Value - start;

        // Create a new random object
        Random random = new Random();

        // Generate a random number of days within the timespan
        int randomDays = random.Next(0, (int)timeSpan.TotalDays);

        // Add the random days to the start date to get the random date
        return start.AddDays(randomDays);
    }

    /// <summary>
    /// Creates call records with random addresses, coordinates, and descriptions.
    /// </summary>
    private static void creatCall()
    {
        // Define arrays for call addresses, latitudes, longitudes, and descriptions.
        string[] CallAddresses = new string[]
        {
        "23 Jaffa Street, Jerusalem", "15 King George Street, Jerusalem", "37 HaNeviim Street, Jerusalem", "78 Hebron Road, Jerusalem", "42 Herzl Boulevard, Jerusalem",
        "40 Agripas Street, Jerusalem", "29 Gaza Street, Jerusalem", "9 Emek Refaim Street, Jerusalem", "18 Keren Hayesod Street, Jerusalem", "10 Shlomzion Hamalka Street, Jerusalem",
        "5 David HaMelech Street, Jerusalem", "33 Ben Yehuda Street, Jerusalem", "25 HaPalmach Street, Jerusalem", "12 Derech Beit Lechem, Jerusalem", "50 Sderot Ben Gurion, Jerusalem",
        "22 Azza Street, Jerusalem", "16 Rachel Imeinu Street, Jerusalem", "7 Derech HaAtsmaut, Jerusalem", "60 Yitzhak Rabin Boulevard, Jerusalem", "3 HaTzanchanim Street, Jerusalem",
        "45 Mount Scopus, Jerusalem", "8 Shmuel HaNavi Street, Jerusalem", "11 Shivtei Israel Street, Jerusalem", "28 Rehov Hillel, Jerusalem", "4 Ben Maimon Street, Jerusalem",
        "17 Ussishkin Street, Jerusalem", "2 HaPisga Street, Jerusalem", "32 Beit HaKerem Street, Jerusalem", "6 Sderot Eshkol, Jerusalem", "14 Jabotinsky Street, Jerusalem",
        "19 HaAri Street, Jerusalem", "36 Betzalel Street, Jerusalem", "24 Rehov Mevo HaGalil, Jerusalem", "9 Ma'alot Street, Jerusalem", "21 Sderot Shazar, Jerusalem",
        "13 Shai Agnon Street, Jerusalem", "27 Haganah Street, Jerusalem", "53 Hamelech Shlomo Street, Jerusalem", "39 Hanassi Street, Jerusalem", "18 Kiryat Moshe Street, Jerusalem",
        "41 Agron Street, Jerusalem", "31 Pierre Koenig Street, Jerusalem", "29 French Hill, Jerusalem", "6 Shlomo HaMelech Street, Jerusalem", "37 Ein Kerem Street, Jerusalem",
        "49 Herzog Street, Jerusalem", "20 Strauss Street, Jerusalem", "34 Lincoln Street, Jerusalem", "11 Haneviim Street, Jerusalem", "56 Malha Road, Jerusalem"
        };

        double[] Calllatitude = new double[]
        {
        31.7833, 31.7798, 31.7832, 31.7487, 31.7903, 31.7835, 31.7839, 31.7612, 31.7743, 31.7806,
        31.7783, 31.7834, 31.7647, 31.7641, 31.7785, 31.7765, 31.7566, 31.7717, 31.7782, 31.7821,
        31.7943, 31.7986, 31.7826, 31.7792, 31.7819, 31.7687, 31.7780, 31.7754, 31.7972, 31.7665,
        31.7789, 31.7798, 31.7693, 31.7700, 31.7726, 31.7856, 31.7714, 31.7737, 31.7822, 31.7881,
        31.7698, 31.7519, 31.7997, 31.7767, 31.7673, 31.7720, 31.7784, 31.7761, 31.7689, 31.7512
        };

        double[] Calllongitude = new double[]
        {
        35.2196, 35.2193, 35.2174, 35.2127, 35.2060, 35.2150, 35.2117, 35.2178, 35.2167, 35.2222,
        35.2248, 35.2201, 35.2156, 35.2234, 35.2104, 35.2215, 35.2103, 35.2238, 35.2046, 35.2149,
        35.2431, 35.2227, 35.2145, 35.2123, 35.2233, 35.2137, 35.2172, 35.2205, 35.2308, 35.2180,
        35.2178, 35.2225, 35.2098, 35.2089, 35.2147, 35.2165, 35.2148, 35.2192, 35.2103, 35.2156,
        35.2222, 35.2084, 35.2221, 35.2157, 35.2170, 35.2187, 35.2149, 35.2234, 35.2116, 35.2049
        };

        string[] callDescriptions =
        {
        "ProvideTransportation: Assistance with transportation for those who need it.",
        "MedicalAssistance: Help with medical needs or emergencies.",
        "EmotionalSupport: Providing emotional support and companionship.",
        "HomeCare: Assistance with tasks around the home for daily living.",
        "HospitalVisit: Visiting individuals in the hospital for support or check-in.",
        "DeliverMedicine: Delivery of prescribed medications to individuals.",
        "CleaningAssistance: Help with cleaning tasks in the home."
    };

        // Set the start and end dates for the call range.
        DateTime start = s_dalConfig.Clock.AddDays(-14);
        int range = (s_dalConfig.Clock - start).Days;

        DateTime end = s_dalConfig.Clock.AddDays(+14);
        int limit = (end - s_dalConfig.Clock).Days;

        // Set the expiration date
        DateTime expired = s_dalConfig.Clock.AddDays(0);
        int expiredLimit = -5;

        // Loop to create 45 active calls
        for (int i = 0; i < 45; i++)
        {
            int t = s_rand.Next(0, 6); // Random type of call

            // Create a call with random address, description, coordinates, and dates
            s_dalCall!.Create(new(0, (CallType)t, CallAddresses[i], callDescriptions[t], Calllatitude[i], Calllongitude[i], start.AddDays(s_rand.Next(range)), end.AddDays(s_rand.Next(limit))));
        }

        // Loop to create 5 expired calls
        for (int i = 45; i < 50; i++)
        {
            int t = s_rand.Next(0, 6); // Random type of call

            // Create an expired call with random address, description, coordinates, and dates
            s_dalCall!.Create(new(0, (CallType)t, CallAddresses[i], callDescriptions[t], Calllatitude[i], Calllongitude[i], start.AddDays(s_rand.Next(range)), s_dalConfig.Clock.AddDays(-1)));
        }
    }

    /// <summary>
    /// Initialization method to set up volunteers, calls, and assignments with configurations.
    /// </summary>
    /// <param name="dalVolunteer">Volunteer data access object</param>
    /// <param name="dalCall">Call data access object</param>
    /// <param name="dalAssignment">Assignment data access object</param>
    /// <param name="dalConfig">Configuration data access object</param>
    /// <exception cref="NullReferenceException">Thrown when any parameter is null</exception>
    public static void Do(IVolunteer? dalVolunteer, ICall? dalCall, IAssignment? dalAssignment, IConfig? dalConfig)
    {
        // Check parameters are not null and assign to internal fields
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object 'Student' cannot be null!");
        s_dalCall = dalCall ?? throw new NullReferenceException("DAL object 'Course' cannot be null!");
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object 'Link' cannot be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object 'Config' cannot be null!");

        // Reset configurations and delete all existing data
        Console.WriteLine("Reset Configuration values and List values...");
        s_dalConfig.Reset();
        s_dalAssignment.DeleteAll();
        s_dalVolunteer.DeleteAll();
        s_dalCall.DeleteAll();

        // Initialize the lists of volunteers, calls, and assignments
        Console.WriteLine("Initializing Students list...");
        creatVolunteer();

        Console.WriteLine("Initializing Courses list...");
        creatCall();

        Console.WriteLine("Initializing Links list...");
        creatAssignment();
    }

}




