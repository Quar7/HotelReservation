using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(HotelReservationDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Hotel)
                .ToListAsync();
        }

        public override async Task<Reservation> GetEntityById(int id)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Hotel)
                .SingleOrDefaultAsync(r => r.Id == id);
        }
    }
}
