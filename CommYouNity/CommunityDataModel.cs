namespace CommYouNity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CommunityDataModel : DbContext
    {
        public CommunityDataModel()
            : base("name=CommunityDataModel")
        {
        }

        public virtual DbSet<Community> Communities { get; set; }
        public virtual DbSet<CommunityTask> CommunityTasks { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationTask> LocationTasks { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberTask> MemberTasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Community>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Community>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Community>()
                .Property(e => e.OfficerName)
                .IsUnicode(false);

            modelBuilder.Entity<Community>()
                .Property(e => e.ImgSrc)
                .IsUnicode(false);

            modelBuilder.Entity<Community>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Community>()
                .HasMany(e => e.CommunityTasks)
                .WithRequired(e => e.Community)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CommunityTask>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CommunityTask>()
                .Property(e => e.StartTime);

            modelBuilder.Entity<CommunityTask>()
                .Property(e => e.EndTime);
 
            modelBuilder.Entity<CommunityTask>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<CommunityTask>()
                .Property(e => e.Budget)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.ImgSrc)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.GoogleMap)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Communities)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationTasks)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocationTask>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTask>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTask>()
                .Property(e => e.Budget)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Member>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Phone);

            modelBuilder.Entity<Member>()
                .Property(e => e.AboutMe)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.ImgSrc)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.MemberTasks)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MemberTask>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MemberTask>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<MemberTask>()
                .Property(e => e.Budget)
                .HasPrecision(19, 4);
        }
    }
}
