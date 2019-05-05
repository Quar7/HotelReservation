using HotelReservation.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task DecreaseAvailableRoomQuantity(int id);
        Task IncreaseAvailableRoomQuantity(int id);
    }
}
