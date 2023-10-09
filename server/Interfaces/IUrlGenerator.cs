namespace server.Interfaces
{
    public interface IUrlGenerator
    {
        public string GenerateVerificationLink(string action, string token, string email);
    }
}
