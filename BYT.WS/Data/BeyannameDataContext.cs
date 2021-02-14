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
    public class BeyannameDataContext : DbContext
    {
       
     
        public BeyannameDataContext(DbContextOptions<BeyannameDataContext> options) : base(options)
        {

        }
        
        public DbSet<DbBeyan> DbBeyan { get; set; }
        public DbSet<DbKalem> DbKalem { get; set; }
        public DbSet<DbFirma> DbFirma { get; set; }
        public DbSet<DbTeminat> DbTeminat { get; set; }
        public DbSet<DbOzetBeyanAcma> DbOzetbeyanAcma { get; set; }
        public DbSet<DbOzetBeyanAcmaTasimaSenet> DbOzetBeyanAcmaTasimaSenet { get; set; }
        public DbSet<DbOzetBeyanAcmaTasimaSatir> DbOzetBeyanAcmaTasimaSatir { get; set; }
        public DbSet<DbKiymetBildirim> DbKiymetBildirim { get; set; }
        public DbSet<DbKiymetBildirimKalem> DbKiymetBildirimKalem { get; set; }
        public DbSet<DbMarka> DbMarka { get; set; }
        public DbSet<DbOdemeSekli> DbOdemeSekli { get; set; }
        public DbSet<DbKonteyner> DbKonteyner { get; set; }
        public DbSet<DbBeyannameAcma> DbBeyannameAcma { get; set; }
        public DbSet<DbTamamlayiciBilgi> DbTamamlayiciBilgi { get; set; }
        public DbSet<DbVergi> DbVergi { get; set; }
        public DbSet<DbBelge> DbBelge { get; set; }
        public DbSet<DbSoruCevap> DbSoruCevap { get; set; }
        public DbSet<DbIghb> DbIghb { get; set; }
        public DbSet<DbIghbListe> DbIghbListe { get; set; }
        public DbSet<Mesai> Mesai { get; set; }
        public DbSet<Dolasim> Dolasim { get; set; }
        
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

        public int GetMesaiIdNextSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            string sequenceName = "RefIdMesai";
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR  " + sequenceName + ")", result);

            return (int)result.Value;

        }
        public int GetIghbIdNextSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            string sequenceName = "RefIdIghb";
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR  " + sequenceName + ")", result);

            return (int)result.Value;

        }

        public int GetDolasimIdNextSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            string sequenceName = "RefIdDolasim";
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR  " + sequenceName + ")", result);

            return (int)result.Value;

        }

    }
}
