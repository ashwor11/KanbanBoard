using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using Application.Features.Auth.Dtos;
using Application.Repositories;
using Core.Security.Entities;
using Core.Security.JWT;
using Domain.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Auth.Commands.RefreshCommand;

public class RefreshCommand : IRequest<RefreshedTokenDto>
{
   public RefreshTokenDto RefreshTokenDto { get; set; }
   public string IpAddress { get; set; }

    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshedTokenDto>
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPersonRepository _personRepository;

        public RefreshCommandHandler(ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository, IPersonRepository personRepository)
        {
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _personRepository = personRepository;
        }

        public async Task<RefreshedTokenDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            //if token expired create and refresh token matches the refresh token on db return new access token

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(request.RefreshTokenDto.Token);
            
            if (jwtSecurityToken.ValidTo > DateTime.UtcNow)
                throw new Exception("Token is not expired yet");

            RefreshToken refreshToken = await _refreshTokenRepository.GetAsync(x => x.Token == request.RefreshTokenDto.RefreshToken);

            IsRefreshTokenRevoked(refreshToken);

           
            DoesRefreshTokenExists(refreshToken);

            IsRefreshTokenExpired(refreshToken);
            Person person = await _personRepository.GetAsync(x => x.Id == refreshToken.UserId, include: x=>x.Include(x=>x.UserOperationClaims).ThenInclude(x=>x.OperationClaim));
            IList<OperationClaim> operationClaims = person.UserOperationClaims.Select(x => x.OperationClaim).ToList();
            refreshToken.Revoke(revokedByIp:request.IpAddress);
            await _refreshTokenRepository.UpdateAsync(refreshToken);

            RefreshToken createdRefreshToken = _tokenHelper.CreateRefreshToken(person, request.IpAddress);
            await _refreshTokenRepository.CreateAsync(createdRefreshToken);

            AccessToken accessToken = _tokenHelper.CreateToken(person, operationClaims);

            return new RefreshedTokenDto() { AccessToken = accessToken, RefreshToken = createdRefreshToken.Token };





        }

        private static void IsRefreshTokenExpired(RefreshToken refreshToken)
        {
            if (refreshToken.ExpireDate < DateTime.UtcNow)
                throw new Exception("Refresh token is expired");
        }

        private static void DoesRefreshTokenExists(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new Exception("Refresh token not found");
        }

        private static void IsRefreshTokenRevoked(RefreshToken refreshToken)
        {
            if (refreshToken.Revoked != refreshToken.CreatedDate )
                throw new Exception("Refresh token is revoked");
        }
    }   

}