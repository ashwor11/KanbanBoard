using System.Text.RegularExpressions;
using AutoMapper.Configuration;
using FluentValidation;

namespace Application.Features.Boards.Commands.ChangeColorOfCardCommand;

public class ChangeCardColorCommandValidator : AbstractValidator<ChangeCardColorCommand>
{
    public ChangeCardColorCommandValidator()
    {
        RuleFor(x => x.ChangeCardColorDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardColorDto.CardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardColorDto.ColorInHex).NotEmpty().NotNull().Must(IsStringHexColor);


    }

    private bool IsStringHexColor(string hexColor)
    {
        string pattern = @"^#(?:[0-9a-fA-F]{3}){1,2}$";

        Regex rg = new(pattern);

        return rg.IsMatch(hexColor);
    }
}