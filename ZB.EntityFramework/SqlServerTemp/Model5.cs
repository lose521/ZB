using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ZB.EntityFramework.SqlServerTemp
{
    public partial class Model5 : DbContext
    {
        public Model5()
            : base("name=Model51")
        {
        }

        public virtual DbSet<bl_invoice> bl_invoice { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bl_invoice>()
                .Property(e => e.invoiceAmt)
                .HasPrecision(18, 0);

            modelBuilder.Entity<bl_invoice>()
                .Property(e => e.invoiceTaxAmt)
                .HasPrecision(18, 0);
        }
    }
}
