using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommYouNity.Models
{
    public class MemberTaskView
    {
        public List<Member> member { get; set; }
        public String memberFirstName = "First Name";
        public String memberLastName = "Last Name";
        public String memberPhone = "Phone #";
        public String memberAboutMe = "About Me";
        public String memberImgSrc = "Image";

        public List<MemberTask> memberTask { get; set; }

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