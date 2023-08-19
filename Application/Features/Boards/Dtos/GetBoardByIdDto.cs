using Domain.Entities.Enums;

namespace Application.Features.Boards.Dtos;

public class GetBoardByIdDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CreatorId { get; set; }
    public virtual List<GetPersonsForBoardDto>? Persons { get; set; }
    public Column? Backlog { get; set; }
    public Column? ToDo { get; set; }
    public Column? InProgress { get; set; }
    public Column? Review { get; set; }
    public Column? Done { get; set; }

}

public class Column
{
    public string Id { get; set; }
    public List<GetCardDto>? Cards { get; set; }

    public Column()
    {
        Cards = new();
    }
}


