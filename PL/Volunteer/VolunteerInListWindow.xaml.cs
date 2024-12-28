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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public VolunteerInListWindow()
        {
            InitializeComponent();
        }

        public BO.VolunteerInList? SelectedVolunteer { get; set; }


        public IEnumerable<BO.VolunteerInList> VolunteersList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteersListProperty); }
            set { SetValue(VolunteersListProperty, value); }
        }

        public static readonly DependencyProperty VolunteersListProperty =
            DependencyProperty.Register("VolunteersList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerInListWindow), new PropertyMetadata(null));


        public BO.CallType typeOfCall { get; set; } = BO.CallType.No_Filter;

        private void TypeOfCall_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryVolunteerList();

        }



        private void queryVolunteerList()
        => VolunteersList = (typeOfCall == BO.CallType.No_Filter) ?
        s_bl?.Volunteer.ReadAll(null,null,null)! : s_bl?.Volunteer.ReadAll(null, null, typeOfCall)!;

        private void VolunteerListObserver()
            => queryVolunteerList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Volunteer.AddObserver(VolunteerListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Volunteer.RemoveObserver(VolunteerListObserver);

        private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedVolunteer != null)
                new VolunteerWindow(SelectedVolunteer.Id).Show();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedVolunteer != null)
                new VolunteerWindow().Show();
        }

       

        private void btnDeleteVolunteer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is BO.Volunteer volunteer)
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
