namespace APIClient.DTO
{
    public class WeatherDataDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Conditions { get; set; }
        public decimal Temperature { get; set; }
        public decimal WindSpeed { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
