using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace the_law.Models
{
    public partial class LawModel : DbContext
    {
        public LawModel()
            : base("name=LawsModel")
        {
        }

        public virtual DbSet<law> laws { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<law>()
                .Property(e => e.dieu)
                .IsFixedLength();

            modelBuilder.Entity<law>()
                .Property(e => e.noidung)
                .IsUnicode(false);
        }
    }
}
