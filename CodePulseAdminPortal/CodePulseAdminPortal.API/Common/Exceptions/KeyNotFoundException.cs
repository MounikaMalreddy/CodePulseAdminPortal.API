namespace CodePulseAdminPortal.API.Common.Exceptions
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string name, object key) 
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
