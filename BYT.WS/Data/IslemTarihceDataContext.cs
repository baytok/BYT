using BYT.WS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Data
{
    public class IslemTarihceDataContext : DbContext
    {
        public IslemTarihceDataContext(DbContextOptions<IslemTarihceDataContext> options) : base(options)
        {

        }
        public DbSet<Islem> Islem { get; set; }
        public DbSet<Tarihce> Tarihce { get; set; }
      
    }
}
