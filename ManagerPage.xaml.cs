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
    public partial class ManagerPage : Window
    {
        private readonly string fullName;
        private readonly string staffNumber;
        private ObservableCollection<Claim> VerifiedClaims = new ObservableCollection<Claim>();
        private Claim selectedClaim;

        public ManagerPage(string fullName, string staffNumber)
        {
            InitializeComponent();
            this.fullName = fullName;
            this.staffNumber = staffNumber;

            RefreshVerifiedClaims();
        }

        private void RefreshVerifiedClaims()
        {
            VerifiedClaims = new ObservableCollection<Claim>(
                ClaimStore.AllClaims.Where(c => c.Status == ClaimStatus.Verified)
            );
            lstVerified.ItemsSource = VerifiedClaims;
        }

        private void lstVerified_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClaim = lstVerified.SelectedItem as Claim;

            if (selectedClaim != null)
            {
                txtDetails.Text =
                    $"Lecturer: {selectedClaim.LecturerName} ({selectedClaim.StaffNumber})\n" +
                    $"Hours: {selectedClaim.Hours}\nRate: {selectedClaim.HourlyRate:C}\nAmount: {selectedClaim.Amount:C}\n" +
                    $"Submitted: {selectedClaim.SubmittedAt:f}\nNotes: {selectedClaim.Notes}\n" +
                    $"Verified by: {selectedClaim.VerifiedBy ?? "—"}";

                txtFileName.Text = string.IsNullOrEmpty(selectedClaim.UploadedFilePath)
                    ? "No File Uploaded"
                    : Path.GetFileName(selectedClaim.UploadedFilePath);

                btnOpenDocument.Visibility = string.IsNullOrEmpty(selectedClaim.UploadedFilePath)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                txtDetails.Text = "Select a claim to view details";
                txtFileName.Text = "";
                btnOpenDocument.Visibility = Visibility.Collapsed;
            }
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClaim == null)
            {
                MessageBox.Show("Please select a claim first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedClaim.Status = ClaimStatus.Approved;
            selectedClaim.ApprovedBy = fullName;

            if (!ClaimStore.AllClaims.Contains(selectedClaim))
                ClaimStore.AllClaims.Add(selectedClaim);

            VerifiedClaims.Remove(selectedClaim);
            MessageBox.Show("Claim approved successfully.", "Approved", MessageBoxButton.OK, MessageBoxImage.Information);

            selectedClaim = null;
            txtDetails.Text = "Select a claim to view details";
            txtFileName.Text = "";
            btnOpenDocument.Visibility = Visibility.Collapsed;

            RefreshVerifiedClaims();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClaim == null)
            {
                MessageBox.Show("Please select a claim first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to reject this claim?", "Confirm Rejection",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                selectedClaim.Status = ClaimStatus.Rejected;

                if (!ClaimStore.AllClaims.Contains(selectedClaim))
                    ClaimStore.AllClaims.Add(selectedClaim);

                VerifiedClaims.Remove(selectedClaim);
                MessageBox.Show("Claim rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Information);

                selectedClaim = null;
                txtDetails.Text = "Select a claim to view details";
                txtFileName.Text = "";
                btnOpenDocument.Visibility = Visibility.Collapsed;

                RefreshVerifiedClaims();
            }
        }

        private void OpenDocument_Click(object sender, RoutedEventArgs e)
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearch.Text.ToLower();
            lstVerified.ItemsSource = string.IsNullOrWhiteSpace(filter)
                ? VerifiedClaims
                : new ObservableCollection<Claim>(
                    VerifiedClaims.Where(c =>
                        c.LecturerName.ToLower().Contains(filter) ||
                        c.StaffNumber.ToLower().Contains(filter))
                  );
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            RefreshVerifiedClaims();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
