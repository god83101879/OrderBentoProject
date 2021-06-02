namespace OrderBentoProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [Key]
        [Column(Order = 0)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TeamID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StoreID { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string DIshName { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DishNum { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime CreateDate { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string WhoCreate { get; set; }

        public DateTime? DeleteDate { get; set; }

        [StringLength(50)]
        public string WhoDelete { get; set; }
    }
}
