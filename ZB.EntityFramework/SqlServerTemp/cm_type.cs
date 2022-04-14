namespace ZB.EntityFramework.SqlServerTemp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cm_type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Application { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public bool? IsSystem { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }
    }
}
