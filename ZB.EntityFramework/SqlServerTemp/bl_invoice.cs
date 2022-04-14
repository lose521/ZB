namespace ZB.EntityFramework.SqlServerTemp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bl_invoice
    {
        [Key]
        public int invoiceId { get; set; }

        public int contractId { get; set; }

        [Required]
        [StringLength(100)]
        public string invoiceName { get; set; }

        [StringLength(100)]
        public string invoiceNo { get; set; }

        [StringLength(500)]
        public string imageUrl { get; set; }

        [Column(TypeName = "date")]
        public DateTime makeDay { get; set; }

        public decimal invoiceAmt { get; set; }

        public decimal invoiceTaxAmt { get; set; }

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
