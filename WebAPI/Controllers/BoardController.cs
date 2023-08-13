using Application.Features.Boards.Commands;
using Application.Features.Boards.Commands.AcceptBoardInvitationCommand;
using Application.Features.Boards.Commands.AddCartToBoardCommand;
using Application.Features.Boards.Commands.CreateBoardCommand;
using Application.Features.Boards.Commands.DeleteBoardCommand;
using Application.Features.Boards.Commands.InvitePersonToBoardCommand;
using Application.Features.Boards.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : BaseController
    {

        [HttpPost("create")]
        public async Task<IActionResult> CreateBoard(BoardToCreateDto boardToCreateDto)
        {
            int personId = GetPersonId();
            CreateBoardCommand createBoardCommand = new() { BoardToCreateDto = boardToCreateDto, PersonId = personId };

            CreatedBoardDto createdBoardDto = await Mediator.Send(createBoardCommand);
            return Ok(createdBoardDto);
        }

        [HttpDelete("delete/{boardId}")]
        public async Task<IActionResult> DeleteBoard([FromRoute]int boardId)
        {
            int personId = GetPersonId();
            DeleteBoardCommand deleteBoardCommand = new() { BoardId = boardId ,PersonId = personId};
            DeletedBoardDto deletedBoardDto = await Mediator.Send(deleteBoardCommand);
            return Ok(deletedBoardDto);
        }

        [HttpPost("board/{boardId}/invitePersonToBoard")]
        public async Task<IActionResult> InvitePersonToBoard([FromRoute] int boardId, [FromBody] string invitedPersonEmail)
        {
            int personId = GetPersonId();
            InvitePersonToBoardDto invitePersonToBoardDto = new(){BoardId = boardId,InvitedPersonEmail = invitedPersonEmail, };
            InvitePersonToBoardCommand invitePersonToBoardCommand = new()
                { PersonId = personId, InvitePersonToBoardDto = invitePersonToBoardDto, InvitationAcceptUrlPrefix = HttpContext.Request.Host.Value + "/acceptInvitation"};

                string result = await Mediator.Send(invitePersonToBoardCommand);

            return Ok(result);
        }

        [HttpGet("acceptInvitation")]
        public async Task<IActionResult> AcceptBoardInvitation([FromQuery] string InvitationAcceptToken)
        {
            int personId = GetPersonId();

            AcceptBoardInvitationCommand acceptBoardInvitationCommand =
                new() { PersonId = personId, InvitationToken = InvitationAcceptToken };
            string result = await Mediator.Send(acceptBoardInvitationCommand);

            return Ok(result);
        }

        [HttpGet("board/{boardId}/addCard")]
        public async Task<IActionResult> AddCardToBoard([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            AddCartToBoardCommand addCartToBoardCommand =
                new() { CardToAddDto = new() { BoardId = boardId }, PersonId = personId };
            AddedCardDto addedCardDto = await Mediator.Send(addCartToBoardCommand);

            return Ok(addedCardDto);
        }
    }
}
