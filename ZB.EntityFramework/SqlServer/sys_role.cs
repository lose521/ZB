namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(250)]
        public string RoleDesc { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int ModifyUserId { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
