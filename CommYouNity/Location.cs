namespace CommYouNity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            Communities = new HashSet<Community>();
            LocationTasks = new HashSet<LocationTask>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Location Name")]
        public string Name { get; set; }
        [Display(Name = "Zip Code")]
        public int Zip { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name = "Image Source" )]
        public string ImgSrc { get; set; }
        public string GoogleMap { get; set; }

        public virtual ICollection<Community> Communities { get; set; }

        public virtual ICollection<LocationTask> LocationTasks { get; set; }
    }
}
