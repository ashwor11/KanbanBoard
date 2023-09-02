using FluentValidation;

namespace Application.Features.Boards.Queries.GetBoardAsWholeCommand;

public class GetBoardAsWholeCommandValidator : AbstractValidator<GetBoardAsWholeQuery>
{
    public GetBoardAsWholeCommandValidator()
    {
        RuleFor(x => x.BoardId).NotEmpty().NotNull();
    }
}