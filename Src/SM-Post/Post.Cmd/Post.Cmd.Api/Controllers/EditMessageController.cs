using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.Common.Constants;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("/Api/V1/[controller]")]
    public class EditMessageController(ICommandDispatcher dispatcher, ILogger<EditMessageController> logger) : ControllerBase
    {
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        private readonly ILogger<EditMessageController> _logger = logger;

        [HttpPost("{id}")]
        public async Task<ActionResult> EditMessageAsync(Guid id, EditMessageCommand command)
        {
            try
            {
                command.Id = id;

                await _dispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = ApiMessages.Success
                });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(e, ErrorMessages.ClientBadRequest);
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                {
                    Message = ErrorMessages.BadInput
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessages.FailedToCreatePost);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = ErrorMessages.ErrorWhileProcessingRequest
                });
            }
        }
    }
}