using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
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
            command.Id = id;

            await _dispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new BaseResponse
            {
                Message = "Edit request message completed successfully"
            });
        }
    }
}