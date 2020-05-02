using BYT.WS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Data
{
    public class OzetBeyanDataContext : DbContext
    {
       
     
        public OzetBeyanDataContext(DbContextOptions<BeyannameDataContext> options) : base(options)
        {

        }
        
        public DbSet<ObBeyan> ObBeyan { get; set; }
        public DbSet<ObTeminat> ObTeminat { get; set; }
        public DbSet<ObOzetBeyanAcma> ObOzetBeyanAcma { get; set; }
        public DbSet<ObOzetBeyanAcmaTasimaSenet> ObOzetBeyanAcmaTasimaSenet { get; set; }
        public DbSet<ObOzetBeyanAcmaTasimaSatir> ObOzetBeyanAcmaTasimaSatir { get; set; }
      
        
        public int GetRefIdNextSequenceValue(string Rejim)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            string sequenceName = "RefId" + Rejim;
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR  " + sequenceName + ")", result);

            return (int)result.Value;

        }



    }
}
