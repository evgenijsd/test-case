using GeoTimeZone;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using test_case.api.Exceptions;
using test_case.api.Models.TimeZone;

namespace test_case.api.Converter
{
    public class TimeZoneConverter
    {
        private readonly IDateTimeZoneProvider _zoneProvider;
        private readonly HttpClient _client;

        public TimeZoneConverter()
        {
            _zoneProvider = DateTimeZoneProviders.Tzdb;
            _client = new HttpClient();
        }

        public async Task<string> ConvertCoordinatesToTimeZoneAsync(double latitude, double longitude)
        {
            var result = string.Empty;
            var timeZone = TimeZoneLookup.GetTimeZone(latitude, longitude);
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (string.IsNullOrEmpty(timeZone?.Result))
            {
                var zone = await GetTimeZone($"http://api.timezonedb.com/v2.1/get-time-zone?key={apiKey}&format=json&by=position&lat={latitude}&lng={longitude}");
                result = zone?.ZoneName;
            }
            else
            {
                result = timeZone.Result;
            }

            if (string.IsNullOrEmpty(result))
                throw new ErrorTimeZoneException("coordinates");

            return result;
        }

        public DateTime ConvertDateToUtc(DateTime date, string timeZoneId)
        {
            var zone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);            

            if (zone == null)                
                throw new ErrorTimeZoneException("ID");

            var localDateTime = LocalDateTime.FromDateTime(date);
            var zonedDateTime = localDateTime.InZoneLeniently(zone);
            return zonedDateTime.ToDateTimeUtc();
        }

        public int GetOffsetByDateInSeconds(DateTime date, string timeZoneId)
        {
            var zone = _zoneProvider.GetZoneOrNull(timeZoneId);

            if (zone == null)
                throw new ErrorTimeZoneException("ID");

            var localDateTime = LocalDateTime.FromDateTime(date);
            var zonedDateTime = localDateTime.InZoneLeniently(zone);
            var offset = zonedDateTime.Offset;
            return (int)offset.Seconds;
        }

        public async Task<ApiZone?> GetTimeZone(string query)
        {
            ApiZone? result = null;
            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApiZone?>(content);
                }
                else
                    result = null;
            }
            catch
            {
                result = null;
            }

            return result;
        }
    }
}
