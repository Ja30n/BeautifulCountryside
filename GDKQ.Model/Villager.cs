namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Villager")]
    public partial class Villager
    {
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        [Required]
        [StringLength(64)]
        public string RealName { get; set; }

        [StringLength(64)]
        public string Mobile { get; set; }

        public DateTime? CreatTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        [StringLength(64)]
        public string LastLoginIP { get; set; }

        public bool Enabled { get; set; }

        public bool IsDeleted { get; set; }
    }
}
