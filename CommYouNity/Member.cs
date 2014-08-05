namespace CommYouNity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        public Member()
        {
            MemberTasks = new HashSet<MemberTask>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        public int Phone { get; set; }

        [Column(TypeName = "text")]
        public string AboutMe { get; set; }

        public int? CommunityId { get; set; }

        public virtual Community Community { get; set; }

        public virtual ICollection<MemberTask> MemberTasks { get; set; }
    }
}
