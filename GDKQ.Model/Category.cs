namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string CaName { get; set; }

        [Required]
        [StringLength(64)]
        public string bh { get; set; }

        [StringLength(64)]
        public string pbh { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
