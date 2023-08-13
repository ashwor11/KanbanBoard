using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories
{
    public interface IJobRepository : IRepository<Job>, IAsyncRepository<Job>
    {
    }
}
