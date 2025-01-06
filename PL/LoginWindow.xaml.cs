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

namespace PL
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {


        /// <summary>
        /// Static instance of the business logic interface.
        /// </summary>
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public LoginWindow()
        {
            InitializeComponent();
           
        }

        public BO.Volunteer CurrentVolunteer { get; set; } = new BO.Volunteer();

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                BO.job j= s_bl.Volunteer.EnterToTheSystem(null,CurrentVolunteer.Password,CurrentVolunteer.Id);
                if (j == BO.job.volunteer)
                {

                  
                }
                else if (j == BO.job.manager)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                   // this.Hide();


                }
            }
            catch (Exception ex)
            {
                // Show error message if fails
                MessageBox.Show($"Failed entering to the system: {ex.Message}","Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
            
           
        }

        
    }
}
