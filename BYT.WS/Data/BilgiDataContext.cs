using BYT.WS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Data
{
    public class BilgiDataContext : DbContext
    {
        public BilgiDataContext(DbContextOptions<BilgiDataContext> options) : base(options)
        {

        }
        public DbSet<Islem> Islem { get; set; }
        public DbSet<Tarihce> Tarihce { get; set; }
        public DbSet<DbBeyan> DbBeyan { get; set; }
        public DbSet<ObBeyanAlan> ObBeyanAlan { get; set; }
        public int GetRefIdNextSequenceValue(string Rejim)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            string sequenceName = "RefId"+ Rejim;
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR  "+ sequenceName+")", result);

            return (int)result.Value;

        }
    }
}
