using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.entityframework;

public class UnitOfWork<T> : IUnitOfWork where T : DbContext
{
    private readonly T dbContext;

    public UnitOfWork(T dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
