using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystemPart1.Core
{
    

    
    public static class FileUploadHelper
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".docx", ".xlsx" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        
        public static bool ValidateUpload(string filePath, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                errorMessage = "File path is empty.";
                return false;
            }

            if (!File.Exists(filePath))
            {
                errorMessage = "File does not exist.";
                return false;
            }

            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
            {
                errorMessage = "Invalid file type. Allowed types are: PDF, DOCX, XLSX.";
                return false;
            }

            var fi = new FileInfo(filePath);
            if (fi.Length > MaxFileSizeBytes)
            {
                errorMessage = "File too large. Maximum size is 5 MB.";
                return false;
            }

            return true;
        }
    }
}
