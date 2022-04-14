namespace ZB.EntityFramework.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int DeptId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeName { get; set; }

        [StringLength(50)]
        public string EmployeeNo { get; set; }

        [StringLength(50)]
        public string Tel { get; set; }

        [StringLength(1)]
        public string Sex { get; set; }

        public DateTime? Birthday { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Photo { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
