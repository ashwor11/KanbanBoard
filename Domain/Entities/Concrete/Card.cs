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


    public void AddJob()
    {
        Jobs.Add(new Job());
    }

    public void ChangeDescriptionOfJob(int jobId, string description)
    {
        Jobs.First(x=>x.Id == jobId).JobDescription = description;
    }
    public void MarkJobAsDone(int jobId, string description)
    {
        Jobs.First(x => x.Id == jobId).IsDone = true;
    }
    public void MarkJobAsUnDone(int jobId, string description)
    {
        Jobs.First(x => x.Id == jobId).IsDone = false;
    }
    public void RemoveJob(int jobId)
    {
        Job job = Jobs.First(x => x.Id == jobId);
        job = null;
    }

    public void AddFeedback()
    {
        Feedbacks.Add(new CardFeedback());
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