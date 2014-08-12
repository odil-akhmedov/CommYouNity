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
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public Int64 Phone { get; set; }

        [DataType(DataType.MultilineText)]
        [Column(TypeName = "text")]
        public string AboutMe { get; set; }
        public string ImgSrc { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySMS { get; set; }
        public int? CommunityId { get; set; }

        public virtual Community Community { get; set; }

        public virtual ICollection<MemberTask> MemberTasks { get; set; }
    }
}
