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
        public DbSet<NbKalem> NbKalem { get; set; }
        public DbSet<NbMuhur> NbMuhur { get; set; }
        public DbSet<NbRota> NbRota { get; set; }
        public DbSet<NbTransitGumruk> NbTransitGumruk { get; set; }
        public DbSet<NbTeminat> NbTeminat { get; set; }
        public DbSet<NbKalemGuvenliGondericiFirma> NbKalemGuvenliGondericiFirma { get; set; }
        public DbSet<NbKalemGuvenliAliciFirma> NbKalemGuvenliAliciFirma { get; set; }
        public DbSet<NbKalemGondericiFirma> NbKalemGondericiFirma { get; set; }
        public DbSet<NbKalemAliciFirma> NbKalemAliciFirma { get; set; }
        public DbSet<NbKonteyner> NbKonteyner { get; set; }
        public DbSet<NbHassasEsya> NbHassasEsya { get; set; }
        public DbSet<NbEkBilgi> NbEkBilgi { get; set; }
        public DbSet<NbBelgeler> NbBelgeler { get; set; }
        public DbSet<NbOncekiBelgeler> NbOncekiBelgeler { get; set; }
        public DbSet<NbObAcma> NbObAcma { get; set; }
        public DbSet<NbAbAcma> NbAbAcma { get; set; }


        public DbSet<NbKap> NbKap { get; set; }
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
