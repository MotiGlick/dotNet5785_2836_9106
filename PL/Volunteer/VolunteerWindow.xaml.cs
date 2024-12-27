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

namespace PL.Volunteer
{

    /// <summary>
    /// Interaction logic for VolunteerWindow.xaml
    /// </summary>
    public partial class VolunteerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        string ButtonText;
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
                        PhoneNumber = "s_bl.Admin.GetClock()",
                        Email = "",
                        Password = "",
                        Address = "",
                        Duty = 0,
                        IsActive = false,
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


            }
        }


        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentVolunteer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));


        public BO.job duty { get; set; } = BO.job.volunteer;

        public BO.DistanceType distanceType { get; set; } = BO.DistanceType.car;

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonText == "Add")
                {
                    s_bl.Volunteer.Create(CurrentVolunteer!);
                    MessageBox.Show("Volunteer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Volunteer.Update(CurrentVolunteer!, CurrentVolunteer!.Id);
                    MessageBox.Show("Volunteer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
