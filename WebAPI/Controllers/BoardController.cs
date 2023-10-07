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
using Application.Features.Boards.Commands.ChangeColorOfCardCommand;
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
using Application.Features.Boards.Commands.MarkJobAsDone;
using Application.Features.Boards.Commands.MarkJobAsUnDoneCommand;
using Application.Features.Boards.Commands.RemoveAssignedDueDateFromCardCommand;
using Application.Features.Boards.Commands.RemoveAssignedPersonFromCardCommand;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Queries.GetBoardAsWholeCommand;
using Application.Features.Boards.Queries.GetPersonsAllBoards;
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

        [HttpPost("{boardId}/invitePersonToBoard")]
        public async Task<IActionResult> InvitePersonToBoard([FromRoute] int boardId, [FromBody] InvitePersonToBoardBody invitePersonToBoardBody)
        {
            int personId = GetPersonId();
            InvitePersonToBoardDto invitePersonToBoardDto = new(){BoardId = boardId,InvitedPersonEmail = invitePersonToBoardBody.InvitedPersonEmail, };
            InvitePersonToBoardCommand invitePersonToBoardCommand = new()
                { PersonId = personId, InvitePersonToBoardDto = invitePersonToBoardDto, InvitationAcceptUrlPrefix = invitePersonToBoardBody.InvitationAcceptUrlPrefix };

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

        [HttpGet("{boardId}/addCard")]
        public async Task<IActionResult> AddCardToBoard([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            AddCartToBoardCommand addCartToBoardCommand =
                new() { CardToAddDto = new() { BoardId = boardId }, PersonId = personId };
            AddedCardDto addedCardDto = await Mediator.Send(addCartToBoardCommand);

            return Ok(addedCardDto);
        }

        [HttpPost("{boardId}/cards/{cardId}/changeName")]
        public async Task<IActionResult> ChangeCardName([FromRoute] int boardId, [FromRoute] int cardId, [FromBody] string name)
        {
            int personId = GetPersonId();
            ChangeCardNameCommand changeCardNameCommand = new()
                { ChangeCardNameDto = new() { Name = name, BoardId = boardId, CardId = cardId }, PersonId = personId };
            await Mediator.Send(changeCardNameCommand);
            return Ok();
        }

        [HttpPost("{boardId}/cards/{cardId}/changeStatus")]
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

        [HttpPost("{boardId}/cards/{cardId}/assignPerson")]
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

        [HttpGet("{boardId}/cards/{cardId}/removeAssignedPerson")]
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

        [HttpPost("{boardId}/cards/{cardId}/assignDueDate")]
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

        [HttpGet("{boardId}/cards/{cardId}/removeAssignedDueDate")]
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

        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetWholeBoard([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            GetBoardAsWholeQuery getBoardAsWholeQuery = new() { PersonId = personId, BoardId = boardId };
            GetBoardByIdDto getWholeBoardDto = await Mediator.Send(getBoardAsWholeQuery);
            return Ok(getWholeBoardDto);
        }

        [HttpGet("{boardId}/cards/{cardId}/addJob")]
        public async Task<IActionResult> AddJobToCard([FromRoute] int boardId, [FromRoute] int cardId)
        {
            int personId = GetPersonId();
            AddJobToCardCommand addJobToCardCommand = new()
                { AddJobToCardDto = new() { BoardId = boardId, CardId = cardId }, PersonId = personId };
            AddedJobDto addedJobDto = await Mediator.Send(addJobToCardCommand);
            return Ok(addedJobDto);
        } 

        [HttpPost("{boardId}/cards/{cardId}/addFeedback")]
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

        [HttpPost("{boardId}/jobs/{jobId}/addFeedback")]
        public async Task<IActionResult> AddFeedbackToJob([FromRoute] int boardId, [FromRoute] int jobId,
            [FromBody] string content)
        {
            int personId = GetPersonId();
            AddFeedbackToJobCommand addFeedbackToJobCommand = new()
            {
                JobFeedbackToAddDto = new() { BoardId = boardId, Content = content, JobId = jobId},
                PersonId = personId
            };

            AddedJobFeedbackDto addedJobFeedbackDto = await Mediator.Send(addFeedbackToJobCommand);
            return Ok(addedJobFeedbackDto);
        }

        [HttpPost("{boardId}/jobs/{jobId}/changeDescription")]
        public async Task<IActionResult> ChangeJobDescription([FromRoute] int boardId, [FromRoute] int jobId,
            [FromBody] string description)
        {
            int personId = GetPersonId();
            ChangeJobDescriptionCommand changeJobDescriptionCommand= new()
            {
                ChangeJobDescriptionDto = new() { BoardId = boardId, JobDescription = description, JobId = jobId },
                PersonId = personId
            };
            await Mediator.Send(changeJobDescriptionCommand);
            return Ok();
        }

        [HttpPost("{boardId}/cards/{cardId}/feedbacks/{cardFeedbackId}/changeFeedback")]
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

        [HttpPost("{boardId}/jobFeedbacks/{jobFeedbackId}/changeFeedback")]
        public async Task<IActionResult> ChangeJobFeedback([FromRoute] int boardId, 
            [FromRoute] int jobFeedbackId,
            [FromBody] string content)
        {
            int personId = GetPersonId();
            ChangeJobFeedbackCommand changeJobFeedbackCommand = new()
            {
                ChangeJobFeedbackDto = new() { BoardId = boardId, JobFeedbackId = jobFeedbackId},
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
            return NoContent();
        }

        [HttpDelete("{boardId}/cardFeedbacks/{cardFeedbackId}/delete")]
        public async Task<IActionResult> DeleteCardFeedback([FromRoute] int boardId, [FromRoute] int cardFeedbackId)
        {
            int personId = GetPersonId();
            DeleteCardFeedbackCommand deleteCardFeedbackCommand = new()
                { DeleteCardFeedbackDto = new() { BoardId = boardId, CardFeedbackId = cardFeedbackId}, PersonId = personId };
            await Mediator.Send(deleteCardFeedbackCommand);
            return NoContent();
        }

        [HttpDelete("{boardId}/jobs/{jobId}/delete")]
        public async Task<IActionResult> DeleteJob([FromRoute] int boardId, [FromRoute] int jobId)
        {
            int personId = GetPersonId();
            DeleteJobCommand deleteJobCommand = new()
                { DeleteJobDto = new() { BoardId = boardId, JobId = jobId}, PersonId = personId };
            await Mediator.Send(deleteJobCommand);
            return NoContent();
        }

        [HttpDelete("{boardId}/jobFeedbacks/{jobFeedbackId}/delete")]
        public async Task<IActionResult> DeleteJobFeedback([FromRoute] int boardId,[FromRoute] int jobFeedbackId)
        {
            int personId = GetPersonId();
            DeleteJobFeedbackCommand deleteJobFeedbackCommand = new()
                { DeleteJobFeedbackDto = new() { BoardId = boardId,JobFeedbackId = jobFeedbackId}, PersonId = personId };
            await Mediator.Send(deleteJobFeedbackCommand);
            return NoContent();
        }

        [HttpGet("boards")]
        public async Task<IActionResult> GetPersonsBoard()
        {
            int personId = GetPersonId();

            GetPersonsAllBoardsQuery getPersonsAllBoardsQuery = new() { PersonId = personId };

            List<GetPersonsBoardDto> getPersonsBoardDtos = await Mediator.Send(getPersonsAllBoardsQuery);

            return Ok(getPersonsBoardDtos);
        }
        [HttpGet("{boardId}/jobs/{jobId}/markAsDone")]
        public async Task<IActionResult> MarkJobAsDone([FromRoute] int boardId, [FromRoute] int jobId){
            int personId = GetPersonId();

            MarkJobAsDoneCommand markJobAsDoneCommand = new()
                { PersonId = personId, MarkJobDto = new() { BoardId = boardId, JobId = jobId } };
            await Mediator.Send(markJobAsDoneCommand);
            return Ok();
        }
        [HttpGet("{boardId}/jobs/{jobId}/markAsUnDone")]
        public async Task<IActionResult> MarkJobAsUnDone([FromRoute] int boardId, [FromRoute] int jobId)
        {
            int personId = GetPersonId();

            MarkJobAsUnDoneCommand markJobAsUnDoneCommand = new()
                { PersonId = personId, MarkJobDto = new() { BoardId = boardId, JobId = jobId } };
            await Mediator.Send(markJobAsUnDoneCommand);
            return Ok();
        }

        [HttpPost("{boardId}/cards/{cardId}/changeColor")]
        public async Task<IActionResult> ChangeCardColor([FromRoute] int boardId, [FromRoute] int cardId,
            [FromBody] string colorInHex)
        {
            int personId = GetPersonId();
            ChangeCardColorCommand changeCardColorCommand = new()
            {
                PersonId = personId,
                ChangeCardColorDto = new ChangeCardColorDto()
                    { BoardId = boardId, CardId = cardId, ColorInHex = colorInHex }
            };

            await Mediator.Send(changeCardColorCommand);
            return Ok();
        }




        public class InvitePersonToBoardBody
        {
            public string InvitationAcceptUrlPrefix { get; set; }
            public string InvitedPersonEmail { get; set; }
        }


    }
}
