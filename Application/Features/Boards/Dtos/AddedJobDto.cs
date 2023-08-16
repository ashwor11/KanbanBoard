namespace Application.Features.Boards.Dtos;

public class AddedJobDto
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public string JobDescription { get; set; }
    public bool IsDone { get; set; }
}