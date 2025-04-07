using Microsoft.AspNetCore.Mvc;
using SellingBook.Services.Storage;

namespace SellingBook.Areas.Admin.Controllers
{
    public class DriveController : Controller
    {
        private readonly GoogleDriveService _googleDriveService;
        public DriveController(GoogleDriveService googleDriveService)
        {
            _googleDriveService = googleDriveService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string folderId = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Invalid file type. Only JPEG, PNG, and GIF are allowed.");

            var tempPath = Path.Combine(Path.GetTempPath(), file.FileName);
            using var stream = new FileStream(tempPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var result = await _googleDriveService.UploadImageAndGetDirectUrl(tempPath, file.ContentType);

            System.IO.File.Delete(tempPath); // Clean up the temporary file
            return Json(new
            {
                DirectImageUrl = result
            });
        }
    }
}
