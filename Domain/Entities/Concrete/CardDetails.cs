using Core.Persistence.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Domain.Entities.Concrete;

public class CardDetails : Entity
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int AssignedPersonId { get; set; }
    public bool IsReassigned { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? SubmitDate { get; set; }
    public bool IsSubmitted { get; set; }
    public virtual Person? Person { get; set; }

    public CardDetails()
    {
        
    }


    public CardDetails(Card card)
    {
        BoardId = card.BoardId;
        CardId = card.Id;
        AssignedPersonId = (int)card.AssignedPersonId!;
        IsReassigned = (bool)card.IsReassigned!;
        AssignedDate = card.AssignedDate;
        DueDate = card.DueDate;
        IsSubmitted = false;
    }



}