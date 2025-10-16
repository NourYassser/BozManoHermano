using StartUp.Models;

namespace BOZMANOHERMANO.Models
{
    public class PostsImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int PostsId { get; set; }
        public Posts Post { get; set; } = null!;
    }
}
