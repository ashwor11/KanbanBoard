using Core.Persistence.Repositories;
using Core.Security.Entities;
using Domain.Entities.Enums;

namespace Domain.Entities.Concrete;

public class Board : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CreatorUserId { get; set; }
    public virtual List<PersonBoard>? PersonBoards { get; set; }
    public virtual List<Card>? Cards{ get; set; }
    
    public Board()
    {
        PersonBoards = new List<PersonBoard>();
        Cards = new List<Card>();
    }

    

   

    
}