using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;

namespace SellingBook.Services.Storage
{
    public class GoogleDriveService
    {
        private readonly IConfiguration _configuration;

        public GoogleDriveService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DriveService GetDriveService()
        {
            var credentials = GoogleCredential.FromFile(_configuration["HostDrive:ServiceAccountKeyPath"])
                .CreateScoped(DriveService.Scope.Drive);

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = _configuration["HostDrive:ApplicationName"],
            });
        }

        public async Task<(string fileId, string directPath)> UploadImageAndGetDirectUrl(string filePath, string mimeType)
        {
            var folderid = _configuration["HostDrive:FolderId"];
            var service = GetDriveService();

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(filePath),
                MimeType = mimeType,
                Parents = folderid != null ? new List<string> { folderid } : null // Set parent folder if provided
            };

            string fileId;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var request = service.Files.Create(fileMetadata, stream, mimeType);
                request.Fields = "id";
                await request.UploadAsync();
                var file = request.ResponseBody;
                fileId = file.Id;
            }

            if (string.IsNullOrEmpty(fileId))
            {
                throw new Exception("File to upload images to Google Drive.");
            }

            var permission = new Permission
            {
                Type = "anyone",
                Role = "reader"
            };

            await service.Permissions.Create(permission, fileId).ExecuteAsync();

            return (fileId, $"https://drive.google.com/uc?export=view&id={fileId}"); // Direct link to the image
        }

        public string GetDirectImageLink(string fileId)
        {
            return $"https://drive.google.com/uc?export=view&id={fileId}"; // Direct link to the image
        }

        public async Task<List<Google.Apis.Drive.v3.Data.File>> ListFiles(string folderId = null)
        {
            var service = GetDriveService();

            var request = service.Files.List();
            request.Q = folderId != null ? $"'{folderId}' in parents" : null; // Filter by folder if provided
            request.PageSize = 10;
            request.Fields = "files(id, name)";
            var result = await request.ExecuteAsync();

            return result.Files.ToList();
        }
    }
}
