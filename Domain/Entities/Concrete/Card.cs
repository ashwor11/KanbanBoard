using Core.Persistence.Repositories;
using Domain.Entities.Enums;

namespace Domain.Entities.Concrete;

public class Card : Entity
{
    public string? Name { get; set; }
    public int? AssignedPersonId { get; set; }
    public bool? IsReassigned { get; set; }
    public CardStatus Status { get; set; }
    public int BoardId { get; set; }
    public Color Color { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public virtual List<CardFeedback> Feedbacks{ get; set; }
    public virtual List<Job> Jobs { get; set; }
    public virtual Person AssignedPerson { get; set; }
    public virtual Board Board{ get; set; }
    public Card()
    {
        Status = CardStatus.Backlog;
        Color = Color.Grey;
        Feedbacks = new List<CardFeedback>();
        Jobs = new List<Job>();
    }


    public void ChangeCardName( string cardName)
    {
       Name = cardName;

    }

    public void AssignCardToPerson( int personId)
    {
        
        AssignedPersonId = personId;
        IsReassigned = false;
        AssignedDate = DateTime.Now;
        PullCardToToDo();


    }

    public void ReAssignCardToPerson(int personId)
    {
        
        AssignedPersonId = personId;
        AssignedDate = DateTime.Now;
        IsReassigned = true;
    }

    public void AssignDueDate(DateTime date)
    {
        DueDate = date;
    }

    public void RemoveAssignedPerson()
    {
       AssignedPersonId = null;
       AssignedDate = null;
    }

    public void RemoveDueDate()
    {
        DueDate = null;
    }

    public void PullCardToBacklog()
    {
        Status = CardStatus.Backlog;

    }

    public void PullCardToToDo()
    {
        Status = CardStatus.ToDo;
    }

    public void PullCardToInProgress()
    {
        Status = CardStatus.InProgress;
    }

    public void PullCardToReview()
    {
        Status = CardStatus.Review;


    }

    public void PullCardToDone()
    {
        Status = CardStatus.Done;
        FinishDate = DateTime.Now;


    }


    public void AddJob()
    {
        Jobs.Add(new Job());
    }

    
    public void RemoveJob(int jobId)
    {
        Job job = Jobs.First(x => x.Id == jobId);
        job = null;
    }

    public void AddFeedback(int writtenByPersonId, string content)
    {
        Feedbacks.Add(new CardFeedback(writtenByPersonId,content));
    }

    public void UpdateFeedback(int feedbackId, string content)
    {
        Feedbacks.First(x => x.Id == feedbackId).Content = content;
    }

    public void DeleteFeedback(int feedbackId)
    {
        CardFeedback feedback = Feedbacks.First(x => x.Id == feedbackId);
        feedback = null;
    }

}