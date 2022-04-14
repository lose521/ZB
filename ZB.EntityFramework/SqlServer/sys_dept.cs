namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_dept
    {
        [Key]
        public int DeptId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [StringLength(250)]
        public string DeptName { get; set; }

        [StringLength(50)]
        public string DeptNo { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int ModifyUserId { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
