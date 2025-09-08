using StartUp.Models;

namespace BOZMANOHERMANO.Models
{
    public class UserDM
    {
        public int Id { get; set; }

        public string Message { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.UtcNow;

        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        public string RecieverId { get; set; }
        public ApplicationUser Reciever { get; set; }
    }
}
