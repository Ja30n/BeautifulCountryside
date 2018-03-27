namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vote_Main
    {
        public int ID { get; set; }

        [StringLength(64)]
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsDeleted { get; set; }

        public bool Enabled { get; set; }
    }
}
