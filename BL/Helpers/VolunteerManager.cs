

using System.Text.RegularExpressions;

namespace Helpers;

internal class VolunteerManager
{
    //public static string ToStringProperty<T>(this T t);

    //public static string ToStringProperty<T>(this T t);
    /// <summary>
    /// A method that checks the correctness of all values ​​in terms of format
    /// </summary>
    /// <param name="volunteer"></param>
    /// <returns></returns>
    public static bool formatCheckToVolunteer(BO.Volunteer volunteer)
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
    /// <summary>
    /// A method that checks the logical correctness of all values.
    /// </summary>
    /// <param name="volunteer"></param>
    /// <returns></returns>
    public static bool logicCheckToVolunteer(BO.Volunteer volunteer)
    {
        if (!IsValidId(volunteer.Id))
        {
            return false; // תעודת זהות לא תקינה
        }
        if (!IsValidPhoneNumber(volunteer.PhoneNumber))
        {
            return false; // מספר טלפון לא תקין
        }
        if (Tools.AddressExists(volunteer.Address))
        {
            volunteer.Latitude = Tools.GetCoordinates(volunteer.Address)[0];
            volunteer.Longitude = Tools.GetCoordinates(volunteer.Address)[1];
        }
        else
            return false;

        return true;
    }
    /// <summary>
    /// A method that checks whether the ID card is valid
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static bool IsValidId(int id)
    {
        // הפיכת מספר תעודת הזהות למחרוזת כדי לטפל בספרות
        string idStr = id.ToString();

        // בדיקה אם האורך שגוי (למשל פחות מ-9 ספרות)
        if (idStr.Length != 9)
        {
            return false;
        }

        int sum = 0;

        for (int i = 0; i < idStr.Length; i++)
        {
            int num = int.Parse(idStr[i].ToString());

            if (i % 2 == 1) // מכפלה של הספרה במקום האי-זוגי
            {
                num *= 2;
                if (num > 9) num -= 9; // אם המספר גדול מ-9, מחסרים 9
            }

            sum += num;
        }

        return sum % 10 == 0; // בדיקת ספרת ביקורת
    }
    /// <summary>
    /// Phone number validation function
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // בדיקה אם המספר ריק, אם הוא לא באורך 10, או אם אינו מתחיל ב-0
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.StartsWith("0"))
        {
            return false;
        }

        // בדיקה שכל התווים הם ספרות
        if (!phoneNumber.All(char.IsDigit))
        {
            return false;
        }

        return true; // מספר הטלפון תקין
    }

}
