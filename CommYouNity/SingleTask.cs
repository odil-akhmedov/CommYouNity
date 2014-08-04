//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommYouNity
{
    using System;
    using System.Collections.Generic;
    
    public partial class SingleTask
    {
        public SingleTask()
        {
            this.Communities = new HashSet<Community>();
            this.Locations = new HashSet<Location>();
            this.Members = new HashSet<Member>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public bool Status { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<bool> Flag { get; set; }
    
        public virtual ICollection<Community> Communities { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
