using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Boards.Commands.InvitePersonToBoardCommand
{
    public class InvitePersonToBoardCommandValidator : AbstractValidator<InvitePersonToBoardCommand>
    {
        public InvitePersonToBoardCommandValidator()
        {
            RuleFor(x=>x.InvitationAcceptUrlPrefix).NotEmpty().NotNull();
            RuleFor(x=>x.InvitePersonToBoardDto.BoardId).NotEmpty().NotNull();
            RuleFor(x=>x.InvitePersonToBoardDto.InvitedPersonEmail).NotEmpty().NotNull().EmailAddress();
            
        }
    }
}
