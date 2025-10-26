using System.Windows;
using ContractMonthlyClaimSystemPart1.Models;


namespace ContractMonthlyClaimSystemPart1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Lecturer_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginAndOpenPage("Lecturer");
        }

        private void Coordinator_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginAndOpenPage("Coordinator");
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginAndOpenPage("Manager");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowLoginAndOpenPage(string role)
        {
            var login = new LoginWindow(role);

            if (login.ShowDialog() == true)
            {
                var user = login.CurrentUser;
                Window page = null;

                switch (role)
                {
                    case "Lecturer":
                        page = new LecturerPage(user.FullName, user.StaffNumber);
                        break;
                    case "Coordinator":
                        page = new CoordinatorPage(user.FullName, user.StaffNumber);
                        break;
                    case "Manager":
                        page = new ManagerPage(user.FullName, user.StaffNumber);
                        break;
                }

                page?.Show();
                this.Close();
            }
        }
    }
}
