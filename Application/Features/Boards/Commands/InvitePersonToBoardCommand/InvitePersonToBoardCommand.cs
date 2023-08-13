using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Application.Services.Abstract;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Validation;
using Core.Security.Encryption.Helpers;
using Core.Security.JWT;
using Core.Security.Mailing;
using Domain.Entities.Concrete;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Boards.Commands.InvitePersonToBoardCommand;

public class InvitePersonToBoardCommand : IRequest<string>, IValidationRequest, ISecuredRequest
{
    public InvitePersonToBoardDto InvitePersonToBoardDto { get; set; }
    public int PersonId { get; set; }
    public string InvitationAcceptUrlPrefix { get; set; }
    public string[] RequiredRoles { get; set; }


    public class InvitePersonToBoardCommandHandler : IRequestHandler<InvitePersonToBoardCommand,string>
    {
        private readonly IMailService _mailService;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IPersonService _personService;
        private readonly IBoardRepository _boardRepository;
        private readonly TokenOptions _tokenOptions;

        public InvitePersonToBoardCommandHandler(IMailService mailService, BoardBusinessRules boardBusinessRules, IPersonService personService, IBoardRepository boardRepository, IOptions<TokenOptions> tokenOptions)
        {
            _mailService = mailService;
            _boardBusinessRules = boardBusinessRules;
            _personService = personService;
            _boardRepository = boardRepository;
            _tokenOptions = tokenOptions.Value;
        }

        public async Task<string> Handle(InvitePersonToBoardCommand request, CancellationToken cancellationToken)
        {
            await _boardBusinessRules.IsPersonAlreadyInBoard(request.InvitePersonToBoardDto.InvitedPersonEmail,
                request.InvitePersonToBoardDto.BoardId);

            Person senderPerson = await _personService.GetPersonWithId(request.PersonId);
            Board board = await _boardRepository.GetAsync(x => x.Id == request.InvitePersonToBoardDto.BoardId)!;
            _boardBusinessRules.DoesBoardExist(board);
            await SendBoardInvitationEmail(senderPerson,request.InvitePersonToBoardDto.InvitedPersonEmail,board, request.InvitationAcceptUrlPrefix);


            return $"Invitation email successfully sent to {request.InvitePersonToBoardDto.InvitedPersonEmail}";


        }

        private async Task SendBoardInvitationEmail(Person senderPerson, string receiverEmail, Board board, string invitationAcceptUrlPrefix)
        {
            Mail mail = CreateInvitationEmail(senderPerson, receiverEmail, board, invitationAcceptUrlPrefix);
            await _mailService.SendEmail(mail);
        }

        private Mail CreateInvitationEmail(Person senderPerson, string receiverEmail, Board board, string invitationAcceptUrlPrefix)
        {
            string invitationAcceptToken = CreateInvitationAcceptToken(receiverEmail, board.Id);
            Mail mail = new()
            {
                ToEmail = receiverEmail,
                Subject = senderPerson.FirstName + senderPerson.LastName + $" invited you to the {board.Name}.",
                HtmlBody =
                    $"To accept invitation to {board.Name} click this link {invitationAcceptUrlPrefix}?InvitationAcceptToken={HttpUtility.HtmlDecode(invitationAcceptToken)}",
                TextBody = "Accept board invitation"

            };
            return mail;
        }

        private string CreateInvitationAcceptToken(string receiverEmail, int boardId)
        {
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
                
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("BoardId",boardId.ToString()));
            claims.Add(new Claim("ReceiverEmail", receiverEmail));
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials credentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken token = securityTokenHandler.CreateJwtSecurityToken(_tokenOptions.Issuer,_tokenOptions.Issuer, new ClaimsIdentity(claims),signingCredentials:credentials);
            return securityTokenHandler.WriteToken(token);
            
        }

       

    }

}