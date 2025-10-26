using ContractMonthlyClaimSystemPart1.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ContractMonthlyClaimSystemPart1
{
    public partial class CoordinatorPage : Window
    {
        private readonly string fullName;
        private readonly string staffNumber;
        private ObservableCollection<Claim> PendingClaims = new ObservableCollection<Claim>();
        private Claim selectedClaim;

        public CoordinatorPage(string fullName, string staffNumber)
        {
            InitializeComponent();
            this.fullName = fullName;
            this.staffNumber = staffNumber;

           
            RefreshPendingClaims();
        }

        private void RefreshPendingClaims()
        {
            PendingClaims = new ObservableCollection<Claim>(
                ClaimStore.AllClaims.Where(c => c.Status == ClaimStatus.PendingVerification)
            );
            lstPending.ItemsSource = PendingClaims;
        }

        private void lstPending_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClaim = lstPending.SelectedItem as Claim;

            if (selectedClaim != null)
            {
                txtDetails.Text =
                    $"Lecturer: {selectedClaim.LecturerName} ({selectedClaim.StaffNumber})\n" +
                    $"Hours: {selectedClaim.Hours}\nRate: {selectedClaim.HourlyRate:C}\nAmount: {selectedClaim.Amount:C}\n" +
                    $"Submitted: {selectedClaim.SubmittedAt:f}\nNotes: {selectedClaim.Notes}";

                txtFileName.Text = string.IsNullOrEmpty(selectedClaim.UploadedFilePath)
                    ? "No File Uploaded"
                    : Path.GetFileName(selectedClaim.UploadedFilePath);

                btnViewDoc.Visibility = string.IsNullOrEmpty(selectedClaim.UploadedFilePath)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                txtDetails.Text = "Select a claim to view details";
                txtFileName.Text = "";
                btnViewDoc.Visibility = Visibility.Collapsed;
            }
        }

        private void ViewDocument_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClaim == null || string.IsNullOrEmpty(selectedClaim.UploadedFilePath))
            {
                MessageBox.Show("No document uploaded for this claim.", "View Document",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (File.Exists(selectedClaim.UploadedFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = selectedClaim.UploadedFilePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("The uploaded file could not be found on the system.",
                        "File Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open the file.\n\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClaim != null)
            {
                selectedClaim.Status = ClaimStatus.Verified;
                selectedClaim.VerifiedBy = fullName;

                
                if (!ClaimStore.AllClaims.Contains(selectedClaim))
                    ClaimStore.AllClaims.Add(selectedClaim);

                PendingClaims.Remove(selectedClaim);
                MessageBox.Show("Claim verified and forwarded to Manager.",
                    "Verified", MessageBoxButton.OK, MessageBoxImage.Information);

                selectedClaim = null;
                txtDetails.Text = "Select a claim to view details";
                txtFileName.Text = "";
                btnViewDoc.Visibility = Visibility.Collapsed;

                RefreshPendingClaims();
            }
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClaim != null)
            {
                if (MessageBox.Show("Are you sure you want to reject this claim?",
                    "Confirm Rejection", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    selectedClaim.Status = ClaimStatus.Rejected;

                    if (!ClaimStore.AllClaims.Contains(selectedClaim))
                        ClaimStore.AllClaims.Add(selectedClaim);

                    PendingClaims.Remove(selectedClaim);
                    MessageBox.Show("Claim rejected.", "Rejected",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    selectedClaim = null;
                    txtDetails.Text = "Select a claim to view details";
                    txtFileName.Text = "";
                    btnViewDoc.Visibility = Visibility.Collapsed;

                    RefreshPendingClaims();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearch.Text.ToLower();
            lstPending.ItemsSource = string.IsNullOrWhiteSpace(filter)
                ? PendingClaims
                : new ObservableCollection<Claim>(
                    PendingClaims.Where(c =>
                        c.LecturerName.ToLower().Contains(filter) ||
                        c.StaffNumber.ToLower().Contains(filter))
                  );
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            RefreshPendingClaims();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
