namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        [StringLength(32)]
        public string Nickname { get; set; }

        public string Hobby { get; set; }

        [StringLength(8)]
        public string Gender { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        [StringLength(32)]
        public string RealName { get; set; }

        public virtual User User { get; set; }
    }
}
