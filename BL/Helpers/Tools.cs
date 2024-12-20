

namespace Helpers;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System;

internal class Tools
{
    //public static string ToStringProperty<T>(this T t);




    public static double[]? GetCoordinates(string address)
    {
        string baseUrl = "https://us1.locationiq.com/v1/search.php";
        string apiKey = "YOUR_API_KEY";
        string requestUrl = $"{baseUrl}?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(requestUrl).Result;

            if (!response.IsSuccessStatusCode)
            {
                // אם יש שגיאה בתקשורת, אפשר לבחור לזרוק חריגה או להחזיר null
                return null;
            }

            string jsonResponse = response.Content.ReadAsStringAsync().Result;

            using (JsonDocument document = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = document.RootElement;

                if (root.GetArrayLength() == 0)
                {
                    // אין תוצאות עבור הכתובת
                    return null;
                }

                JsonElement firstResult = root[0];
                double latitude = double.Parse(firstResult.GetProperty("lat").GetString());
                double longitude = double.Parse(firstResult.GetProperty("lon").GetString());

                return new double[] { latitude, longitude };
            }
        }
    }


    public static bool AddressExists(string address)//#
    {
        double[]? coordinates = Tools.GetCoordinates(address);
        return coordinates != null;
    }

    public static double GetLatitude(string address)//#
    {
        double[]? coordinates = Tools.GetCoordinates(address);
        return coordinates![0];
    }

    public static double GetLongitude(string address)//#
    {
        double[]? coordinates = Tools.GetCoordinates(address);
        return coordinates![1];
    }


    public static double CalculateDistance(string address1, string address2)
    {
        // שליפת הקואורדינטות של שתי הכתובות
        double[] coords1 = GetCoordinates(address1);
        double[] coords2 = GetCoordinates(address2);

        // חישוב המרחק בעזרת נוסחת Haversine
        return CalculateHaversineDistance(coords1[0], coords1[1], coords2[0], coords2[1]);
    }

    private static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // רדיוס כדור הארץ בקילומטרים

        // המרת מעלות לרדיאנים
        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c; // המרחק בקילומטרים
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }

    //public static void InputCorectness(string address)
    //{
    //    if (string.IsNullOrWhiteSpace(address))
    //    {
    //        throw new ArgumentException("Address cannot be empty or null.");
    //    }
    //}







}
