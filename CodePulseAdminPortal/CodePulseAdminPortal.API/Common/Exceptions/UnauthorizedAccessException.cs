namespace CodePulseAdminPortal.API.Common.Exceptions
{
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string? message) : base(message)
        {
        }
    }
}
