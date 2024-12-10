

using System.Text.RegularExpressions;

namespace Helpers;

internal class Tools
{
    //public static string ToStringProperty<T>(this T t);
    public static bool Validate(BO.Volunteer volunteer)
    {

        // בדיקת ת.ז (האם מספרי)
        if (!int.TryParse(volunteer.Id.ToString(), out int id) || id <= 0) return false;

        // בדיקת שם מלא (אורך נדרש)
        if (string.IsNullOrWhiteSpace(volunteer.Name) || volunteer.Name.Length < 2 || volunteer.Name.Length > 50)
            return false;

        // בדיקת טלפון סלולרי (אורך ומבנה תקין)
        if (string.IsNullOrWhiteSpace(volunteer.PhoneNumber) ||
            !Regex.IsMatch(volunteer.PhoneNumber, @"^0\d{9}$"))
            return false;

        // בדיקת קו רוחב (אם קיים)
        if (volunteer.Latitude.HasValue && !double.TryParse(volunteer.Latitude.ToString(), out double latitude)) return false;


        // בדיקת קו אורך (אם קיים)
        if (volunteer.Longitude.HasValue && !double.TryParse(volunteer.Longitude.ToString(), out double longitude)) return false;


        // בדיקת מרחק מרבי (אם קיים)
        if (volunteer.MaximumDistance.HasValue && (!double.TryParse(volunteer.MaximumDistance.ToString(), out double maxDistance) || maxDistance <= 0)) return false;

        if (string.IsNullOrWhiteSpace(volunteer.Email))
            return false;

        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(volunteer.Email, emailPattern))
            return false;

        // בדיקת כתובת מלאה (אורך נדרש אם קיים)
        if (!string.IsNullOrWhiteSpace(volunteer.Address) &&
            (volunteer.Address.Length < 5 || volunteer.Address.Length > 100))
            return false;


        return true;
    }
}

