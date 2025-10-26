using System;
using System.Collections.Generic;
using ContractMonthlyClaimSystemPart1.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystemPart1.Tests.Utils
{
   
    public class FileUploadHelperTests : IDisposable
    {
        private readonly string tempFolder;

        public FileUploadHelperTests()
        {
            tempFolder = Path.Combine(Path.GetTempPath(), "claim_uploads_" + Guid.NewGuid());
            Directory.CreateDirectory(tempFolder);
        }

        public void Dispose()
        {
            Directory.Delete(tempFolder, true);
        }

        [Fact]
        public void ValidateUpload_ReturnsFalse_For_InvalidExtension()
        {
            var file = Path.Combine(tempFolder, "test.txt");
            File.WriteAllText(file, "Hello");

            var result = FileUploadHelper.ValidateUpload(file, out var error);

            Assert.False(result);
            Assert.Contains("Invalid file type", error);
        }
    }
}
