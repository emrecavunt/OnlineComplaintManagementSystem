namespace EastMed.Data.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    // Get the connection string and connect the database then fill the Database Set object from the database tables and use in coding part as a table
    // OnModelCreating To say the connection type between the table in database like Foreing Key or Identity Attributes
    // Future implementation also possible to add another table to here as like the other and use the update-database command before enable the migration and 
    // backup the database. will be more easier than to regenerate a database and configure again.
    // You can edit the table connection without touching the database  with entity frame work approach.
    // Save TIME WHILE CODING ! 
    public partial class EastMedDB : DbContext
    {
        public EastMedDB()
            : base("name=EastMedDB")
        {
        }

        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<complaint> complaint { get; set; }
        public virtual DbSet<complaint_history> complaint_history { get; set; }
        public virtual DbSet<departmant> departmant { get; set; }
        public virtual DbSet<item> item { get; set; }
        public virtual DbSet<itemtype> itemtype { get; set; }
        public virtual DbSet<location> location { get; set; }
        public virtual DbSet<location_has_item> location_has_item { get; set; }        
        public virtual DbSet<privilege> privilege { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<complaintview> complaintview { get; set; }
        public virtual DbSet<locationview> locationview { get; set; }
        public virtual DbSet<userview> userview { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<category>()
                .Property(e => e.CATEGORY_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.complaint)
                .WithOptional(e => e.category)
                .HasForeignKey(e => e.FK_CATEGORY_ID);

            modelBuilder.Entity<complaint>()
                .Property(e => e.COMMENT)
                .IsUnicode(false);

            modelBuilder.Entity<complaint>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<complaint>()
                .Property(e => e.ITEM_ID)
                .IsUnicode(false);

            modelBuilder.Entity<complaint>()
                .Property(e => e.ImgURL)
                .IsUnicode(false);

            modelBuilder.Entity<complaint>()
                .HasMany(e => e.complaint_history)
                .WithRequired(e => e.complaint)
                .HasForeignKey(e => e.FK_COMPLAINT_ID)
                .WillCascadeOnDelete(false);

           
            modelBuilder.Entity<complaint_history>()
                .Property(e => e.COMMENT)
                .IsUnicode(false);

            modelBuilder.Entity<complaint_history>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<departmant>()
                .Property(e => e.DEPT_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<departmant>()
                .HasMany(e => e.location)
                .WithOptional(e => e.departmant)
                .HasForeignKey(e => e.FK_DEPT_ID);

            modelBuilder.Entity<item>()
                .Property(e => e.ITEM_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.complaint)
                .WithOptional(e => e.item)
                .HasForeignKey(e => e.FK_ITEM_ID);

            modelBuilder.Entity<item>()
                .HasMany(e => e.location_has_item)
                .WithRequired(e => e.item)
                .HasForeignKey(e => e.item_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<itemtype>()
                .Property(e => e.Item_Type)
                .IsUnicode(false);

            modelBuilder.Entity<itemtype>()
                .HasMany(e => e.item)
                .WithRequired(e => e.itemtype)
                .HasForeignKey(e => e.FK_ITEMTYPE_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<location>()
                .Property(e => e.ROOM_ID)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .Property(e => e.TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .HasMany(e => e.complaint)
                .WithOptional(e => e.location)
                .HasForeignKey(e => e.FK_Location_ID);

            modelBuilder.Entity<location>()
                .HasMany(e => e.location_has_item)
                .WithRequired(e => e.location)
                .HasForeignKey(e => e.location_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<location>()
                .HasMany(e => e.user)
                .WithOptional(e => e.location)
                .HasForeignKey(e => e.FK_LOCATION_ID);

            

            modelBuilder.Entity<privilege>()
                .Property(e => e.ROLE)
                .IsUnicode(false);

            modelBuilder.Entity<privilege>()
                .HasMany(e => e.user)
                .WithRequired(e => e.privilege)
                .HasForeignKey(e => e.FK_PRIVILEGE_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
               .Property(e => e.UNI_ID)
               .IsUnicode(false);
            modelBuilder.Entity<user>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.category)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.FK_USER_ID);

            modelBuilder.Entity<user>()
                .HasMany(e => e.complaint)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.FK_USER_ID);

            modelBuilder.Entity<complaintview>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<locationview>()
                .Property(e => e.ROOM_ID)
                .IsUnicode(false);

            modelBuilder.Entity<locationview>()
                .Property(e => e.Item_Type)
                .IsUnicode(false);

            modelBuilder.Entity<locationview>()
                .Property(e => e.ITEM_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<userview>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<userview>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<userview>()
                .Property(e => e.TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<userview>()
                .Property(e => e.PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<userview>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<userview>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);
        }
    }
}
