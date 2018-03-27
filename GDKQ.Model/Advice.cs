namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Advice")]
    public partial class Advice
    {
        public int ID { get; set; }

        public string Body { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsRead { get; set; }

        [StringLength(64)]
        public string From_Name { get; set; }
    }
}
