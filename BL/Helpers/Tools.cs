

using System.Text.Json;
using System.Text.RegularExpressions;

namespace Helpers;

internal class Tools 
{

    public static bool AddressExists(string address)
    {
        double[]? coordinates = Tools.GetCoordinates(address);
        return coordinates != null;
    }



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
}

