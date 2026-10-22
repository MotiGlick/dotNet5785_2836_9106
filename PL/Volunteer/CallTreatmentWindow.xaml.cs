using BO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;


namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for CallTreatmentWindow.xaml
    /// </summary>
    public partial class CallTreatmentWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public CallTreatmentWindow(int id)
        {
            this.UserId = id;
            CurrentVolunteer = s_bl.Volunteer.Read(id);
            InitializeComponent();
        }
        int UserId { get; set; }

        public IEnumerable<BO.OpenCallInList> OpenCallsList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallsListProperty); }
            set { SetValue(OpenCallsListProperty, value); }
        }

        public static readonly DependencyProperty OpenCallsListProperty =
            DependencyProperty.Register("OpenCallsList", typeof(IEnumerable<BO.OpenCallInList>), typeof(CallTreatmentWindow), new PropertyMetadata(null));


        public BO.OpenCallInList SelectedCall
        {
            get { return (BO.OpenCallInList)GetValue(SelectedCallProperty); }
            set { SetValue(SelectedCallProperty, value); }
        }

        public static readonly DependencyProperty SelectedCallProperty =
            DependencyProperty.Register("SelectedCall", typeof(BO.OpenCallInList), typeof(CallTreatmentWindow), new PropertyMetadata(null));



        //public BO.OpenCallInList SelectedCall { get; set; }

        public BO.CallType? typeOfCall { get; set; } = BO.CallType.No_Filter;

        private void BtCooseCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Call.ChoosingCallForHandling(UserId, SelectedCall.Id);
                MessageBox.Show("Call was chosen successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // Show error message if deletion fails
                MessageBox.Show($"Failed to choose call for handling : {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        /// <summary>
        /// The currently edited volunteer.
        /// </summary>
        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }


        /// <summary>
        /// Dependency property for the CurrentVolunteer object.
        /// </summary>
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(CallTreatmentWindow), new PropertyMetadata(null));



        private volatile DispatcherOperation? _firstObserverOperation = null; //stage 7
        private volatile DispatcherOperation? _secondeObserverOperation = null; //stage 7
        /// <summary>
        /// Updates the CurrentVolunteer by getting the data from the business logic layer.
        /// </summary>
        private void VolunteerObserver()
        {
            if (_firstObserverOperation is null || _firstObserverOperation.Status == DispatcherOperationStatus.Completed)
                _firstObserverOperation = Dispatcher.BeginInvoke(() =>
                {
                    int id = CurrentVolunteer!.Id;
                    CurrentVolunteer = null;
                    CurrentVolunteer = s_bl.Volunteer.Read(id);
                });

        }

        private void queryOpenCallsList()
        => OpenCallsList = (typeOfCall == BO.CallType.No_Filter) ?
          s_bl?.Call.GetOpenCallsForVolunteer(UserId, null, null)! : s_bl?.Call.GetOpenCallsForVolunteer(UserId, typeOfCall, null)!;




        /// <summary>
        /// Observes changes to the call list and updates the screen.
        /// </summary>
        private void CallListObserver()
        {
            if (_secondeObserverOperation is null || _secondeObserverOperation.Status == DispatcherOperationStatus.Completed)
                _secondeObserverOperation = Dispatcher.BeginInvoke(() => queryOpenCallsList());
        }



        /// <summary>
        /// Registers the observer for call list updates when the window loads.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            UpdateMap();
            s_bl.Call.AddObserver(CallListObserver);

        }



        /// <summary>
        /// Unregisters the observer for call list updates when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(CallListObserver);

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryOpenCallsList();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMap();
        }


        private async void UpdateMap()
        {
            string originAddress = s_bl.Volunteer.Read(UserId).Address;
            string destinationAddress = SelectedCall?.Address;

            if (!string.IsNullOrEmpty(originAddress) && !string.IsNullOrEmpty(destinationAddress))
            {
                string apiKey = "";
                string configPath = "api_key.txt";

                // 1. קריאת הקובץ אם הוא קיים
                if (System.IO.File.Exists(configPath))
                {
                    apiKey = System.IO.File.ReadAllText(configPath).Trim();
                }

                // 2. טיפול אלגנטי במקרה שאין מפתח או שהמפתח לא תקין (כמו "xscd")
                if (string.IsNullOrEmpty(apiKey) || !apiKey.StartsWith("AIza"))
                {
                    string noKeyHtml = @"
                <html>
                <body style='display:flex; justify-content:center; align-items:center; height:100vh; background-color:#f8d7da; color:#721c24; font-family:Arial;'>
                    <div style='text-align:center;'>
                        <h2>Google Maps Disabled</h2>
                        <p>API Key is missing or invalid.</p>
                        <p>Please add a valid Google Maps API Key to <b>api_key.txt</b>.</p>
                    </div>
                </body>
                </html>";

                    string errorFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "map_error.html");
                    System.IO.File.WriteAllText(errorFilePath, noKeyHtml);

                    if (MapBrowser != null)
                    {
                        await MapBrowser.EnsureCoreWebView2Async();
                        MapBrowser.Source = new Uri(errorFilePath);
                    }
                    return; 
                }

               
                string mapHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Custom Google Map</title>
    <script src='https://maps.googleapis.com/maps/api/js?key={apiKey}&libraries=places,directions'></script>
    <style>
        #map {{
            height: 100%;
            width: 100%;
        }}
        html, body {{
            height: 100%;
            margin: 0;
            padding: 0;
        }}
    </style>
