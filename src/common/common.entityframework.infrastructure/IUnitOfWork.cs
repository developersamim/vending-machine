using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.entityframework;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
