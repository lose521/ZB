namespace ZB.EntityFramework.SqlServerTemp
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model19")
        {
        }

        public virtual DbSet<cm_type> cm_type { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cm_type>()
                .Property(e => e.Application)
                .IsUnicode(false);

            modelBuilder.Entity<cm_type>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<cm_type>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<cm_type>()
                .Property(e => e.Status)
                .IsUnicode(false);
        }
    }
}
