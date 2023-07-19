namespace EmailMS.Models
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? Priority { get; set; }
    }
}
