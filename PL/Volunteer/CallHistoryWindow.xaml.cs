using BO;
using PL.Call;
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
    /// Interaction logic for CallHistoryWindow.xaml
    /// </summary>
    public partial class CallHistoryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public CallHistoryWindow(int id)
        {
            this.UserId = id;
            InitializeComponent();
        }
        int UserId { get; set; }
        public IEnumerable<BO.ClosedCallInList> ClosedCallsInList   
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(CallsListProperty); }
            set { SetValue(CallsListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CallsList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallsListProperty =
            DependencyProperty.Register("CallsList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(CallHistoryWindow), new PropertyMetadata(null));

        public BO.CallType callTypeFilter { get; set; } = BO.CallType.No_Filter;
      


        public BO.ClosedCallInListField orderCallsList { get; set; }
     

        private void queryCallsList() => ClosedCallsInList = (callTypeFilter == BO.CallType.No_Filter) ?
           s_bl?.Call.GetClosedCallsByVolunteer(UserId, null, orderCallsList)! : s_bl?.Call.GetClosedCallsByVolunteer(UserId, callTypeFilter, orderCallsList)!;

        

        private void callTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            queryCallsList();
        }

        private void statusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            queryCallsList();
        }

        private void CallListObserver()
        {
            queryCallsList();
        }
        //  => queryCallsList();
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Call.AddObserver(CallListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(CallListObserver);

    }
}
