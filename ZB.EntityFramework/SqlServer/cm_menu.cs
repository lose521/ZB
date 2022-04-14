namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cm_menu
    {
        [Key]
        public int MnuId { get; set; }

        [Required]
        [StringLength(50)]
        public string AppCode { get; set; }

        public int MnuOrder { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuName { get; set; }

        [StringLength(50)]
        public string MnuUrl { get; set; }

        [StringLength(50)]
        public string MnuPic { get; set; }

        [StringLength(50)]
        public string MnuParentNo { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }
    }
}
