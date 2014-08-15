namespace CommYouNity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Community")]
    public partial class Community
    {
        public Community()
        {
            CommunityTasks = new HashSet<CommunityTask>();
            Members = new HashSet<Member>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Community Group")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z''-'\s]*$", ErrorMessage = "Name must be alphabetical")]
        [Display(Name = "Officer Name")]
        public string OfficerName { get; set; }
        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Community Photo")]

        public string ImgSrc { get; set; }
        [Required]
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<CommunityTask> CommunityTasks { get; set; }

        
    }
}
