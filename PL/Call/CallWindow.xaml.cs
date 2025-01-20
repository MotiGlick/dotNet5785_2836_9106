using PL.Volunteer;
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

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window
    {

        /// <summary>
        /// Button text indicating whether the action is "Add" or "Update".
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Static reference to the business logic layer instance.
        /// </summary>
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public CallWindow(int Callid = 0)
        {
            ButtonText = Callid == 0 ? "Add" : "Update";
            InitializeComponent();
            try
            {
                CurrentCall = (Callid != 0) ? s_bl.Call.GetCallDetails(Callid)!
                    : new BO.Call()
                    {
                        Id = 0,
                        CallType = 0,
                        Description = "",
                        Address = "",
                        Latitude = 0,
                        Longitude = 0,
                        OpeningTime = s_bl.Admin.GetClock(),
                        MaxTime = null,
                        status = BO.StatusOfCall.Open,
                        CallAssignsList = null,
                    };

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


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
            DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));


        private void Callbserver()
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
           
                s_bl.Call.AddObserver(CurrentCall!.Id, Callbserver);
            
        }

        /// <summary>
        /// Event handler for when the window is closed.
        /// Removes the observer from the volunteer's updates.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {

            s_bl.Call.RemoveObserver(CurrentCall!.Id, Callbserver);

        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (chackFormet())
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    if (ButtonText == "Add")
                    {
                        s_bl.Call.AddCall(CurrentCall!);
                        MessageBox.Show("Call added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Mouse.OverrideCursor = null;
                        this.Close();
                    }
                    else
                    {
                        s_bl.Call.UpdateCallDetails(CurrentCall!);
                        MessageBox.Show("Call updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Mouse.OverrideCursor = null;
                        this.Close();
                    }
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

        private bool chackFormet()
        {
            if (CurrentCall!.Address == null || CurrentCall!.Address == "" || CurrentCall!.Address.Length < 5 || CurrentCall!.Address.Length > 100)
            {
                MessageBox.Show("Address not valid! \nThe address must be between 5 and 100 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

           
            if (CurrentCall!.Description != "" && double.TryParse(CurrentCall.Description, out double e))
            {
                MessageBox.Show("Description cannot be a number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
           

            return true;

        }


    }
}
