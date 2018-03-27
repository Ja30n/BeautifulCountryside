namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int ID { get; set; }

        [StringLength(64)]
        public string Title { get; set; }

        public string Body { get; set; }

        [StringLength(64)]
        public string Author { get; set; }

        [Required]
        [StringLength(64)]
        public string bh { get; set; }

        public int VisitNum { get; set; }

        public DateTime CreateTime { get; set; }

        public string Photo { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(64)]
        public string CaName { get; set; }

        public string Url { get; set; }
    }
}
