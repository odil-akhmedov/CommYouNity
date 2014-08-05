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
        public string Name { get; set; }

        public int Zip { get; set; }

        [Required]
        [StringLength(50)]
        public string ImgSrc { get; set; }

        [Required]
        public string GoogleMap { get; set; }

        public virtual ICollection<Community> Communities { get; set; }

        public virtual ICollection<LocationTask> LocationTasks { get; set; }
    }
}
