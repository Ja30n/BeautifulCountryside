namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Photo")]
    public partial class Photo
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string author { get; set; }

        [StringLength(10)]
        public string place { get; set; }

        public int like_count { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(64)]
        public string Title { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }
    }
}
