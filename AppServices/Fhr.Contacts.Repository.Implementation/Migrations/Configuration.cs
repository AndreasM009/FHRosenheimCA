namespace Fhr.Contacts.Repository.Implementation.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ContactContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Fhr.Contacts.Repository.Implementation.ContactContext aContext)
        {            
        }
    }
}
