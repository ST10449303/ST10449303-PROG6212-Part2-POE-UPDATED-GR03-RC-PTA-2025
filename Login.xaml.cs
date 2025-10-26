
using System.Windows;
using System.Windows.Controls;
using ContractMonthlyClaimSystemPart1.Models;
using System.Windows.Documents; 

namespace ContractMonthlyClaimSystemPart1
{
    public partial class LoginWindow : Window
    {
        public User CurrentUser { get; private set; }
        public string Role { get; private set; }

        
        public LoginWindow()
        {
            InitializeComponent();
        }

        
        public LoginWindow(string role) : this() 
        {
            foreach (ComboBoxItem item in cmbRole.Items)
            {
                if (item.Content.ToString() == role)
                {
                    cmbRole.SelectedItem = item;
                    break;
                }
            }
        }

       
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            
            ComboBoxItem selectedRole = cmbRole.SelectedItem as ComboBoxItem;
            if (selectedRole == null)
            {
                MessageBox.Show("Please select a role before signing in.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Role = selectedRole.Content.ToString();

           
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            CurrentUser = new User
            {
                FullName = $"{Role} User",
                StaffNumber = Role.Substring(0, 3).ToUpper() + "001",
                Email = email,
                Password = password,
                Role = Role
            };

            MessageBox.Show($"Welcome {CurrentUser.FullName}!", "Login Successful",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

       
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Redirecting to Sign Up form (prototype).",
                            "Sign Up", MessageBoxButton.OK, MessageBoxImage.Information);

            
        }
    }
}
