using Core.Security.Entities;

namespace Domain.Entities.Concrete;

public class Person : User
{
    public virtual HashSet<PersonBoard> PersonBoards { get; set; }
    public virtual HashSet<Card> AssignedCards { get; set; }

    public Person() : base()
    {
        PersonBoards = new HashSet<PersonBoard>();
        AssignedCards = new HashSet<Card>();
    }

}