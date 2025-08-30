namespace StartUp.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string TagName { get; set; } = string.Empty;
        public string? ProfilePicPath { get; set; } = string.Empty;
        public string? HeaderPath { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
    }
    public class EditUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string TagName { get; set; } = string.Empty;
        public IFormFile? ProfilePicPath { get; set; }
        public string? HeaderPath { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
    }
}
