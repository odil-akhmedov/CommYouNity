using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommYouNity.Models
{
    public class CommunityTaskView
    {
        public List<Community> community { get; set; }
        public String communityName = "Community";
        public String communityDescription = "Description";
        public String communityOfficerName = "Officer";
        public String communityImgSrc = "Image";
        public Community singleCommunity { get; set; }
        public List<CommunityTask> communityTask { get; set; }

        public String communityTaskName = "Task";
        public String communityTaskStartTime = "Starting time";
        public String communityTaskEndTime = "End time";
        public String communityTaskDescription = "Description";
        public String communityTaskBudget = "Budget";
        public String communityTaskStatus = "Status";
        public String communityTaskPriority = "Priority";
        public String communityTaskFlag = "Flag";
    }
}