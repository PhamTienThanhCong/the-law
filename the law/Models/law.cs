namespace the_law.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class law
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(10)]
        public string dieu { get; set; }

        [Column(TypeName = "ntext")]
        public string ten { get; set; }

        [Column(TypeName = "ntext")]
        public string noidung { get; set; }
    }
}
