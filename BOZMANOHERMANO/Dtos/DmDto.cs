namespace BOZMANOHERMANO.Dtos
{
    public class DmDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
        public string SenderId { get; set; }
        public string UserName { get; set; }
        public string TagName { get; set; }
    }
    public class ChatDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string UserName { get; set; }
        public string TagName { get; set; }
        public List<MessagesDto> Messages { get; set; }
    }
    public class MessagesDto
    {
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
