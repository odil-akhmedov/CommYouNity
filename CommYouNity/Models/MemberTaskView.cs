using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;

namespace CommYouNity.Models
{
    public class MemberTaskView
    {
        public List<Member> member { get; set; }
        public IPagedList<Member> pagedMember { get; set; }
        [Display(Name = "First Name")]
        public String memberFirstName = "First Name";
        public String memberLastName = "Last Name";
        public String memberPhone = "Phone #";
        public String memberAboutMe = "About Me";
        public String memberImgSrc = "Image";
        public Member singleMember { get; set; }
        public List<MemberTask> memberTask { get; set; }
        public MemberTask singleMemberTask { get; set; }

        public String memberTaskName = "Task";
        public String memberTaskStartTime = "Starting time";
        public String memberTaskEndTime = "End time";
        public String memberTaskDescription = "Description";
        public String memberTaskBudget = "Budget";
        public String memberTaskStatus = "Status";
        public String memberTaskPriority = "Priority";
        public String memberTaskFlag = "Flag";
    }
}