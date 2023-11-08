namespace server.DTOs
{
    public class StandardServiceResponse<T>
    {
        public ResponseType ResponseType { get; set; }
        public T Data { get; set; }

        public StandardServiceResponse(ResponseType type, T data)
        {
            ResponseType = type;
            Data = data;
        }
    }

    public enum ResponseType { Success, BadRequest, NotFound, Unauthorized, InternalServerError, Conflict }
}
