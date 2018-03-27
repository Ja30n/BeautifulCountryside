namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Article")]
    public partial class Article
    {
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public int AuthorID { get; set; }

        [Required]
        [StringLength(64)]
        public string CategoryID { get; set; }

        public DateTime CreateTime { get; set; }

        public int VisitNum { get; set; }

        public string Photo { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime ModTime { get; set; }

        [Required]
        public string lab { get; set; }

        [Required]
        [StringLength(64)]
        public string CaName { get; set; }

        [StringLength(32)]
        public string AuthorName { get; set; }

        public int like_count { get; set; }

        public int Comments { get; set; }

        public bool Enable { get; set; }
    }
}
