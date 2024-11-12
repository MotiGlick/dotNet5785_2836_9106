

namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;

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
                id = s_rand.Next(20000000,40000000);
            while (s_dalVolunteer!.Read(id) != null);

            distanec = (double)s_rand.Next(0,100000);

            s_dalVolunteer!.Create(new(id, VolunteerNames[i], VolunteerPhones[i], VolunteerEmails[i], 
                VolunteerPasswords[i], VolunteerAddresses[i], VolunteerLatitudes[i], VolunteerLongitudes[i],
                distanec, job.volunteer,true ,distanceType.Air));
        }



{
            //creat manager
            int id;
        do
            id = s_rand.Next(20000000, 40000000);
        while (s_dalVolunteer!.Read(id) != null);
        s_dalVolunteer!.Create(new(id, "Moti glick", "0527540619", "moti_glick@gmail.com",
                "00324366"," 21 HaVaad HaLeumi St, Jerusalem", 31.7853, 35.2137, 100, job.manager, true, distanceType.Air));}

    }


}


