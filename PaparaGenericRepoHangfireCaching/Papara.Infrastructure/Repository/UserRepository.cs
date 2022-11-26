
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Entites;
using Papara.Core.Entity;
using Papara.Core.Enums;
using Papara.Core.Interfaces;
using System;

namespace Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DbSet<User> _user;

        public UserRepository(ApplicationDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
            _user = dbContext.Set<User>();
        }
    }
}
