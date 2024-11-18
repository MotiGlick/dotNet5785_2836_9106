

namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;


/// <summary>
/// 
/// </summary>
public static class Initialization
{
    private static IVolunteer? s_dalVolunteer; //stage 1
    private static ICall? s_dalCall; //stage 1
    private static IAssignment? s_dalAssignment; //stage 1
    private static IConfig? s_dalConfig; //stage 1

    private static readonly Random s_rand = new();

    private static void creatVolunteer()
    {

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

        for (int i = 0; i < VolunteerNames.Length; i++)
        {
            int id;

            //Distance in meters
            double distanec;

            do
                id = s_rand.Next(20000000, 40000000);
            while (s_dalVolunteer!.Read(id) != null);

            distanec = (double)s_rand.Next(0, 100000);

            s_dalVolunteer!.Create(new(id, VolunteerNames[i], VolunteerPhones[i], VolunteerEmails[i],
                VolunteerPasswords[i], VolunteerAddresses[i], VolunteerLatitudes[i], VolunteerLongitudes[i],
                distanec, job.volunteer, true, distanceType.Air));
        }

        {
            //creat manager
            int id;
            do
                id = s_rand.Next(20000000, 40000000);
            while (s_dalVolunteer!.Read(id) != null);
            s_dalVolunteer!.Create(new(id, "Moti glick", "0527540619", "moti_glick@gmail.com",
                    "00324366", " 21 HaVaad HaLeumi St, Jerusalem", 31.7853, 35.2137, 100, job.manager, true, distanceType.Air));
        }

    }

    private static void creatAssignment()
    {

        List<Call> CList = s_dalCall.ReadAll();
        List<Volunteer> VList = s_dalVolunteer.ReadAll();

        for (int i = 0; i < 20; i++)//Volunteers who handled a lot calls
        {
            int randNum = s_rand.Next(0, 5);

              DateTime CallStartTime = CList[i].CallTime;
              DateTime? CallEndTime = CList[i].TotalTime;
            DateTime AssignmentTime = GetRandomDate(CallStartTime, CallEndTime);
            DateTime AssignmentTotalTime = GetRandomDate(AssignmentTime, CallEndTime);

            s_dalAssignment!.Create(new(0, CList[i].Id, VList[randNum].Id, AssignmentTime, AssignmentTotalTime, (CallStatus)randNum));
        }


        for (int i = 20; i < 45; i++)//Volunteers who handled a few calls
        {
            int randNum = s_rand.Next(6, 13);


            DateTime CallStartTime = CList[i].CallTime;
            DateTime? CallEndTime = CList[i].TotalTime;
            DateTime AssignmentTime = GetRandomDate(CallStartTime, CallEndTime);
            DateTime AssignmentTotalTime = GetRandomDate(AssignmentTime, CallEndTime);


            s_dalAssignment!.Create(new(0, CList[i].Id, VList[randNum].Id, AssignmentTime, AssignmentTotalTime, (CallStatus)randNum));
        }

    }


    static DateTime GetRandomDate(DateTime start, DateTime? end)//function for random date ,we use Chat GPT
    {
        // אם end הוא null, מחזירים את start
        if (!end.HasValue)
        {
            return start;
        }

        // מחשבים את ההפרש בזמן בין שני התאריכים
        TimeSpan timeSpan = end.Value - start;

        // יוצרים אובייקט אקראי
        Random random = new Random();

        // מגרילים מספר אקראי בטווח משך הזמן
        int randomDays = random.Next(0, (int)timeSpan.TotalDays);

        // מוסיפים את הימים האקראיים לתאריך ההתחלה כדי לקבל את התאריך האקראי
        return start.AddDays(randomDays);
    }

    private static void creatCall()
    {
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

        //int day = s_rand.Next(1, 30);
        DateTime start = s_dalConfig.Clock.AddDays(-14);
        int range = (s_dalConfig.Clock - start).Days;

        DateTime end = s_dalConfig.Clock.AddDays(+14);
        int limit = (end - s_dalConfig.Clock).Days;

        DateTime expired = s_dalConfig.Clock.AddDays(0);
        int expiredLimit = -5;

        for (int i = 0; i < 45; i++)
        {
            int t = s_rand.Next(0, 6);

            s_dalCall!.Create(new(0, (CallType)t, CallAddresses[i], callDescriptions[t], Calllatitude[i], Calllongitude[i], start.AddDays(s_rand.Next(range)), end.AddDays(s_rand.Next(limit))));
        }
        for (int i = 45; i < 50; i++)
        {
            int t = s_rand.Next(0, 6);

            s_dalCall!.Create(new(0, (CallType)t, CallAddresses[i], callDescriptions[t], Calllatitude[i], Calllongitude[i], start.AddDays(s_rand.Next(range)), s_dalConfig.Clock.AddDays(-1)));
        }

    }



    public static void Do(IVolunteer? dalVolunteer, ICall? dalCall, IAssignment? dalAssignment, IConfig? dalConfig) //stage 1
    {
        // בדיקה שהפרמטרים אינם null והצבה למשתני הגישה
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object 'Student' cannot be null!");
        s_dalCall = dalCall ?? throw new NullReferenceException("DAL object 'Course' cannot be null!");
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object 'Link' cannot be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object 'Config' cannot be null!");


        Console.WriteLine("Reset Configuration values and List values...");
        s_dalConfig.Reset(); 
        s_dalAssignment.DeleteAll(); 
        s_dalVolunteer.DeleteAll();
        s_dalCall.DeleteAll();



        // אתחול רשימות
        Console.WriteLine("Initializing Students list...");
        creatVolunteer();

        Console.WriteLine("Initializing Courses list...");
        creatCall();

        Console.WriteLine("Initializing Links list...");
        creatAssignment();
    }



}




