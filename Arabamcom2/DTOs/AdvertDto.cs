using Arabamcom2.Models;

namespace Arabamcom2.DTOs
{
    public class AdvertDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }
        public required string CarId { get; set; }
        public string City { get; set; }
    }
}
