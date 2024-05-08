using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("/Api/V1/[controller]")]
    public class NewPostController(ICommandDispatcher dispatcher, ILogger<NewPostController> logger) : ControllerBase
    {
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        private readonly ILogger<NewPostController> _logger = logger;

        [HttpPost]
        public async Task<ActionResult> CreateAsync(NewPostCommand command)
        {
            var id = Guid.NewGuid();

            try
            {
                command.Id = id;
                await _dispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Id = id,
                    Message = "New Post creation request completed successfully"
                });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(e, "Clieant made a bad request");
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                {
                    Message = "Bad input"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to create a new post");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "Error while proccessing the request to create a new post"
                });
            }

        }

    }
}