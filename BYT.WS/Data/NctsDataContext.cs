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
    public class NctsDataContext : DbContext
    {
       
     
        public NctsDataContext(DbContextOptions<NctsDataContext> options) : base(options)
        {

        }
        
        public DbSet<NbBeyan> NbBeyan { get; set; }
        public DbSet<NbBeyanSahibi> NbBeyanSahibi { get; set; }
        public DbSet<NbTasiyiciFirma> NbTasiyiciFirma { get; set; }
        public DbSet<NbAliciFirma> NbAliciFirma { get; set; }
        public DbSet<NbGondericiFirma> NbGondericiFirma { get; set; }
        public DbSet<NbAsilSorumluFirma> NbAsilSorumluFirma { get; set; }
        public DbSet<NbGuvenliAliciFirma> NbGuvenliAliciFirma { get; set; }
        public DbSet<NbGuvenliGondericiFirma> NbGuvenliGondericiFirma { get; set; }

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
