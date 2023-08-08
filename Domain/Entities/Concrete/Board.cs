using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Domain.Entities.Concrete;

public class Board : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorUserId { get; set; }
    public virtual HashSet<PersonBoard>? PersonBoards { get; set; }
    public virtual HashSet<Card>? Cards{ get; set; }

    public Board()
    {
        PersonBoards = new HashSet<PersonBoard>();
        Cards = new HashSet<Card>();
    }
}