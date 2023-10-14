namespace server.DTOs
{
    public class JWTTokenDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
