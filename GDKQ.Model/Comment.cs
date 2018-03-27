namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(32)]
        public string UserName { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        public string Body { get; set; }

        public int ArtcleID { get; set; }

        public string Photo { get; set; }
    }
}
