using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace StudentLoginSystem.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string folderId = "1FpHhFY_6WwQ3p0KuWCgQ2jcr01NUjsv5";

        public GoogleDriveService()
        {
            var credential = GoogleCredential
                .FromFile(@"F:\MamaParsi\StudentLoginSystem\StudentLoginSystem\Files\studentlogin-465315-ff341c2155e6.json") // Path to your service account file
                .CreateScoped(DriveService.Scope.DriveReadonly);

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "StudentLoginSystem"
            });
        }

        public string GetVoucherUrlByRollNumber(string rollNumber)
        {
            var request = _driveService.Files.List();
            request.Q = $"name = '{rollNumber}.pdf' and '{folderId}' in parents";
            request.Fields = "files(id, name)";
            var result = request.Execute();

            var file = result.Files.FirstOrDefault();
            if (file != null)
            {
                return $"https://drive.google.com/file/d/{file.Id}/view";
            }

            return null;
        }
    }

}
