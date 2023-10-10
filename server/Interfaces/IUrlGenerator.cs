namespace server.Interfaces
{
    public interface IUrlGenerator
    {
        public string GenerateVerificationLink(string token, string email);
    }
}
