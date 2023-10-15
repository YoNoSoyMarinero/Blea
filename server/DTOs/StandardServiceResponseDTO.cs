using server;

namespace server.DTOs
{
    public class StandardServiceResponseDTO
    {
        public ResponseType ResponseType { get; set; }
        public object Data { get; set; }

        public StandardServiceResponseDTO (ResponseType type, object data) 
        {
            ResponseType = type;
            Data = data; 
        }
    }
 
    public enum ResponseType { Success, BadRequest, NotFound, Unauthorized, InternalServerError, Conflict }
}