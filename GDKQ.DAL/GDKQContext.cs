namespace GDKQ.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Model;

    public partial class GDKQContext : DbContext
    {
        public GDKQContext()
            : base("name=GDKQContext")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Advice> Advice { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Model.Attribute> Attribute { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Mail> Mail { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<UA_Category> UA_Category { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Villager> Villager { get; set; }
        public virtual DbSet<Vote_Main> Vote_Main { get; set; }
        public virtual DbSet<Vote_Result> Vote_Result { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>()
                .Property(e => e.author)
                .IsFixedLength();

            modelBuilder.Entity<Photo>()
                .Property(e => e.place)
                .IsFixedLength();
        }
    }
}
