namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cm_menuModule
    {
        [Key]
        public int MnuModuleId { get; set; }

        [StringLength(50)]
        public string MnuNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuModuleNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuModeuleName { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }
    }
}
