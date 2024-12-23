
namespace Helpers;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System;
using System.Reflection;
using System.Collections;


/// <summary>
/// A utility class containing various helper methods for geographic calculations, address validation, 
/// and object property string representation.
/// </summary>
internal static class Tools
{

    /// <summary>
    /// Retrieves the geographic coordinates (latitude and longitude) of a given address using an external API.
    /// </summary>
    /// <param name="address">The address to retrieve coordinates for.</param>
    /// <returns>
    /// An array containing latitude and longitude as doubles, or null if the address cannot be resolved.
    /// </returns>



    public static double[]? GetCoordinates(string address)
    {
        string baseUrl = "https://nominatim.openstreetmap.org/search";
        string requestUrl = $"{baseUrl}?q={Uri.EscapeDataString(address)}&format=json&addressdetails=1";

        using (HttpClient client = new HttpClient())
        {
            // הוספת User-Agent לבקשה (נדרש על ידי OpenStreetMap)
            client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");

            HttpResponseMessage response = client.GetAsync(requestUrl).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                return null;
            }

            string jsonResponse = response.Content.ReadAsStringAsync().Result;

            using (JsonDocument document = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = document.RootElement;

                if (root.GetArrayLength() == 0)
                {
                    Console.WriteLine("No results found for the given address.");
                    return null;
                }

                JsonElement firstResult = root[0];
                double latitude = double.Parse(firstResult.GetProperty("lat").GetString());
                double longitude = double.Parse(firstResult.GetProperty("lon").GetString());

                return new double[] { latitude, longitude };
            }
        }
    }





/// <summary>
/// Checks whether a given address exists by attempting to retrieve its coordinates.
/// </summary>
/// <param name="address">The address to validate.</param>
/// <returns>True if the address exists; otherwise, false.</returns>
public static bool AddressExists(string address)//#
    {
        double[]? coordinates = GetCoordinates(address);
        return coordinates != null;
    }

    /// <summary>
    /// Retrieves the latitude of a given address.
    /// </summary>
    /// <param name="address">The address to retrieve latitude for.</param>
    /// <returns>The latitude as a double.</returns>
    /// <exception cref="Exception">Thrown if the address cannot be resolved.</exception>
    public static double GetLatitude(string address)//#
    {
        double[]? coordinates = GetCoordinates(address);
        return coordinates![0];
    }


    /// <summary>
    /// Retrieves the longitude of a given address.
    /// </summary>
    /// <param name="address">The address to retrieve longitude for.</param>
    /// <returns>The longitude as a double.</returns>
    /// <exception cref="Exception">Thrown if the address cannot be resolved.</exception>
    public static double GetLongitude(string address)//#
    {
        double[]? coordinates = GetCoordinates(address);
        return coordinates![1];
    }


    /// <summary>
    /// Calculates the distance in kilometers between two addresses using their geographic coordinates.
    /// </summary>
    /// <param name="doVolunteer">The first address.</param>
    /// <param name="doCall">The second address.</param>
    /// <returns>The distance in kilometers as a double.</returns>
    public static double CalculateDistance(DO.Volunteer? doVolunteer, DO.Call? doCall)
    {

        if (doVolunteer == null || doCall == null)
            return 0;
        return CalculateHaversineDistance(doVolunteer.Latitude, doVolunteer.Longitude, doCall.Latitude, doCall.Latitude);
    }


    /// <summary>
    /// Calculates the distance between two geographic coordinates using the Haversine formula.
    /// </summary>
    /// <param name="lat1">Latitude of the first point.</param>
    /// <param name="lon1">Longitude of the first point.</param>
    /// <param name="lat2">Latitude of the second point.</param>
    /// <param name="lon2">Longitude of the second point.</param>
    /// <returns>The distance in kilometers as a double.</returns>
    public static double CalculateHaversineDistance(double? lat1, double? lon1, double? lat2, double? lon2)
    {


        if (lat1 == null || lon1 == null || lat2 == null || lon2 == null)
            return 0;
        const double R = 6371; // Radius of the Earth in kilometers

        //  converting degrees to radians
        double dLat = DegreesToRadians((double)lat2 - (double)lat1);
        double dLon = DegreesToRadians((double)lon2 - (double)lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians((double)lat1)) * Math.Cos(DegreesToRadians((double)lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c; // distance in kilometers
    }

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <returns>The angle in radians.</returns>
    private static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }




    /// <summary>
    /// Extension method that converts the properties of an object to a formatted string.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="t">The object to convert.</param>
    /// <returns>A string containing the object's property names and values.</returns>
    public static string ToStringProperty<T>(this T t)//#
    {
        string str = "";
        foreach (PropertyInfo item in typeof(T).GetProperties())
        {
            var value = item.GetValue(t, null);
            str += item.Name + ": ";
            if (value is not string && value is IEnumerable)
            {
                str += "\n";
                foreach (var it in (IEnumerable<object>)value)
                {
                    str += it.ToString() + '\n';
                }
            }
            else
                str += value?.ToString() + '\n';
        }
        return str;
    }


}
