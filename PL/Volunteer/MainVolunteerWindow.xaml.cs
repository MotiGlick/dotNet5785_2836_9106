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
    /// Interaction logic for MainVolunteerWindow.xaml
    /// </summary>
    public partial class MainVolunteerWindow : Window
    {
        /// <summary>
        /// Button text indicating whether the action is "Add" or "Update".
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Static reference to the business logic layer instance.
        /// </summary>
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Initializes a new instance of the VolunteerWindow class.
        /// </summary>
        /// <param name="id">The ID of the volunteer to edit, or 0 to create a new volunteer.</param>
        public MainVolunteerWindow(int id = 0)
        {
            
            InitializeComponent();

            try
            {
                CurrentVolunteer = s_bl.Volunteer.Read(id);
                if(CurrentVolunteer.CallInProgress != null)
                    CurrentCall = s_bl.Call.GetCallDetails(CurrentVolunteer.CallInProgress.CallId);
                else { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(MainVolunteerWindow), new PropertyMetadata(null));



        
        /// <summary>
        /// The currently edited volunteer.
        /// </summary>
        public BO.Call? CurrentCall
        {
            get { return (BO.Call?)GetValue(CurrentCallProperty); }
            set { SetValue(CurrentCallProperty, value); }
        }


        /// <summary>
        /// Dependency property for the CurrentVolunteer object.
        /// </summary>
        public static readonly DependencyProperty CurrentCallProperty =
            DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(MainVolunteerWindow), new PropertyMetadata(null));


        /// <summary>
        /// Represents the volunteer's  duty.
        /// </summary>
        //public BO.job duty { get; set; } = BO.job.volunteer;

        ///// <summary>
        ///// Represents the distance type for the volunteer.
        ///// </summary>
        //public BO.DistanceType distanceType { get; set; } = BO.DistanceType.car;

        /// <summary>
        /// Updates the CurrentVolunteer by getting the data from the business logic layer.
        /// </summary>
        private void VolunteerObserver()
        {

            int id = CurrentVolunteer!.Id;
            CurrentVolunteer = null;
            CurrentVolunteer = s_bl.Volunteer.Read(id);

        }

        private void CallObserver()
        {

            int id = CurrentCall!.Id;
            CurrentCall = null;
            CurrentCall = s_bl.Call.GetCallDetails(id);

        }

        /// <summary>
        /// Event handler for when the window is loaded.
        /// Adds an observer to the volunteer's updates.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(CurrentVolunteer.CallInProgress != null)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, VolunteerObserver);
                s_bl.Call.AddObserver(CurrentCall!.Id, CallObserver);
            }
        }

        /// <summary>
        /// Event handler for when the window is closed.
        /// Removes the observer from the volunteer's updates.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {

            s_bl.Volunteer.RemoveObserver(CurrentVolunteer!.Id, VolunteerObserver);
            if (CurrentVolunteer.CallInProgress != null)
                s_bl.Call.RemoveObserver(CurrentCall!.Id, CallObserver);
        }


        /// <summary>
        /// Event handler for the Add/Update button click.
        /// Adds a new volunteer or updates the existing volunteer in the system.
        /// </summary>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
              
                
                    s_bl.Volunteer.Update(CurrentVolunteer!, CurrentVolunteer!.Id);
                    MessageBox.Show("Volunteer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Mouse.OverrideCursor = null;
                    this.Close();
                
            }
            catch (BO.BlDoesNotExistException ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (BO.BlNotValidException ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnEndCall_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                s_bl.Call.EndHandling(CurrentVolunteer.Id, CurrentVolunteer.CallInProgress.Id);
                MessageBox.Show("Call ended successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex){
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CallCancel_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                s_bl.Call.CancelHandling(CurrentVolunteer.Id, CurrentVolunteer.CallInProgress.Id);
                MessageBox.Show("Call cenceled successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnHistoryWindows_Click(object sender, MouseButtonEventArgs e)
        {
           //new  CallHistoryWindow(CurrentVolunteer.Id).Show();
        }

        private void BtnTreatmentWindow_Click(object sender, MouseButtonEventArgs e)
        {
            new CallTreatmentWindow().Show();
        }
    }
}
