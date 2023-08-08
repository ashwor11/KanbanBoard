using Core.Persistence.Repositories;

namespace Domain.Entities.Concrete;

public class PersonBoard : Entity
{
    public int PersonId { get; set; }
    public int BoardId { get; set; }


    public virtual Person Person { get; set; }
    public virtual Board Board { get; set; }


}