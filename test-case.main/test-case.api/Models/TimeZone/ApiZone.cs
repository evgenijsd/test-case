namespace test_case.api.Models.TimeZone
{
    public class ApiZone
    {
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? ZoneName { get; set; }
        public string? Abbreviation { get; set; }
        public int GmtOffset { get; set; }
        public int Dst { get; set; }
        public int ZoneStart { get; set; }
        public int ZoneEnd { get; set; }
        public long Timestamp { get; set; }
        public string? Formatted { get; set; }
    }
}
