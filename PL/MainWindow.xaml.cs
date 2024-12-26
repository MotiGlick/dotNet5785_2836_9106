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
        public MainWindow()
        {
            InitializeComponent();
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

        private void btnShowList_Click(object sender, RoutedEventArgs e)
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
            s_bl.Admin.SetMaxRange(MaxYearRange);
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
    }
}
