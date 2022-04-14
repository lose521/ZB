namespace ZB.EntityFramework.SqlServerTemp
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

        [Required]
        [StringLength(100)]
        public string customerNo { get; set; }

        [Required]
        [StringLength(100)]
        public string customerName { get; set; }

        [Required]
        [StringLength(100)]
        public string telephone { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(1)]
        public string sex { get; set; }

        [Required]
        [StringLength(100)]
        public string address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthday { get; set; }

        [StringLength(4000)]
        public string remark { get; set; }

        [Required]
        [StringLength(10)]
        public string recordstatus { get; set; }

        public int createuserid { get; set; }

        public DateTime createdate { get; set; }

        public int modifyuserid { get; set; }

        public DateTime modifydate { get; set; }
    }
}
