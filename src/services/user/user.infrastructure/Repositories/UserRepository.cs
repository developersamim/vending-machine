using common.entityframework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.application.Contracts.Persistence;
using user.domain;
using user.infrastructure.Persistence;

namespace user.infrastructure.Repositories;

public class UserRepository : BaseRepository<ApplicationUser, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
