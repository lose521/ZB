namespace ZB.EntityFramework.SqlServerTemp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_user
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [StringLength(50)]
        public string loginName { get; set; }

        [Required]
        [StringLength(200)]
        public string password { get; set; }

        [Required]
        [StringLength(10)]
        public string status { get; set; }

        public int createuserid { get; set; }

        public DateTime createdate { get; set; }

        public int modifyuserid { get; set; }

        public DateTime modifydate { get; set; }
    }
}
