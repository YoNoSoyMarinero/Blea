using Microsoft.AspNetCore.Mvc;
using server.DTOs;

namespace server.Wrappers
{
    public class ActionResultWrapper<T> : ActionResult
    {
        private readonly StandardServiceResponseDTO<T> _dto;

        public ActionResultWrapper(StandardServiceResponseDTO<T> dto)
        {
            _dto = dto;
        }

        public IActionResult GetResponse()
        {
            switch (_dto.ResponseType)
            {
                case ResponseType.Success:
                    return new OkObjectResult(_dto.Data);
                case ResponseType.NotFound:
                    return new NotFoundObjectResult(_dto.Data);
                case ResponseType.Unauthorized:
                    return new UnauthorizedObjectResult(_dto.Data);
                case ResponseType.BadRequest:
                    return new BadRequestObjectResult(_dto.Data);
                case ResponseType.Conflict:
                    return new ConflictObjectResult(_dto.Data);
                case ResponseType.InternalServerError:
                    return new ObjectResult(_dto.Data)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                default:
                    return new ObjectResult("Unknown status")
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
            }
        }
    }
}
