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
        [Display(Name = "First Name" )]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
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
        [Display(Name = "About Me")]
        public string AboutMe { get; set; }
        [Display(Name = "Image Source")]
        public string ImgSrc { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name = "Notify By Email")]
        public bool NotifyByEmail { get; set; }
        [Display(Name = "Notify By Text Message")]
        public bool NotifyBySMS { get; set; }
        [Display(Name = "Community Group Name")]
        public int? CommunityId { get; set; }

        public virtual Community Community { get; set; }

        public virtual ICollection<MemberTask> MemberTasks { get; set; }
    }
}
