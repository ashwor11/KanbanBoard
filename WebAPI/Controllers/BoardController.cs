using Application.Features.Boards.Commands;
using Application.Features.Boards.Commands.AcceptBoardInvitationCommand;
using Application.Features.Boards.Commands.AddCartToBoardCommand;
using Application.Features.Boards.Commands.AddFeedbackToCardCommand;
using Application.Features.Boards.Commands.AddFeedbackToJobCommand;
using Application.Features.Boards.Commands.AddJobToCardCommand;
using Application.Features.Boards.Commands.AssignCardToPersonCommand;
using Application.Features.Boards.Commands.AssignDueDateToCardCommand;
using Application.Features.Boards.Commands.ChangeCardFeedBackCommand;
using Application.Features.Boards.Commands.ChangeCardNameCommand;
using Application.Features.Boards.Commands.ChangeJobDescriptionCommand;
using Application.Features.Boards.Commands.ChangeJobFeedbackCommand;
using Application.Features.Boards.Commands.ChangeStatusOfCardCommand;
using Application.Features.Boards.Commands.CreateBoardCommand;
using Application.Features.Boards.Commands.DeleteBoardCommand;
using Application.Features.Boards.Commands.DeleteCardCommand;
using Application.Features.Boards.Commands.DeleteCardFeedbackCommand;
using Application.Features.Boards.Commands.DeleteJobCommand;
using Application.Features.Boards.Commands.DeleteJobFeedbackCommand;
using Application.Features.Boards.Commands.InvitePersonToBoardCommand;
using Application.Features.Boards.Commands.RemoveAssignedDueDateFromCardCommand;
using Application.Features.Boards.Commands.RemoveAssignedPersonFromCardCommand;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Models;
using Application.Features.Boards.Queries.GetBoardAsWholeCommand;
using Application.Features.Boards.Queries.GetPersonsAllBoards;
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

        [HttpPost("board/{boardId}/cards/{cardId}/changeName")]
        public async Task<IActionResult> ChangeCardName([FromRoute] int boardId, [FromRoute] int cardId, [FromBody] string name)
        {
            int personId = GetPersonId();
            ChangeCardNameCommand changeCardNameCommand = new()
                { ChangeCardNameDto = new() { Name = name, BoardId = boardId, CardId = cardId }, PersonId = personId };
            await Mediator.Send(changeCardNameCommand);
            return Ok();
        }

        [HttpPost("board/{boardId}/cards/{cardId}/changeStatus")]
        public async Task<IActionResult> ChangeCardStatus([FromRoute] int boardId, [FromRoute] int cardId,
            [FromBody] string status)
        {
            int personId = GetPersonId();
            ChangeStatusOfCardCommand changeStatusOfCardCommand = new()
            {
                PersonId = personId, ChangeCardStatusDto = new() { BoardId = boardId, Status = status, CardId = cardId }
            };
            await Mediator.Send(changeStatusOfCardCommand);
            return Ok();

        }

        [HttpPost("board/{boardId}/cards/{cardId}/assignPerson")]
        public async Task<IActionResult> AssignPersonToCard([FromRoute] int boardId, [FromRoute] int cardId,
            [FromBody] int assignedPersonId)
        {
            int personId = GetPersonId();
            AssignCardToPersonCommand assignCardToPersonCommand = new()
            {
                PersonId = personId,
                AssignCardToPersonDto = new() { PersonId = assignedPersonId, BoardId = boardId, CardId = cardId }
            };
            await Mediator.Send(assignCardToPersonCommand);
            return Ok();
        }

        [HttpGet("board/{boardId}/cards/{cardId}/removeAssignedPerson")]
        public async Task<IActionResult> RemoveAssignedPersonToCard([FromRoute] int boardId, [FromRoute] int cardId)
        {
            int personId = GetPersonId();
            RemoveAssignedPersonFromCardCommand removeAssignedPersonFromCardCommand= new()
            {
                PersonId = personId,
                RemoveAssignedPersonFromCardDto = new() { BoardId = boardId, CardId = cardId }
            };
            await Mediator.Send(removeAssignedPersonFromCardCommand);
            return Ok();
        }

        [HttpPost("board/{boardId}/cards/{cardId}/assignDueDate")]
        public async Task<IActionResult> AssignDueDateToCard([FromRoute] int boardId, [FromRoute] int cardId,
            [FromBody] DateTime dueDate)
        {
            int personId = GetPersonId();
            AssignDueDateToCardCommand assignDueDateToCardCommand= new()
            {
                PersonId = personId,
                AssignDueDateToCardDto = new() { BoardId = boardId, CardId = cardId, DueDate = dueDate}
            };
            await Mediator.Send(assignDueDateToCardCommand);
            return Ok();
        }

        [HttpGet("board/{boardId}/cards/{cardId}/removeAssignedDueDate")]
        public async Task<IActionResult> RemoveAssignedDueDateFromCard([FromRoute] int boardId, [FromRoute] int cardId)
        {

            int personId = GetPersonId();
            RemoveAssignedDueDateFromCardCommand removeAssignedDueDateFromCardCommand= new()
            {
                PersonId = personId,
                RemoveAssignedDueDateFromCardDto = new() { BoardId = boardId, CardId = cardId }
            };
            await Mediator.Send(removeAssignedDueDateFromCardCommand);
            return Ok();
        }

        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetWholeBoard([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            GetBoardAsWholeCommand getBoardAsWholeCommand = new() { PersonId = personId, BoardId = boardId };
            GetWholeBoardDto getWholeBoardDto = await Mediator.Send(getBoardAsWholeCommand);
            return Ok(getWholeBoardDto);
        }

        [HttpGet("board/{boardId}/cards/{cardId}/addJob")]
        public async Task<IActionResult> AddJobToCard([FromRoute] int boardId, [FromRoute] int cardId)
        {
            int personId = GetPersonId();
            AddJobToCardCommand addJobToCardCommand = new()
                { AddJobToCardDto = new() { BoardId = boardId, CardId = cardId }, PersonId = personId };
            AddedJobDto addedJobDto = await Mediator.Send(addJobToCardCommand);
            return Ok(addedJobDto);
        }

        [HttpPost("board/{boardId}/cards/{cardId}/addFeedback")]
        public async Task<IActionResult> AddFeedbackToCard([FromRoute] int boardId, [FromRoute] int cardId,
            [FromBody] string content)
        {
            int personId = GetPersonId();
            AddFeedbackToCardCommand addFeedbackToCardCommand = new()
            {
                CardFeedbackToAddDto = new() { BoardId = boardId, CardId = cardId, Content = content },
                PersonId = personId
            };

            AddedCardFeedbackDto addedCardFeedbackDto = await Mediator.Send(addFeedbackToCardCommand);
            return Ok(addedCardFeedbackDto);
        }

        [HttpPost("board/{boardId}/cards/{cardId}/jobs/{jobId}/addFeedback")]
        public async Task<IActionResult> AddFeedbackToJob([FromRoute] int boardId, [FromRoute] int cardId, [FromRoute] int jobId,
            [FromBody] string content)
        {
            int personId = GetPersonId();
            AddFeedbackToJobCommand addFeedbackToJobCommand = new()
            {
                JobFeedbackToAddDto = new() { BoardId = boardId, CardId = cardId, Content = content, JobId = jobId},
                PersonId = personId
            };

            AddedJobFeedbackDto addedJobFeedbackDto = await Mediator.Send(addFeedbackToJobCommand);
            return Ok(addedJobFeedbackDto);
        }

        [HttpPost("board/{boardId}/cards/{cardId}/jobs/{jobId}/changeDescription")]
        public async Task<IActionResult> ChangeJobDescription([FromRoute] int boardId, [FromRoute] int cardId, [FromRoute] int jobId,
            [FromBody] string jobDescription)
        {
            int personId = GetPersonId();
            ChangeJobDescriptionCommand changeJobDescriptionCommand= new()
            {
                ChangeJobDescriptionDto = new() { BoardId = boardId, CardId = cardId, JobDescription = jobDescription, JobId = jobId },
                PersonId = personId
            };
            await Mediator.Send(changeJobDescriptionCommand);
            return Ok();
        }

        [HttpPost("board/{boardId}/cards/{cardId}/feedbacks/{cardFeedbackId}/changeFeedback")]
        public async Task<IActionResult> ChangeCardFeedback([FromRoute] int boardId, [FromRoute] int cardId,
            [FromRoute] int cardFeedbackId,
            [FromBody] string content)
        {
            int personId = GetPersonId();
            ChangeCardFeedbackCommand changeCardFeedbackCommand = new()
            {
                ChangeCardFeedbackDto = new() { BoardId = boardId, CardId = cardId, Content = content, CardFeedbackId= cardFeedbackId},
                PersonId = personId
            };
            await Mediator.Send(changeCardFeedbackCommand);
            return Ok();
        }

        [HttpPost("board/{boardId}/cards/{cardId}/jobs/{jobId}/feedbacks/{jobFeedbackId}/changeFeedback")]
        public async Task<IActionResult> ChangeJobFeedback([FromRoute] int boardId, [FromRoute] int cardId,
            [FromRoute] int jobFeedbackId,
            [FromRoute] int jobId,
            [FromBody] string content)
        {
            int personId = GetPersonId();
            ChangeJobFeedbackCommand changeJobFeedbackCommand = new()
            {
                ChangeJobFeedbackDto = new() { BoardId = boardId, CardId = cardId, Content = content, JobId = jobId, JobFeedbackId = jobFeedbackId},
                PersonId = personId
            };
            await Mediator.Send(changeJobFeedbackCommand);
            return Ok();
        }

        [HttpDelete("{boardId}/cards/{cardId}/delete")]
        public async Task<IActionResult> DeleteCard([FromRoute] int boardId, [FromRoute] int cardId)
        {
            int personId = GetPersonId();
            DeleteCardCommand deleteCardCommand = new()
                { DeleteCardDto = new() { BoardId = boardId, CardId = cardId }, PersonId = personId };
            await Mediator.Send(deleteCardCommand);
            return Ok();
        }

        [HttpDelete("{boardId}/cards/{cardId}/feedbacks/{cardFeedbackId}/delete")]
        public async Task<IActionResult> DeleteCard([FromRoute] int boardId, [FromRoute] int cardId, [FromRoute] int cardFeedbackId)
        {
            int personId = GetPersonId();
            DeleteCardFeedbackCommand deleteCardFeedbackCommand = new()
                { DeleteCardFeedbackDto = new() { BoardId = boardId, CardId = cardId, CardFeedbackId = cardFeedbackId}, PersonId = personId };
            await Mediator.Send(deleteCardFeedbackCommand);
            return Ok();
        }

        [HttpDelete("{boardId}/cards/{cardId}/jobs/{jobId}/delete")]
        public async Task<IActionResult> DeleteJob([FromRoute] int boardId, [FromRoute] int cardId, [FromRoute] int jobId)
        {
            int personId = GetPersonId();
            DeleteJobCommand deleteJobCommand = new()
                { DeleteJobDto = new() { BoardId = boardId, CardId = cardId, JobId = jobId}, PersonId = personId };
            await Mediator.Send(deleteJobCommand);
            return Ok();
        }

        [HttpDelete("{boardId}/cards/{cardId}/jobs/{jobId}/feedbacks/{jobFeedbackId}/delete")]
        public async Task<IActionResult> DeleteJobFeedback([FromRoute] int boardId, [FromRoute] int cardId, [FromRoute] int jobId, [FromRoute] int jobFeedbackId)
        {
            int personId = GetPersonId();
            DeleteJobFeedbackCommand deleteJobFeedbackCommand = new()
                { DeleteJobFeedbackDto = new() { BoardId = boardId, CardId = cardId, JobId = jobId,JobFeedbackId = jobFeedbackId}, PersonId = personId };
            await Mediator.Send(deleteJobFeedbackCommand);
            return Ok();
        }

        [HttpGet("/boards")]
        public async Task<IActionResult> GetPersonsBoard()
        {
            int personId = GetPersonId();

            GetPersonsAllBoardsQuery getPersonsAllBoardsQuery = new() { PersonId = personId };

            GetPersonsAllBoardsModel getPersonsAllBoardsModel = await Mediator.Send(getPersonsAllBoardsQuery);

            return Ok(getPersonsAllBoardsModel);
        }







    }
}
