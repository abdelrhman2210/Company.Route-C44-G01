namespace Company.Route_C44_G01.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1- upload folder
        // Get Folder Location
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //string folderPath = "D:\\AE\\Route\\EF Core\\Company.Route-C44-G01\\Company.Route-C44-G01.PL\\wwwroot\\Files\\"+ FolderName;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", FolderName);

            // Get File Name and make it unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}"; 


            // filePath
            var filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            
            file.CopyTo(fileStream);
        
            return fileName;
        }
        
        // 2- delete folder
        
    public static void DeleteFile(string fileName, string FolderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", FolderName);
            var filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}
