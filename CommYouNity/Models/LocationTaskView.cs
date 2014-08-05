using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CommYouNity.Models
{
    public class LocationTaskView
    {
        public List<Location> location { get; set; }
        public Location singleLocation { get; set; }
        public String locationName = "Location";
        public String locationZip = "Zip";
        public String locationImgSrc = "Image";
        public String locationGoogleMap = "Google map";

        public List<LocationTask> locationTask { get; set; }
        public LocationTask singleLocationTask { get; set; }

        public String locationTaskName = "Task";
        public String locationTaskStartTime = "Starting time";
        public String locationTaskEndTime = "End time";
        public String locationTaskDescription = "Description";
        public String locationTaskBudget = "Budget";
        public String locationTaskStatus = "Status";
        public String locationTaskPriority = "Priority";
        public String locationTaskFlag = "Flag";
    }
}