using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using BO;

namespace PL.Volunteer
{
    // <summary>
    /// Interaction logic for VolunteerWindow
    /// </summary>
    public partial class VolunteerWindow : Window
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
                        MaximumDistance = 0,
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
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));

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

        /// <summary>
        /// Event handler for when the window is loaded.
        /// Adds an observer to the volunteer's updates.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentVolunteer!.Id != 0)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, VolunteerObserver);
            }
        }

        /// <summary>
        /// Event handler for when the window is closed.
        /// Removes the observer from the volunteer's updates.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
             
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer!.Id, VolunteerObserver);
            
        }


        /// <summary>
        /// Event handler for the Add/Update button click.
        /// Adds a new volunteer or updates the existing volunteer in the system.
        /// </summary>
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
                    this.Close();
                }
                else
                {
                    s_bl.Volunteer.Update(CurrentVolunteer!, CurrentVolunteer!.Id);
                    MessageBox.Show("Volunteer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //VolunteerObserver();
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

     

    }
}



//public class CallInProgress
//{

//    /// <summary>
//    /// Gets the Id of the assignment for the call.
//    /// </summary>
//    public int Id { get; init; }//AssignmentId

//    /// <summary>
//    /// Gets the Id of the call.
//    /// </summary>
//    public int CallId { get; init; }

//    /// <summary>
//    /// Gets the type of the call.
//    /// </summary>
//    public CallType CallType { get; init; }

//    /// <summary>
//    /// Gets the description of the call, if available.
//    /// </summary>
//    public string? Description { get; init; }

//    /// <summary>
//    /// Gets the address associated with the call.
//    /// </summary>
//    public string Address { get; init; }

//    /// <summary>
//    /// Gets the time when the call was opened.
//    /// </summary>
//    public DateTime OpeningTime { get; init; }

//    /// <summary>
//    /// Gets the maximum allowed time for handling the call, if applicable.
//    /// </summary>
//    public DateTime? MaxTime { get; init; }

//    /// <summary>
//    /// Gets the time when the handling of the call started.
//    /// </summary>
//    public DateTime HndalStartTime { get; init; }

//    /// <summary>
//    /// Gets the distance of the volunteer from the call's location.
//    /// </summary>
//    public double DistanceFromVolunteer { get; init; }

//    /// <summary>
//    /// Gets or sets the current status of the call.
//    /// </summary>
//    public StatusOfCall status { get; set; }

//    /// <summary>
//    /// Returns a string that represents the current object, including its properties.
//    /// </summary>
//    /// <returns>A string representation of the object.</returns>
//    public override string ToString() => this.ToStringProperty();

//}