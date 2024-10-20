using System.Net.Mime;
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
using Application.Features.Boards.Commands.LeaveBoardCommand;
using Application.Features.Boards.Commands.MarkJobAsDone;
using Application.Features.Boards.Commands.MarkJobAsUnDoneCommand;
using Application.Features.Boards.Commands.RemoveAssignedDueDateFromCardCommand;
using Application.Features.Boards.Commands.RemoveAssignedPersonFromCardCommand;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Queries.GetBoardAsWholeCommand;
using Application.Features.Boards.Queries.GetPersonsAllBoards;
using Application.Services.Interfaces;
using Core.CrossCuttingConcerns.Exceptions.HttpExceptions;
using Core.Security.JWT;
using Infrastructure.SSE.BoardUpdate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class BoardController : BaseController
    {
        private readonly IBoardConnectionManager _boardConnectionManager;
        private readonly IEventPublisher _eventPublisher;


        public BoardController(IBoardConnectionManager boardConnectionManager, IEventPublisher eventPublisher)
        {
            _boardConnectionManager = boardConnectionManager;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Creates a board
        /// </summary>
        /// <param name="boardToCreateDto"></param>
        /// <returns>A board dto with boardId</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(CreatedBoardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBoard(BoardToCreateDto boardToCreateDto)
        {
            int personId = GetPersonId();
            CreateBoardCommand createBoardCommand = new() { BoardToCreateDto = boardToCreateDto, PersonId = personId };

            CreatedBoardDto createdBoardDto = await Mediator.Send(createBoardCommand);
            return Ok(createdBoardDto);
        }


        /// <summary>
        /// Deletes the board with specified id
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpDelete("{boardId}/delete")]
        [ProducesResponseType(typeof(DeletedBoardDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBoard([FromRoute]int boardId)
        {
            int personId = GetPersonId();
            DeleteBoardCommand deleteBoardCommand = new() { BoardId = boardId ,PersonId = personId};
            DeletedBoardDto deletedBoardDto = await Mediator.Send(deleteBoardCommand);
            return Ok(deletedBoardDto);
        }
        /// <summary>
        /// Sends an email to the invited person. InvitatonAcceptUrlPrefix should be given as a full path url for frontend.
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="invitePersonToBoardBody"></param>
        /// <returns></returns>
        
        [HttpPost("{boardId}/invitePersonToBoard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InvitePersonToBoard([FromRoute] int boardId, [FromBody] InvitePersonToBoardBody invitePersonToBoardBody)
        {
            int personId = GetPersonId();
            InvitePersonToBoardDto invitePersonToBoardDto = new(){BoardId = boardId,InvitedPersonEmail = invitePersonToBoardBody.InvitedPersonEmail, };
            InvitePersonToBoardCommand invitePersonToBoardCommand = new()
                { PersonId = personId, InvitePersonToBoardDto = invitePersonToBoardDto, InvitationAcceptUrlPrefix = invitePersonToBoardBody.InvitationAcceptUrlPrefix };

                string result = await Mediator.Send(invitePersonToBoardCommand);

            return Ok(result);
        }
        /// <summary>
        /// The token part on the link sent the invited person's email should be given as a query parameter.
        /// </summary>
        /// <param name="InvitationAcceptToken"></param>
        /// <returns></returns>
        [HttpGet("acceptInvitation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AcceptBoardInvitation([FromQuery] string InvitationAcceptToken)
        {
            int personId = GetPersonId();

            AcceptBoardInvitationCommand acceptBoardInvitationCommand =
                new() { PersonId = personId, InvitationToken = InvitationAcceptToken };
            string result = await Mediator.Send(acceptBoardInvitationCommand);

            return Ok(result);
        }
        /// <summary>
        /// Adds new card to the board.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/addCard")]
        [ProducesResponseType(typeof(AddedCardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddCardToBoard([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            AddCardToBoardCommand addCartToBoardCommand =
                new() { CardToAddDto = new() { BoardId = boardId }, PersonId = personId };
            AddedCardDto addedCardDto = await Mediator.Send(addCartToBoardCommand);

            return Ok(addedCardDto);
        }

        /// <summary>
        /// Changes card name
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/changeName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeCardName([FromRoute] int boardId, [FromRoute] int cardId, [FromBody] string name)
        {
            int personId = GetPersonId();
            ChangeCardNameCommand changeCardNameCommand = new()
                { ChangeCardNameDto = new() { Name = name, BoardId = boardId, CardId = cardId }, PersonId = personId };
            await Mediator.Send(changeCardNameCommand);
            return Ok();
        }

        /// <summary>
        /// Changes card's status
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/changeStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Assigns a person to the card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="assignedPersonId"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/assignPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Removes assigned person from the card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/cards/{cardId}/removeAssignedPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Assigns due date to the card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/assignDueDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Removes assigned date from card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/cards/{cardId}/removeAssignedDueDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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


        /// <summary>
        /// Gets all the details of the board including persons on board
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}")]
        [ProducesResponseType(typeof(GetBoardByIdDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetWholeBoard([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            GetBoardAsWholeQuery getBoardAsWholeQuery = new() { PersonId = personId, BoardId = boardId };
            GetBoardByIdDto getWholeBoardDto = await Mediator.Send(getBoardAsWholeQuery);
            return Ok(getWholeBoardDto);
        }


        /// <summary>
        /// Adds a job to the card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/cards/{cardId}/addJob")]
        [ProducesResponseType(typeof(AddedJobDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddJobToCard([FromRoute] int boardId, [FromRoute] int cardId)
        {
            int personId = GetPersonId();
            AddJobToCardCommand addJobToCardCommand = new()
                { AddJobToCardDto = new() { BoardId = boardId, CardId = cardId }, PersonId = personId };
            AddedJobDto addedJobDto = await Mediator.Send(addJobToCardCommand);
            return Ok(addedJobDto);
        } 

        /// <summary>
        /// Add new feedback to the card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/addFeedback")]
        [ProducesResponseType(typeof(AddedCardFeedbackDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Adds new feedback to the job
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/jobs/{jobId}/addFeedback")]
        [ProducesResponseType((typeof(AddedJobFeedbackDto)),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Changes job's description
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/jobs/{jobId}/changeDescription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Changes specified card feedback 
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="cardFeedbackId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/feedbacks/{cardFeedbackId}/changeFeedback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Changes specified job feedback
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobFeedbackId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/jobFeedbacks/{jobFeedbackId}/changeFeedback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Deletes specified card from board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpDelete("{boardId}/cards/{cardId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCard([FromRoute] int boardId, [FromRoute] int cardId)
        {
            int personId = GetPersonId();
            DeleteCardCommand deleteCardCommand = new()
                { DeleteCardDto = new() { BoardId = boardId, CardId = cardId }, PersonId = personId };
            await Mediator.Send(deleteCardCommand);
            return NoContent();
        }

        /// <summary>
        /// Deletes specified job from card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardFeedbackId"></param>
        /// <returns></returns>
        [HttpDelete("{boardId}/cardFeedbacks/{cardFeedbackId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCardFeedback([FromRoute] int boardId, [FromRoute] int cardFeedbackId)
        {
            int personId = GetPersonId();
            DeleteCardFeedbackCommand deleteCardFeedbackCommand = new()
                { DeleteCardFeedbackDto = new() { BoardId = boardId, CardFeedbackId = cardFeedbackId}, PersonId = personId };
            await Mediator.Send(deleteCardFeedbackCommand);
            return NoContent();
        }

        /// <summary>
        /// Deletes specified job from card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpDelete("{boardId}/jobs/{jobId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteJob([FromRoute] int boardId, [FromRoute] int jobId)
        {
            int personId = GetPersonId();
            DeleteJobCommand deleteJobCommand = new()
                { DeleteJobDto = new() { BoardId = boardId, JobId = jobId}, PersonId = personId };
            await Mediator.Send(deleteJobCommand);
            return NoContent();
        }

        /// <summary>
        /// Deletes specified job feedback from job
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobFeedbackId"></param>
        /// <returns></returns>
        [HttpDelete("{boardId}/jobFeedbacks/{jobFeedbackId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteJobFeedback([FromRoute] int boardId,[FromRoute] int jobFeedbackId)
        {
            int personId = GetPersonId();
            DeleteJobFeedbackCommand deleteJobFeedbackCommand = new()
                { DeleteJobFeedbackDto = new() { BoardId = boardId,JobFeedbackId = jobFeedbackId}, PersonId = personId };
            await Mediator.Send(deleteJobFeedbackCommand);
            return NoContent();
        }

        /// <summary>
        /// Gets all the boards of the person who is logged in
        /// </summary>
        /// <returns></returns>
        [HttpGet("boards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPersonsBoard()
        {
            int personId = GetPersonId();

            GetPersonsAllBoardsQuery getPersonsAllBoardsQuery = new() { PersonId = personId };

            List<GetPersonsBoardDto> getPersonsBoardDtos = await Mediator.Send(getPersonsAllBoardsQuery);

            return Ok(getPersonsBoardDtos);
        }

        /// <summary>
        /// Marks specified job as done
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/jobs/{jobId}/markAsDone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarkJobAsDone([FromRoute] int boardId, [FromRoute] int jobId){
            int personId = GetPersonId();

            MarkJobAsDoneCommand markJobAsDoneCommand = new()
                { PersonId = personId, MarkJobDto = new() { BoardId = boardId, JobId = jobId } };
            await Mediator.Send(markJobAsDoneCommand);
            return Ok();
        }

        /// <summary>
        /// Marks specified job as undone
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/jobs/{jobId}/markAsUnDone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarkJobAsUnDone([FromRoute] int boardId, [FromRoute] int jobId)
        {
            int personId = GetPersonId();

            MarkJobAsUnDoneCommand markJobAsUnDoneCommand = new()
                { PersonId = personId, MarkJobDto = new() { BoardId = boardId, JobId = jobId } };
            await Mediator.Send(markJobAsUnDoneCommand);
            return Ok();
        }

        /// <summary>
        /// Changes the color of the specified card
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardId"></param>
        /// <param name="colorInHex"></param>
        /// <returns></returns>
        [HttpPost("{boardId}/cards/{cardId}/changeColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// The person who is logged in leaves the board
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet("{boardId}/leave")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BusinessProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorizationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UnauthorizedProblemDetails), StatusCodes.Status403Forbidden)]
        public async   Task<IActionResult> LeaveTable([FromRoute] int boardId)
        {
            int personId = GetPersonId();
            LeaveBoardCommand leaveBoardCommand = new()
            { LeaveBoardDto = new() { BoardId = boardId }, PersonId = personId };
            await Mediator.Send(leaveBoardCommand);
            return Ok();
        }

        [HttpGet("subscribe/{boardId}")]
        public async Task Subscribe(int boardId, CancellationToken cancellationToken)
        {
            var response = Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.Headers.Add("Cache-Control", "no-cache");
            response.Headers.Add("Connection", "keep-alive");

            var boardConnection = new BoardConnectionImplementation(Guid.NewGuid().ToString(), boardId, response.Body);
            _boardConnectionManager.AddConnection(boardId, boardConnection);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                    await response.WriteAsync("data: keepalive\n\n");
                    await response.Body.FlushAsync();
                }
            }
            finally
            {
                _boardConnectionManager.RemoveConnection(boardId, boardConnection.ConnectionId);
            }
        }

       




        public class InvitePersonToBoardBody
        {
            public string InvitationAcceptUrlPrefix { get; set; }
            public string InvitedPersonEmail { get; set; }
        }


    }
}
