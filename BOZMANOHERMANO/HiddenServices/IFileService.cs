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
                throw new ArgumentException("No file uploaded.");

            if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                || Path.GetExtension(file.FileName).ToLower() != ".jpeg"
                || Path.GetExtension(file.FileName).ToLower() != ".png")

                throw new ArgumentException("Only JPG/JPEG/PNG Pictures are allowed.");

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
