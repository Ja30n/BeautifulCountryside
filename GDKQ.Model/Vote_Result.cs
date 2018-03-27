namespace GDKQ.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vote_Result
    {
        public int ID { get; set; }

        public int? VoteID { get; set; }

        public int? ChoiceID { get; set; }

        public int? VillagerID { get; set; }

        public DateTime CreateTime { get; set; }

        public bool? Agree { get; set; }
    }
}
