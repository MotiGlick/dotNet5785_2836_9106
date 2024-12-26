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



        public IEnumerable<BO.VolunteerInList> VolunteersList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteersListProperty); }
            set { SetValue(VolunteersListProperty, value); }
        }

        public static readonly DependencyProperty VolunteersListProperty =
            DependencyProperty.Register("CourseList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerInListWindow), new PropertyMetadata(null));


        public BO.VolunteerInListField Field { get; set; } = BO.VolunteerInListField.None;

        private void IsActiv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            VolunteersList = (Field == BO.VolunteerInListField.None) ?
            s_bl?.Volunteer.ReadAll(null,null)! : s_bl?.Volunteer.ReadAll(null, BO.VolunteerInListField.None)!;

        }
    }






}
