namespace StartUp.HiddenServices
{
    public interface IFileService
    {
        string UploadPPAsync(IFormFile file, string folderName);
    }
    public class FileService : IFileService
    {
        public string UploadPPAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                return "Only JPG, JPEG, or PNG pictures are allowed.";


            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
