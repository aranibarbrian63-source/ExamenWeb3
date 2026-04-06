using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Examenwed3.Models;

namespace Examenwed3.Data
{
    public class Examenwed3Context : DbContext
    {
        public Examenwed3Context (DbContextOptions<Examenwed3Context> options)
            : base(options)
        {
        }

        public DbSet<Examenwed3.Models.Hotel> Hotel { get; set; } = default!;
        public DbSet<Examenwed3.Models.Reserva> Reserva { get; set; } = default!;
    }
}
