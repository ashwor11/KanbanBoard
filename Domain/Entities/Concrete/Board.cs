using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Domain.Entities.Enums;
using Org.BouncyCastle.Math.EC.Rfc7748;

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


    public void RemovePersonFromBoard(int toRemovePersonId)
    {
        IfPersonToRemoveIsCreator(toRemovePersonId);
        PersonBoards.RemoveAll(x => x.PersonId == toRemovePersonId);
    }

    private void IfPersonToRemoveIsCreator(int toRemovePersonId)
    {
        if (CreatorUserId == toRemovePersonId)
        {
            throw new BusinessException("Creator of the board cannot be removed");
        }
    }
}