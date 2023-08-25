namespace Arabamcom2.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
