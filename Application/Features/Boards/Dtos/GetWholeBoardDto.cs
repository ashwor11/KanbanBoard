using Domain.Entities.Enums;

namespace Application.Features.Boards.Dtos;

public class GetWholeBoardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorUserId { get; set; }
    public virtual List<GetPersonsForBoardDto>? Persons { get; set; }
    public virtual List<GetCardDto>? Cards { get; set; }
}

public class GetCardDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? AssignedPersonId { get; set; }
    public bool? IsReassigned { get; set; }
    public CardStatus Status { get; set; }
    public int BoardId { get; set; }
    public Color Color { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public virtual List<GetCardFeedbackDto> Feedbacks{ get; set; }
    public virtual List<GetJobDto> Jobs { get; set; }

}

public class GetJobDto
{
    public int Id { get; set; }
    public string JobDescription { get; set; }
    public bool IsDone { get; set; }
    public virtual List<GetJobFeedbackDto> Feedbacks { get; set; }

}

public class GetCardFeedbackDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int WrittenByPersonId { get; set; }
}

public class GetJobFeedbackDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int WrittenByPersonId { get; set; }
}

public class GetPersonsForBoardDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}