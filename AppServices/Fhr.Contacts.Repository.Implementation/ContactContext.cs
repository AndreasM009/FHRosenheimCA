//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/2/2016 3:17:58 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System.Data.Common;
using System.Data.Entity;

namespace Fhr.Contacts.Repository.Implementation
{
    public class ContactContext : DbContext
    {        
        public ContactContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ContactContext, Migrations.Configuration>());
        }

        /// <summary>
        /// Test only constructor
        /// </summary>
        /// <param name="aConnection">DbConnection</param>
        public ContactContext(DbConnection aConnection)
            : base(aConnection, true)
        {

        }

        public DbSet<Models.Contact> Contacts { get; set; }
        public DbSet<Models.ContactSettings> ContactsSettings { get; set; }
        public DbSet<Models.ContactImage> ContactsImages { get; set; }

        #region Overrides of DbContext

        protected override void OnModelCreating(DbModelBuilder aModelBuilder)
        {
            aModelBuilder.Entity<Models.Contact>().ToTable("tbl_contact");
            aModelBuilder.Entity<Models.Contact>().HasKey(u => u.Id);

            aModelBuilder.Entity<Models.Contact>().Property(u => u.Id)
                         .HasColumnName("id")
                         .IsRequired();

            aModelBuilder.Entity<Models.Contact>().Property(u => u.OwnerUserId)
                         .HasColumnName("owneruserid")
                         .IsRequired();

            aModelBuilder.Entity<Models.Contact>().Property(u => u.Name)
                         .HasMaxLength(512)
                         .IsUnicode(true)
                         .HasColumnName("name")
                         .IsRequired();

            aModelBuilder.Entity<Models.Contact>().Property(u => u.Surename)
                         .HasMaxLength(512)
                         .IsUnicode(true)
                         .HasColumnName("surename")
                         .IsRequired();

            aModelBuilder.Entity<Models.Contact>().Property(u => u.Birthdate)
                         .HasColumnName("birthdate")
                         .HasColumnType("datetime")
                         .IsRequired();

            aModelBuilder.Entity<Models.Contact>().Property(u => u.Email)
                         .HasColumnName("email")
                         .IsRequired();

            aModelBuilder.Entity<Models.ContactSettings>().ToTable("tbl_contactsettings");
            aModelBuilder.Entity<Models.ContactSettings>().HasKey(s => s.UserId);

            aModelBuilder.Entity<Models.ContactSettings>().Property(s => s.UserId)
                .HasColumnName("userid")
                .IsRequired();

            aModelBuilder.Entity<Models.ContactSettings>().Property(s => s.SortBySurename)
                .HasColumnName("sortbysurename")
                .IsRequired();

            aModelBuilder.Entity<Models.ContactSettings>().Property(s => s.DisplaySurenameFirst)
                .HasColumnName("displaysurenamefirst")
                .IsRequired();

            aModelBuilder.Entity<Models.ContactImage>().ToTable("tbl_contactimages");
            aModelBuilder.Entity<Models.ContactImage>().HasKey(s => s.ContactId);

            aModelBuilder.Entity<Models.ContactImage>().Property(s => s.ContactId)
                .HasColumnName("contactid")
                .IsRequired();

            aModelBuilder.Entity<Models.ContactImage>().Property(s => s.ImageUrl)
                .HasColumnName("url")
                .HasMaxLength(2048)
                .IsRequired();

            base.OnModelCreating(aModelBuilder);
        }

        #endregion
    }
}
