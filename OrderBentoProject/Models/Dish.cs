namespace OrderBentoProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dish
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DishID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string DishName { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DishPrice { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StoreID { get; set; }

        public DateTime? CreateDate { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string WhoCreate { get; set; }

        public DateTime? DeleteDate { get; set; }

        [StringLength(50)]
        public string WhoDelete { get; set; }
    }
}
