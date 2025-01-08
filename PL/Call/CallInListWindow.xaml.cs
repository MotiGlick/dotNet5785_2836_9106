using BO;
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
    /// Interaction logic for CallInListWindow.xaml
    /// </summary>
    public partial class CallInListWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public CallInListWindow()
        {
            InitializeComponent();
        }


        public BO.CallInList? SelectedCall { get; set; }



        public IEnumerable<BO.CallInList> CallsList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallsListProperty); }
            set { SetValue(CallsListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CallsList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallsListProperty =
            DependencyProperty.Register("CallsList", typeof(IEnumerable<BO.CallInList>), typeof(CallInListWindow), new PropertyMetadata(null));


        public BO.CallType callTypeFilter { get; set; } = BO.CallType.No_Filter;

        public BO.StatusOfCall statusFilter { get; set; } = BO.StatusOfCall.No_Filter;

        public BO.CallInListField orderCallsList { get; set; } = BO.CallInListField.Id;

        private void queryCallsListByType() => CallsList = (callTypeFilter == BO.CallType.No_Filter) ?
            s_bl?.Call.GetCallList(null, null, orderCallsList)! : s_bl?.Call.GetCallList(CallInListField.CallType, callTypeFilter, orderCallsList)!;

        private void queryCallsListByStatus() => CallsList = (statusFilter == BO.StatusOfCall.No_Filter) ?
            s_bl?.Call.GetCallList(null, null, orderCallsList)! : s_bl?.Call.GetCallList(CallInListField.status, statusFilter, orderCallsList)!;



        private void callTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            queryCallsListByType();
        }

        private void statusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            queryCallsListByStatus();
        }


        private void CallListObserver()
        {
            queryCallsListByType();
            queryCallsListByStatus();
        }
        //  => queryCallsList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Call.AddObserver(CallListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(CallListObserver);


        private void btnDeleteCall_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is BO.CallInList call)
            {
                // Display confirmation dialog
                var result = MessageBox.Show($"Are you sure you want to delete call with id={call.CallId}?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Attempt to delete the volunteer
                        s_bl.Call.DeleteCall(call.CallId);
                        MessageBox.Show("Call deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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


        private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (SelectedVolunteer != null)
            //    new VolunteerWindow(SelectedVolunteer.Id).Show();
        }
      

    }
}

