using MediatR;

namespace Application.Features.Boards.Commands.ChangeCardNameCommand;

public class ChangeCardNameCommand : IRequest
{
    public int Id { get; set; }
}