using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class GoogleDriveService
{
    private readonly IConfiguration _configuration;

    public GoogleDriveService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private DriveService GetDriveService()
    {
        // Define scopes matching the Python code
        string[] scopes = new[]
        {
            DriveService.Scope.DriveFile, // Access to files created by the app
            DriveService.Scope.Drive      // Full Drive access
        };

        // Load credentials from the service account key file
        var credential = GoogleCredential.FromFile(_configuration["HostDrive:ServiceAccountKeyPath"])
            .CreateScoped(scopes);

        // Initialize the Drive service
        var service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _configuration["HostDrive:ApplicationName"]
        });

        return service;
    }

    private async Task MakeFilePublic(DriveService service, string fileId)
    {
        try
        {
            var permission = new Permission
            {
                Type = "anyone", // Anyone can access
                Role = "reader"  // Read-only access
            };

            await service.Permissions.Create(permission, fileId).ExecuteAsync();
            Console.WriteLine($"File {fileId} is now public.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error making file public: {ex.Message}");
            throw; // Re-throw to handle upstream if needed
        }
    }

    public async Task<string> UploadImageToDrive(string filePath, string fileName, string mimeType = "image/jpeg")
    {
        try
        {
            // Get the Drive service
            var savePath = Path.GetFullPath(filePath);
            var service = GetDriveService();

            // Define file metadata
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                MimeType = mimeType
            };
            if (!string.IsNullOrEmpty(_configuration["HostDrive:FolderId"]))
            {
                fileMetadata.Parents = new List<string> { _configuration["HostDrive:FolderId"] };
            }

            // Upload the file
            string fileId;
            using (var stream = new FileStream(savePath, FileMode.Open))
            {
                var request = service.Files.Create(fileMetadata, stream, mimeType);
                request.Fields = "id"; // Only request the file ID
                await request.UploadAsync();

                var file = request.ResponseBody;
                if (file == null || string.IsNullOrEmpty(file.Id))
                {
                    throw new Exception("Upload response did not contain a file ID.");
                }
                fileId = file.Id;
            }
            Console.WriteLine($"File uploaded successfully. File ID: {fileId}");

            // Make the file public
            await MakeFilePublic(service, fileId);

            return fileId; // Return fileId as in Python code
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file to Google Drive: {ex.Message}");
            return null; // Return null on error, similar to Python
        }
    }

    // Helper method to generate the public URL (optional, as per Python logic)
    public string GetDirectImageLink(string fileId)
    {
        return $"https://drive.google.com/uc?id={fileId}";
    }
}