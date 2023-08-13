using Core.Persistence.Repositories;
using Core.Security.Entities;
using Domain.Entities.Enums;

namespace Domain.Entities.Concrete;

public class Board : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorUserId { get; set; }
    public virtual List<PersonBoard>? PersonBoards { get; set; }
    public virtual List<Card>? Cards{ get; set; }
    public virtual List<CardDetails>? CardDetails { get; set; }

    public Board()
    {
        PersonBoards = new List<PersonBoard>();
        Cards = new List<Card>();
        CardDetails = new List<CardDetails>();
    }

    public void ChangeCardName(int cardId, string cardName)
    {
       Cards.FirstOrDefault(c => c.Id == cardId).Name = cardName;
       
    }

    public void AssignCardToPerson(int cardId, int personId)
    { 
        Card card = Cards.FirstOrDefault(x => x.Id == cardId);
        card.AssignedPersonId = personId;
        card.IsReassigned = false;
        card.AssignedDate = DateTime.Now;
        PullCardToToDo(cardId);

        CardDetails cardDetails = new(card);
        this.CardDetails.Add(cardDetails);
    }

    public void ReAssignCardToPerson(int cardId, int personId)
    {
        Card card = Cards.FirstOrDefault(x => x.Id == cardId);
        card.AssignedPersonId = personId;
        card.AssignedDate = DateTime.Now;
        card.IsReassigned = true;

        CardDetails cardDetail = CardDetails.FirstOrDefault(x => x.CardId == cardId && x.IsSubmitted == false);
        cardDetail.IsSubmitted = true;
        cardDetail.SubmitDate = DateTime.Now;

        CardDetails cardDetails = new(card);
        this.CardDetails.Add(cardDetails);

    }

    public void AssignDueDate(int cardId, DateTime date)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).DueDate = date;
    }

    public void RemoveAssignedPerson(int cardId)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).AssignedPersonId = null;
        CardDetails cardDetail = CardDetails.First(x => x.CardId == cardId && x.IsSubmitted == false);
        if (cardDetail != null)
            cardDetail = null;
    }

    public void RemoveDueDate(int cardId)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).DueDate = null;
    }

    public void PullCardToBacklog(int cardId)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).Status = CardStatus.Backlog;

    }

    public void PullCardToToDo(int cardId)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).Status = CardStatus.ToDo;
    }

    public void PullCardToInProgress(int cardId)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).Status = CardStatus.InProgress;
    }

    public void PullCardToReview(int cardId)
    {
        Cards.FirstOrDefault(x => x.Id == cardId).Status = CardStatus.Review;
        CardDetails cardDetail = CardDetails.FirstOrDefault(x => x.CardId == cardId && x.IsSubmitted == false);
            cardDetail.IsSubmitted = true;
            cardDetail.SubmitDate = DateTime.Now;

    }

    public void PullCardToDone(int cardId)
    {
        Card card = Cards.FirstOrDefault(x => x.Id == cardId);
        card.Status = CardStatus.Done;
        card.FinishDate = DateTime.Now;


    }

    
}