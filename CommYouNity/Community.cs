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
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string OfficerName { get; set; }
        public string Email { get; set; }

        public string ImgSrc { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<CommunityTask> CommunityTasks { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
