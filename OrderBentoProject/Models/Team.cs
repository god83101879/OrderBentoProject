namespace OrderBentoProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Team
    {
        public int TeamID { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamName { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public int StoreID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string PictureFIle { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(50)]
        public string WhoCreate { get; set; }

        public DateTime? DeleteDate { get; set; }

        [StringLength(50)]
        public string WhoDelete { get; set; }

        public virtual Store Store { get; set; }

        public virtual User User { get; set; }
    }
}
