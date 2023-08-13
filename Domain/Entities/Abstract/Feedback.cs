using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Domain.Entities.Abstract;

public abstract class Feedback : Entity
{ 
    public string Content { get; set; }
    public int WrittenByPersonId { get; set; }


    public virtual Person Person { get; set; }

    public Feedback(string content)
    {
        Content = content;
    }

    public Feedback()
    {
        
    }
}