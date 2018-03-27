namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        public string Photo { get; set; }

        public DateTime? CreatTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        [StringLength(64)]
        public string LastLoginIP { get; set; }
    }
}
