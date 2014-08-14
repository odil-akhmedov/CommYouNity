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
        [Required]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^(\d{5})[\.\-\+ ]?(\d{4})?$", ErrorMessage = "Must be a valid U.S. Zip Code")]
        public int Zip { get; set; }
        [Required]
        [Display(Name = "Location Email")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "City Photo" )]
        public string ImgSrc { get; set; }
        public string GoogleMap { get; set; }

        public virtual ICollection<Community> Communities { get; set; }

        public virtual ICollection<LocationTask> LocationTasks { get; set; }
    }
}
