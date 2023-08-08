using Core.Persistence.Repositories;
using Domain.Entities.Enums;

namespace Domain.Entities.Concrete;

public class Card : Entity
{
    public string Name { get; set; }
    public int AssignedPersonId { get; set; }
    public bool IsReassigned { get; set; } = false;
    public CardStatus Status { get; set; }
    public int BoardId { get; set; }
    public Color Color { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime FinishDate { get; set; }
    public virtual HashSet<CardFeedback> Feedbacks{ get; set; }
    public virtual HashSet<Job> Jobs { get; set; }
    public virtual Person AssignedPerson { get; set; }
    public virtual Board Board{ get; set; }
    public Card()
    {
        Feedbacks = new HashSet<CardFeedback>();
        Jobs = new HashSet<Job>();
    }
    
}