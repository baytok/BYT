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
       
     
        public OzetBeyanDataContext(DbContextOptions<OzetBeyanDataContext> options) : base(options)
        {

        }
        
        public DbSet<ObBeyan> ObBeyan { get; set; }
        public DbSet<ObTasitUgrakUlke> ObTasitUgrakUlke { get; set; }
        public DbSet<ObTasiyiciFirma> ObTasiyiciFirma { get; set; }
        public DbSet<ObTasimaSenet> ObTasimaSenet { get; set; }
        public DbSet<ObIhracat> ObIhracat { get; set; }
        public DbSet<ObUgrakUlke> ObUgrakUlke { get; set; }
        public DbSet<ObTasimaSatir> ObTasimaSatir { get; set; }
        public DbSet<ObSatirEsya> ObSatirEsya { get; set; }
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
