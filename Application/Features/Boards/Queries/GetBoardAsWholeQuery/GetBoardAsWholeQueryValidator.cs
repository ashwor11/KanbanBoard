using FluentValidation;

namespace Application.Features.Boards.Queries.GetBoardAsWholeCommand;

public class GetBoardAsWholeQueryValidator : AbstractValidator<GetBoardAsWholeQuery>
{
    public GetBoardAsWholeQueryValidator()
    {
        RuleFor(x => x.BoardId).NotEmpty().NotNull();
    }
}