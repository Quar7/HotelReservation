using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(HotelReservationDbContext context) : base(context)
        {

        }

        public async Task DecreaseAvailableRoomQuantity(int id)
        {
            var hotel = await GetEntityById(id);
            hotel.AvailableRooms--;
        }

        public async Task IncreaseAvailableRoomQuantity(int id)
        {
            var hotel = await GetEntityById(id);
            hotel.AvailableRooms++;
        }
    }
}

