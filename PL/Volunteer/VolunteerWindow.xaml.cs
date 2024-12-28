using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace PL.Volunteer
{
    public partial class VolunteerWindow : Window
    {
        public string ButtonText { get; set; }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public VolunteerWindow(int id = 0)
        {
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();

            try
            {
                CurrentVolunteer = (id != 0) ? s_bl.Volunteer.Read(id)!
                    : new BO.Volunteer()
                    {
                        Id = 0,
                        Name = "",
                        PhoneNumber = "10 digits, starting with 0",
                        Email = "For example: user@gmail.com",
                        Password = "",
                        Address = "",
                        Duty = 0,
                        IsActive = true,
                        MaximumDistance = null,
                        Distance = 0,
                        HandeledCalls = 0,
                        CanceledCalls = 0,
                        ExpiredCalls = 0,
                        CallInProgress = null,
                    };

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));

        public BO.job duty { get; set; } = BO.job.volunteer;
        public BO.DistanceType distanceType { get; set; } = BO.DistanceType.car;



        private void VolunteerObserver()
        {
            int id = CurrentVolunteer!.Id;
            CurrentVolunteer = null;
            CurrentVolunteer = s_bl.Volunteer.Read(id);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(VolunteerObserver);


        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(VolunteerObserver);


        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (ButtonText == "Add")
                {
                    s_bl.Volunteer.Create(CurrentVolunteer!);
                    MessageBox.Show("Volunteer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    s_bl.Volunteer.Update(CurrentVolunteer!, CurrentVolunteer!.Id);
                    MessageBox.Show("Volunteer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    VolunteerObserver();
                    Mouse.OverrideCursor = null;
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
            //if (CurrentVolunteer!.Id != 0)
            //{
            //    s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, VolunteerObserver);
            //}
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
