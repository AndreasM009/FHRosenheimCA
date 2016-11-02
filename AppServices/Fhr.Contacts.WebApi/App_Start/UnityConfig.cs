using Microsoft.Practices.Unity;
using System.Web.Http;
using Fhr.Contacts.TransactionScript.Implementation;
using Fhr.Contacts.Repository.Implementation;
using Fhr.Contacts.Repository.Interfaces;
using Fhr.Contacts.TransactionScript.Interfaces;
using Unity.WebApi;

namespace Fhr.Contacts.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration aConfiguration)
        {
			var container = new UnityContainer();
            
            // Register the IContactService and its Implementation
            container.RegisterType<ContactContext>(new TransientLifetimeManager(), new InjectionConstructor());
            container.RegisterType<IContactRepository, ContactRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IContactImageRepository, ContactImageRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IContactsSettingsRepository, ContactSettingsRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IImageBlobService, ImageBlobService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IContactService, ContactService>(new ContainerControlledLifetimeManager());            

            aConfiguration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}