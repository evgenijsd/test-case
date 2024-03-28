using GeoTimeZone;
using NodaTime;
using NodaTime.Extensions;
using test_case.api.Exceptions;

namespace test_case.api.Converter
{
    public class TimeZoneConverter
    {
        private readonly IDateTimeZoneProvider _zoneProvider;

        public TimeZoneConverter()
        {
            _zoneProvider = DateTimeZoneProviders.Tzdb;
        }

        public string ConvertCoordinatesToTimeZone(double latitude, double longitude)
        {
            var timeZone = TimeZoneLookup.GetTimeZone(latitude, longitude);

            if (string.IsNullOrEmpty(timeZone?.Result))
                throw new ErrorTimeZoneException("coordinates");

            return timeZone.Result;
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
    }
}
