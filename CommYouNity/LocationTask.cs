namespace CommYouNity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocationTask")]
    public partial class LocationTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndTime { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal? Budget { get; set; }
        [Display(Name = "Completed")]
        public bool Status { get; set; }

        public int? Priority { get; set; }

        public bool Flag { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}
