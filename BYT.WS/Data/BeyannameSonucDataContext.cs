using BYT.WS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Data
{
    public class BeyannameSonucDataContext : DbContext
    {
        public BeyannameSonucDataContext(DbContextOptions<BeyannameSonucDataContext> options) : base(options)
        {

        }

       
        public DbSet<DbSonucHatalar> DbSonucHatalar { get; set; }
        public DbSet<DbSonucSorular> DbSonucSorular { get; set; }
        public DbSet<DbSonucSoruCevaplar> DbSonucSoruCevaplar { get; set; }
        public DbSet<DbSonucBelgeler> DbSonucBelgeler { get; set; }
        public DbSet<DbSonucVergiler> DbSonucVergiler { get; set; }
        public DbSet<DbSonucToplamVergiler> DbSonucToplamVergiler { get; set; }
        public DbSet<DbSonucToplananVergiler> DbSonucToplananVergiler { get; set; }
        public DbSet<DbSonucHesapDetaylar> DbSonucHesapDetaylar { get; set; }
        public DbSet<DbSonucGumrukKiymeti> DbSonucGumrukKiymeti { get; set; }
        public DbSet<DbSonucIstatistikiKiymet> DbSonucIstatistikiKiymet { get; set; }
        public DbSet<DbSonucOzetBeyan> DbSonucOzetBeyan { get; set; }
        public DbSet<DbSonucDigerBilgiler> DbSonucDigerBilgiler { get; set; }

        public DbSet<DbVergi> DbVergi { get; set; }
        public DbSet<DbBelge> DbBelge { get; set; }
        public DbSet<DbSoruCevap> DbSoruCevap { get; set; }





    }
}
