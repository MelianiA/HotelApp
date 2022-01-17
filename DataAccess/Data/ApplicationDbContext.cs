﻿using DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public  class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
    }
}
