namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mail")]
    public partial class Mail
    {
        public int ID { get; set; }

        public int To_UID { get; set; }

        public int From_UID { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsRead { get; set; }

        public bool IsDelect { get; set; }

        [Required]
        [StringLength(64)]
        public string Title { get; set; }

        [Required]
        [StringLength(64)]
        public string To_Name { get; set; }

        [Required]
        [StringLength(64)]
        public string From_Name { get; set; }
    }
}
