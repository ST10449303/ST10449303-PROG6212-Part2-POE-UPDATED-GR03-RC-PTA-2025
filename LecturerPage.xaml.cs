using ContractMonthlyClaimSystemPart1.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ContractMonthlyClaimSystemPart1
{
    public partial class LecturerPage : Window
    {
        private readonly string lecturerName;
        private readonly string staffNumber;
        private string uploadedFilePath = string.Empty;
        private Claim selectedClaim = null;

        
        public ObservableCollection<Claim> MyClaims { get; } = new ObservableCollection<Claim>();

        
        public LecturerPage(string lecturerName, string staffNumber)
        {
            InitializeComponent();

            this.lecturerName = lecturerName ?? string.Empty;
            this.staffNumber = staffNumber ?? string.Empty;

            
            txtLecturerName.Text = this.lecturerName;
            txtStaffNumber.Text = this.staffNumber;

            
            lstMyClaims.ItemsSource = MyClaims;

            
            RefreshMyClaims();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(out decimal hours, out decimal rate)) return;

            if (selectedClaim != null)
            {
              
                selectedClaim.LecturerName = txtLecturerName.Text.Trim();
                selectedClaim.StaffNumber = txtStaffNumber.Text.Trim();
                selectedClaim.Hours = hours;
                selectedClaim.HourlyRate = rate;
                selectedClaim.Notes = txtNotes.Text?.Trim() ?? "";
                selectedClaim.UploadedFilePath = uploadedFilePath;
                selectedClaim.Status = ClaimStatus.PendingVerification;

                MessageBox.Show("Claim updated successfully.", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                
                var claim = new Claim
                {
                    LecturerName = txtLecturerName.Text.Trim(),
                    StaffNumber = txtStaffNumber.Text.Trim(),
                    Hours = hours,
                    HourlyRate = rate,
                    Notes = txtNotes.Text?.Trim() ?? "",
                    UploadedFilePath = uploadedFilePath,
                    SubmittedBy = txtLecturerName.Text.Trim(),
                    Status = ClaimStatus.PendingVerification,
                    SubmittedAt = DateTime.Now
                };

                
                ClaimStore.AllClaims.Add(claim);

                MessageBox.Show("Claim submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ClearForm();
            RefreshMyClaims();
        }

        private void SaveDraft_Click(object sender, RoutedEventArgs e)
        {
          
            if (selectedClaim != null)
            {
                selectedClaim.LecturerName = txtLecturerName.Text.Trim();
                selectedClaim.StaffNumber = txtStaffNumber.Text.Trim();
                selectedClaim.Hours = decimal.TryParse(txtHours.Text, out decimal h) ? h : 0;
                selectedClaim.HourlyRate = decimal.TryParse(txtRate.Text, out decimal r) ? r : 0;
                selectedClaim.Notes = txtNotes.Text?.Trim() ?? "";
                selectedClaim.UploadedFilePath = uploadedFilePath;
                
            }
            else
            {
                var draft = new Claim
                {
                    LecturerName = txtLecturerName.Text.Trim(),
                    StaffNumber = txtStaffNumber.Text.Trim(),
                    Hours = decimal.TryParse(txtHours.Text, out decimal h2) ? h2 : 0,
                    HourlyRate = decimal.TryParse(txtRate.Text, out decimal r2) ? r2 : 0,
                    Notes = txtNotes.Text?.Trim() ?? "",
                    UploadedFilePath = uploadedFilePath,
                    SubmittedBy = txtLecturerName.Text.Trim(),
                    Status = ClaimStatus.PendingVerification,
                    SubmittedAt = DateTime.Now
                };
                ClaimStore.AllClaims.Add(draft);
            }

            MessageBox.Show("Draft saved successfully.", "Draft Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearForm();
            RefreshMyClaims();
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Select supporting document",
                Multiselect = false
            };

            bool? res = dlg.ShowDialog();
            if (res == true)
            {
                try
                {
                    var fi = new FileInfo(dlg.FileName);
                    
                    if (fi.Length > 5 * 1024 * 1024)
                    {
                        MessageBox.Show("File too large. Max size is 5 MB.", "Upload error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var appFolder = AppDomain.CurrentDomain.BaseDirectory;
                    var uploadFolder = Path.Combine(appFolder, "UploadedFiles");
                    Directory.CreateDirectory(uploadFolder);

                    var dest = Path.Combine(uploadFolder, Guid.NewGuid().ToString() + "_" + Path.GetFileName(dlg.FileName));
                    File.Copy(dlg.FileName, dest, true);

                    uploadedFilePath = dest;
                    txtUploadedFile.Text = $"Uploaded: {Path.GetFileName(dest)}";
                    MessageBox.Show("File uploaded successfully.", "Upload", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Upload failed: {ex.Message}", "Upload error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void lstMyClaims_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClaim = lstMyClaims.SelectedItem as Claim;
            if (selectedClaim != null)
            {
                txtLecturerName.Text = selectedClaim.LecturerName;
                txtStaffNumber.Text = selectedClaim.StaffNumber;
                txtHours.Text = selectedClaim.Hours.ToString(CultureInfo.InvariantCulture);
                txtRate.Text = selectedClaim.HourlyRate.ToString(CultureInfo.InvariantCulture);
                txtNotes.Text = selectedClaim.Notes;
                uploadedFilePath = selectedClaim.UploadedFilePath ?? string.Empty;
                txtUploadedFile.Text = string.IsNullOrEmpty(uploadedFilePath) ? "" : $"Uploaded: {Path.GetFileName(uploadedFilePath)}";
            }
        }

        private bool ValidateForm(out decimal hours, out decimal rate)
        {
            hours = 0; rate = 0;

            if (string.IsNullOrWhiteSpace(txtLecturerName.Text) ||
                string.IsNullOrWhiteSpace(txtStaffNumber.Text) ||
                string.IsNullOrWhiteSpace(txtHours.Text) ||
                string.IsNullOrWhiteSpace(txtRate.Text))
            {
                MessageBox.Show("Please complete all required fields (Name, Staff #, Hours, Rate).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(txtHours.Text, out hours) || hours <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for hours.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(txtRate.Text, out rate) || rate < 0)
            {
                MessageBox.Show("Please enter a valid number for hourly rate.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            // Keep lecturer/staff fields editable per your request
            // txtLecturerName.Text = lecturerName;
            // txtStaffNumber.Text = staffNumber;

            txtHours.Clear();
            txtRate.Clear();
            txtNotes.Clear();
            uploadedFilePath = string.Empty;
            txtUploadedFile.Text = "";
            selectedClaim = null;
            lstMyClaims.SelectedItem = null;
        }

        private void RefreshMyClaims()
        {
            MyClaims.Clear();

           
            foreach (var c in ClaimStore.AllClaims.Where(x => x.StaffNumber == txtStaffNumber.Text.Trim()))
            {
                MyClaims.Add(c);
            }
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
