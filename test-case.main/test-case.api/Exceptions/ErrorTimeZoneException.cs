namespace test_case.api.Exceptions
{
    public class ErrorTimeZoneException : Exception
    {
        public ErrorTimeZoneException(string exZone) : base($"Invalid {exZone} zone.") { }
    }
}
