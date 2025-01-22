using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;

internal class ConvertUpdateToTrue : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string key = (string)value;
        if (key == "Update")
            return true;
        else
            return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}



public class TextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string key = (string)value;
        if (key == "Update")
        {
            return Brushes.Gray;
        }
        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class buttonvisibilityConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int callId = (int)value;
        if (s_bl.Call.CanBeDeleted(callId))
        {
            return Visibility.Visible;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}




public class BoolToVisibilityConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      
         //BO.CallInProgress currentCall = (BO.CallInProgress)value;
        if (value is BO.CallInProgress currentCall && currentCall != null)
        {
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}



public class BooleanToWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
       
        if (value is BO.CallInProgress call)
        {
            
            return call != null ? 980 : 470;
        }

       
        return 470; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertStatusOfCallToBoolForMaxTime : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.StatusOfCall CallStatus = (BO.StatusOfCall)value;
        if (CallStatus == BO.StatusOfCall.InProgressAtRisk
            || CallStatus == BO.StatusOfCall.InProgress
            || CallStatus == BO.StatusOfCall.Open
            || CallStatus == BO.StatusOfCall.OpenAtRisk)
           return true;

        return false;
        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


internal class ConvertStatusOfCallToBool : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.StatusOfCall CallStatus = (BO.StatusOfCall)value;
        if ( CallStatus == BO.StatusOfCall.Open || CallStatus == BO.StatusOfCall.OpenAtRisk)
            return true;

        return false;

    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


internal class ConvertCallStatusToColor : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.StatusOfCall CallStatus = (BO.StatusOfCall)value;
        if (CallStatus == BO.StatusOfCall.Open || CallStatus == BO.StatusOfCall.Closed
            || CallStatus==BO.StatusOfCall.Expired ||CallStatus==BO.StatusOfCall.InProgress )
            return Brushes.Black; ;

        return Brushes.OrangeRed; ;

    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class visibilityFotVolunteererConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int VolunteererId = (int)value;
        if (s_bl.Volunteer.CanDeleted(VolunteererId))
        {
            return Visibility.Visible;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class PasswordConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && value is string password)
        {
            return new string('*', password.Length); 
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }


}

public class IsChacedConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Call CalL = (BO.Call)value;
        
        if (CalL != null )
        {
            return true;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

public class ChusingCallEnableConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Volunteer volunteer = (BO.Volunteer)value;


        if (volunteer!=null&&volunteer.IsActive && (volunteer.CallInProgress== null))
        {
            return Visibility.Visible;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

