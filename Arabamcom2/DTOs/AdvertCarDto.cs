namespace Arabamcom2.DTOs
{
    public class AdvertCarDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }
        public required string CarId { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public float Km { get; set; }
        public string Color { get; set; }
        public string Gear { get; set; }
        public string Fuel { get; set; }
    }
}
