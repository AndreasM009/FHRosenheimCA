namespace Fhr.Contacts.Repository.Implementation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_contact",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        owneruserid = c.Guid(nullable: false),
                        name = c.String(nullable: false, maxLength: 512),
                        surename = c.String(nullable: false, maxLength: 512),
                        email = c.String(nullable: false),
                        birthdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tbl_contactimages",
                c => new
                    {
                        contactid = c.Guid(nullable: false),
                        url = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.contactid);
            
            CreateTable(
                "dbo.tbl_contactsettings",
                c => new
                    {
                        userid = c.Guid(nullable: false),
                        sortbysurename = c.Boolean(nullable: false),
                        displaysurenamefirst = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.userid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbl_contactsettings");
            DropTable("dbo.tbl_contactimages");
            DropTable("dbo.tbl_contact");
        }
    }
}
