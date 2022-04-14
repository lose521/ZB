namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bl_customer
    {
        [Key]
        public int customerId { get; set; }

        [StringLength(100)]
        public string customerNo { get; set; }

        [Required]
        [StringLength(100)]
        public string customerName { get; set; }

        [Required]
        [StringLength(100)]
        public string telephone { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(10)]
        public string sex { get; set; }

        [StringLength(100)]
        public string address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthday { get; set; }

        [StringLength(4000)]
        public string remark { get; set; }

        [Required]
        [StringLength(10)]
        public string status { get; set; }

        public int createUserId { get; set; }

        public DateTime createDate { get; set; }

        public int modifyUserId { get; set; }

        public DateTime modifyDate { get; set; }
    }
}
