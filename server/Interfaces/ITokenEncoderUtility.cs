namespace server.Interfaces
{
    public interface ITokenEncoderUtility
    {
        public string DecodeToken(string token);
        public string EncodeToken(string token);
    }
}