</head>
<body>
    <div id='map'></div>
    <script>
        function initMap() {{
            const map = new google.maps.Map(document.getElementById('map'), {{
                zoom: 10,
                center: {{ lat: 31.7683, lng: 35.2137 }} // נקודת התחלה זמנית
            }});

            const geocoder = new google.maps.Geocoder();
            const directionsService = new google.maps.DirectionsService();
            const directionsRenderer = new google.maps.DirectionsRenderer({{
                suppressMarkers: true // מניעת סימונים כפולים
            }});
            directionsRenderer.setMap(map);

            // אייקונים מותאמים אישית
            const originIcon = 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png'; // כחול - מיקום נוכחי
            const destinationIcon = 'https://maps.google.com/mapfiles/ms/icons/red-dot.png'; // אדום - יעד

            geocoder.geocode({{ address: '{originAddress}' }}, function(results, status) {{
                if (status === 'OK') {{
                    const origin = results[0].geometry.location;
                    new google.maps.Marker({{
                        position: origin,
                        map: map,
                        title: 'Your Location',
                        icon: originIcon
                    }});

                    geocoder.geocode({{ address: '{destinationAddress}' }}, function(results, status) {{
                        if (status === 'OK') {{
                            const destination = results[0].geometry.location;
                            new google.maps.Marker({{
                                position: destination,
                                map: map,
                                title: 'Destination',
                                icon: destinationIcon
                            }});

                            // יצירת מסלול
                            directionsService.route({{
                                origin: origin,
                                destination: destination,
                                travelMode: google.maps.TravelMode.DRIVING
                            }}, function(response, status) {{
                                if (status === 'OK') {{
                                    directionsRenderer.setDirections(response);
                                }} else {{
                                    alert('Path not found: ' + status);
                                }}
                            }});
                        }} else {{
                            alert('Address not found: ' + status);
                        }}
                    }});
                }} else {{
                    alert('Address not found: ' + status);
                }}
            }});
        }}
        window.onload = initMap;
    </script>
</body>
</html>";

                // כתיבת HTML לקובץ זמני
                string filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "map.html");
                System.IO.File.WriteAllText(filePath, mapHtml);

                // טעינת ה-HTML ב-WebView2
                if (MapBrowser != null)
                {
                    await MapBrowser.EnsureCoreWebView2Async();
                    MapBrowser.Source = new Uri("about:blank"); // מנקה קודם
                    MapBrowser.Source = new Uri(filePath); // טוען מחדש
                }
            }
            else
            {
                if (MapBrowser != null)
                {
                    await MapBrowser.EnsureCoreWebView2Async();
                    MapBrowser.Source = new Uri("about:blank");
                }
            }
        }

    }



}

