using Core.Persistence.Repositories;

namespace Domain.Entities.Concrete;

public class CardDetails : Entity
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public int AssignedPersonId { get; set; }
    public bool IsReassigned { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? SubmitDate { get; set; }

    public virtual Card Card { get; set; }
    public virtual Person Person { get; set; }



}