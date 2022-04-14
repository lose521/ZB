namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cm_status
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusCode { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }
    }
}
