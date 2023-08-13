using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Application.Services.Abstract;
using Core.Application.Pipelines.Validation;
using Core.Security.Encryption.Helpers;
using Core.Security.JWT;
using Domain.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Boards.Commands.AcceptBoardInvitationCommand;

public class AcceptBoardInvitationCommand : IRequest<string>, IValidationRequest
{
    public string InvitationToken { get; set; }
    public int PersonId { get; set; }
    

    public class AcceptBoardInvitationCommandHandler : IRequestHandler<AcceptBoardInvitationCommand,string>
    {
        private readonly TokenOptions _tokenOptions;
        private readonly IPersonService _personService;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IPersonBoardRepository _personBoardRepository;

        public AcceptBoardInvitationCommandHandler(IOptions<TokenOptions> tokenOptions, IPersonService personService, BoardBusinessRules boardBusinessRules, IPersonBoardRepository personBoardRepository)
        {
            _tokenOptions = tokenOptions.Value;
            _personService = personService;
            _boardBusinessRules = boardBusinessRules;
            _personBoardRepository = personBoardRepository;
        }

        public async Task<string> Handle(AcceptBoardInvitationCommand request, CancellationToken cancellationToken)
        {
            (int boardId, string email) = ValidateToken(request.InvitationToken);
            Person person = await _personService.GetPersonWithEmail(email);
            _boardBusinessRules.DoPersonIdAndEmailDirectSamePerson(person, request.PersonId, email);

            PersonBoard personBoard = new() { BoardId = boardId, PersonId = person.Id };

            await _personBoardRepository.CreateAsync(personBoard);

            return "Successfully entered the board.";


        }

        private (int boardId, string personEmail) ValidateToken(string token)
        {
            token = HttpUtility.HtmlDecode(token);
            SecurityToken securityToken;   
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();
            ClaimsPrincipal claims = securityTokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            if (!claims.HasClaim(x => x.Type == "BoardId") || !claims.HasClaim(x => x.Type == "ReceiverEmail"))
                throw new BadHttpRequestException("Not valid token");

            return (Convert.ToInt32(claims.Claims.FirstOrDefault(x => x.Type == "BoardId").Value),
                claims.Claims.FirstOrDefault(x => x.Type == "ReceiverEmail").Value);

        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenOptions.Issuer,
                ValidAudience = _tokenOptions.Audience,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey)
            };
        }
    }
}