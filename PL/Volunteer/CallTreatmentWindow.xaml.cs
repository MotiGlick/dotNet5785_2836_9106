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
    /// Interaction logic for CallTreatmentWindow.xaml
    /// </summary>
    public partial class CallTreatmentWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public CallTreatmentWindow(int id)
        {
            this.UserId = id;
            InitializeComponent();
        }
        int UserId { get; set; }

        public IEnumerable<BO.OpenCallInList> OpenCallsList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallsListProperty); }
            set { SetValue(OpenCallsListProperty, value); }
        }

        public static readonly DependencyProperty OpenCallsListProperty =
            DependencyProperty.Register("OpenCallsList", typeof(IEnumerable<BO.OpenCallInList>), typeof(CallTreatmentWindow), new PropertyMetadata(null));


        public BO.OpenCallInList SelectedCall
        {
            get { return (BO.OpenCallInList)GetValue(SelectedCallProperty); }
            set { SetValue(SelectedCallProperty, value); }
        }

        public static readonly DependencyProperty SelectedCallProperty =
            DependencyProperty.Register("SelectedCall", typeof(BO.OpenCallInList), typeof(CallTreatmentWindow), new PropertyMetadata(null));

       

        //public BO.OpenCallInList SelectedCall { get; set; }

        public BO.CallType? typeOfCall { get; set; }= BO.CallType.No_Filter;

        private void BtCooseCall_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                s_bl.Call.ChoosingCallForHandling(UserId, SelectedCall.Id);
                MessageBox.Show("Call was chosen successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // Show error message if deletion fails
                MessageBox.Show($"Failed to choose call for handling : {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void queryOpenCallsList()
        => OpenCallsList = (typeOfCall == BO.CallType.No_Filter) ?
          s_bl?.Call.GetOpenCallsForVolunteer(UserId, null, null)! : s_bl?.Call.GetOpenCallsForVolunteer(UserId,  typeOfCall,null)!;


        /// <summary>
        /// Observes changes to the call list and updates the screen.
        /// </summary>
        private void CallListObserver()
            => queryOpenCallsList();

        //private void SelectedCallObserver()
        //{

        //    int id = SelectedCall.Id;
        //    SelectedCall = null;
        //    SelectedCall = s_bl.Call.GetCallDetails(id);

        //}

        /// <summary>
        /// Registers the observer for call list updates when the window loads.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
           =>s_bl.Call.AddObserver(CallListObserver);
          //s_bl.Call.AddObserver(SelectedCall.Id, SelectedCallObserver);

        


        /// <summary>
        /// Unregisters the observer for call list updates when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(CallListObserver);

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryOpenCallsList();
        }

     
    }
}
