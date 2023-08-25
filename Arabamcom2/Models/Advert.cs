namespace Arabamcom2.Models
{
    public class Advert
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set;}
        public bool Active { get; set;}
        public string CarId { get; set; }
        public string City { get; set; }
    }
}
