using PL.Call;
using PL.Volunteer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        int UserId { get; set; }
        public MainWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }
        // Observer for clock updates
        private void ClockObserver()
        {
            CurrentTime = s_bl.Admin.GetClock();
        }

        // Observer for configuration updates
        private void ConfigObserver()
        {
            MaxYearRange = s_bl.Admin.GetMaxRange();
        }

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));

        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);
        }

        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);
        }

        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Day);
        }

        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);
        }

        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Year);
        }

        private void btnShowVolunteerList_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerInListWindow().Show();
        }

        public TimeSpan MaxYearRange
        {
            get { return (TimeSpan)GetValue(CurrentRiskRangeProperty); }
            set { SetValue(CurrentRiskRangeProperty, value); }
        }
        public static readonly DependencyProperty CurrentRiskRangeProperty =
        DependencyProperty.Register("CurrentRiskRange",typeof(TimeSpan), typeof(MainWindow));

        private void btnUpdateRiskRange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.SetMaxRange(MaxYearRange);
                MessageBox.Show("Risk Range updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during updating Risk Range: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            MaxYearRange = s_bl.Admin.GetMaxRange();
            s_bl.Admin.AddClockObserver(ClockObserver);
            s_bl.Admin.AddConfigObserver(ConfigObserver);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // Remove clock observer
            s_bl.Admin.RemoveClockObserver(ClockObserver);

            // Remove configuration observer
            s_bl.Admin.RemoveConfigObserver(ConfigObserver);
        }

        private void btnInitializeDatabase_Click(object sender, RoutedEventArgs e)
        {
            // הודעה למשתמש
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to initialize the database?",
                "Confirm Initialization",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // שינוי האייקון של העכבר לשעון חול
                    Mouse.OverrideCursor = Cursors.Wait;

                    // סגירת כל החלונות פרט לחלון הראשי
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != this)
                        {
                            window.Close();
                        }
                    }

                    // קריאה למתודת האתחול
                    s_bl.Admin.InitializeDB();

                    MessageBox.Show("Database initialized successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during initialization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // החזרת האייקון של העכבר למצב רגיל
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void btnResetDatabase_Click(object sender, RoutedEventArgs e)
        {
            // הודעה למשתמש
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to reset the database?",
                "Confirm Reset",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // שינוי האייקון של העכבר לשעון חול
                    Mouse.OverrideCursor = Cursors.Wait;

                    // סגירת כל החלונות פרט לחלון הראשי
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != this)
                        {
                            window.Close();
                        }
                    }

                    // קריאה למתודת האיפוס
                    s_bl.Admin.ResetDB();

                    MessageBox.Show("Database reset successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during reset: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // החזרת האייקון של העכבר למצב רגיל
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void btnShowCallList_Click(object sender, RoutedEventArgs e)
        {
            new CallInListWindow(UserId).Show();
        }
    }
}
