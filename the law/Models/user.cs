namespace the_law.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        [Key]
        [StringLength(10)]
        public string mssv { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(10)]
        public string password { get; set; }
    }
}
