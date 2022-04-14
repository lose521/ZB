namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_company
    {
        [Key]
        public int CompanyId { get; set; }
        public int? ParentId { get; set; }

        [Required]
        [StringLength(250)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string CompanyNo { get; set; }

        [StringLength(50)]
        public string CompanyGoup { get; set; }
        [StringLength(500)]
        public string Remark { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Tel { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int ModifyUserId { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
