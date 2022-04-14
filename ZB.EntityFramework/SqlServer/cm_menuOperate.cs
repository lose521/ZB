namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cm_menuOperate
    {
        [Key]
        public int MnuOperateId { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuModuleNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuOperateNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuOperateName { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuScript { get; set; }

        public bool IsAccess { get; set; }

        [StringLength(50)]
        public string ImageName { get; set; }

        public int MnuOperateOrder { get; set; }

        [Required]
        [StringLength(50)]
        public string MnuOperatePosition { get; set; }
    }
}
