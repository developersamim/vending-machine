using common.entityframework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.domain;

namespace user.application.Contracts.Persistence;

public interface IUserRepository : IAsyncRepository<ApplicationUser>
{
}
