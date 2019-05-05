using HotelReservation.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetEntityById(int id);
        void AddEntity(TEntity entity);
        void DeactivateEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        Task<bool> SaveAsync();
    }
}
