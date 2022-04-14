namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bl_contract
    {
        [Key]
        public int contractId { get; set; }

        public int customerId { get; set; }

        [StringLength(100)]
        public string contractNo { get; set; }

        [Required]
        [StringLength(100)]
        public string contractName { get; set; }

        public decimal contractAmt { get; set; }

        public DateTime signDate { get; set; }

        [StringLength(4000)]
        public string remark { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; }

        public int createUserId { get; set; }

        public DateTime createDate { get; set; }

        public int modifyUserId { get; set; }

        public DateTime modifyDate { get; set; }
    }
}
