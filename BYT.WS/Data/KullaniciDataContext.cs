﻿using BYT.WS.Entities;
using BYT.WS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Data
{
    public class KullaniciDataContext : DbContext
    {
        public KullaniciDataContext(DbContextOptions<KullaniciDataContext> options) : base(options)
        {

        }
        public DbSet<Kullanici> Kullanici { get; set; }
        public DbSet<Musteri> Musteri { get; set; }
        public DbSet<Firma> Firma { get; set; }

        public DbSet<Yetki> Yetki { get; set; }

        public DbSet<KullaniciYetki> KullaniciYetki { get; set; }
        
    }
}
