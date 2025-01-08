using BO;
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
    /// Interaction logic for VolunteerInListWindow.xaml
    /// </summary>
    public partial class VolunteerInListWindow : Window
    {

        /// <summary>
        /// Static instance of the business logic interface.
        /// </summary>
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public VolunteerInListWindow()
        {
            InitializeComponent();

        }


        /// <summary>
        /// Gets or sets the currently selected volunteer in the list.
        /// </summary>
        public BO.VolunteerInList? SelectedVolunteer { get; set; }

        /// <summary>
        /// Dependency property for the list of volunteers.
        /// </summary>
        public IEnumerable<BO.VolunteerInList> VolunteersList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteersListProperty); }
            set { SetValue(VolunteersListProperty, value); }
        }

        public static readonly DependencyProperty VolunteersListProperty =
            DependencyProperty.Register("VolunteersList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerInListWindow), new PropertyMetadata(null));


        /// <summary>
        /// The type of call used to filter the volunteer list.
        /// </summary>
        public BO.CallType typeOfCall { get; set; } = BO.CallType.No_Filter;



        /// <summary> 
        /// Handles selection changes in the call type ComboBox.
        /// </summary>
        private void TypeOfCall_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryVolunteerList();

        }


        /// <summary>
        /// Queries the volunteer list based on the selected call type filter.
        /// </summary>
        private void queryVolunteerList()
        => VolunteersList = (typeOfCall == BO.CallType.No_Filter) ?
        s_bl?.Volunteer.ReadAll(null, null, null)! : s_bl?.Volunteer.ReadAll(null, null, typeOfCall)!;


        /// <summary>
        /// Observes changes to the volunteer list and updates the screen.
        /// </summary>
        private void VolunteerListObserver()
            => queryVolunteerList();


        /// <summary>
        /// Registers the observer for volunteer list updates when the window loads.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Volunteer.AddObserver(VolunteerListObserver);


        /// <summary>
        /// Unregisters the observer for volunteer list updates when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Volunteer.RemoveObserver(VolunteerListObserver);

        /// <summary>
        /// Opens a new VolunteerWindow when a user is double-clicked in the list.
        /// </summary>
        private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedVolunteer != null)
                new VolunteerWindow(SelectedVolunteer.Id).Show();
        }

        /// <summary>
        /// Opens a new VolunteerWindow to add a new volunteer.
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //  if (SelectedVolunteer != null)
            {
                new VolunteerWindow().Show();

            }
        }


        /// <summary>
        /// Handles the deletion of a volunteer when the delete button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event (delete button).</param>
        /// <param name="e">Event arguments.</param>
        private void btnDeleteVolunteer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is BO.VolunteerInList volunteer)
            {
                // Display confirmation dialog
                var result = MessageBox.Show($"Are you sure you want to delete {volunteer.Name}?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Attempt to delete the volunteer
                        s_bl.Volunteer.Delete(volunteer.Id);

                    }
                    catch (Exception ex)
                    {
                        // Show error message if deletion fails
                        MessageBox.Show($"Failed to delete volunteer: {ex.Message}",
                                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }







}
