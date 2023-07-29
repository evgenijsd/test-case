namespace test_case.api.Exceptions
{
    public class NoFileException : Exception
    {
        public NoFileException() : base("File has not been downloaded.") { }
    }
}
