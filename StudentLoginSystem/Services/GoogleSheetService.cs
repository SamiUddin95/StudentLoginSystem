using Newtonsoft.Json;
using StudentLoginSystem.Models;

namespace StudentLoginSystem.Services
{
    public class GoogleSheetService
    {

        private const string CsvUrl = "https://docs.google.com/spreadsheets/d/1he_O73c-kr8jyjLxJRA3f3kTij79WgEXungoUUPuWGQ/export?format=csv&gid=0";

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = new List<UserModel>();

            using var httpClient = new HttpClient();
            var csvData = await httpClient.GetStringAsync(CsvUrl);

            var lines = csvData.Split('\n');
            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                var columns = lines[i].Split(',');

                if (columns.Length < 8) continue;

                users.Add(new UserModel
                {
                    RollNumber = columns[0].Trim(),
                    StudentName = columns[1].Trim(),
                    FatherName = columns[2].Trim(),
                    Program = columns[3].Trim(),
                    Group = columns[4].Trim(),
                    StandardClass = columns[5].Trim(),
                    Password = columns[8].Trim()
                });
            }

            return users;
        }
    }
}
