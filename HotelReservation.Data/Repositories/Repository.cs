using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories.Interfaces
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected HotelReservationDbContext _context;

        public Repository(HotelReservationDbContext context)
        {
            _context = context;
        }

        public virtual void AddEntity(TEntity entity)
        {
            entity.IsActive = true;
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void DeactivateEntity(TEntity entity)
        {
            entity.IsActive = false;
        }

        public virtual async Task<TEntity> GetEntityById(int id)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual void UpdateEntity(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
