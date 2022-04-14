namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFContext : DbContext
    {
        public EFContext()
            : base("name=EFContext")
        {
        }
        public virtual DbSet<bl_invoice> bl_invoice { get; set; }
        public virtual DbSet<bl_contract> bl_contract { get; set; }
        public virtual DbSet<bl_customer> bl_customer { get; set; }
        public virtual DbSet<sys_user> sys_user { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
