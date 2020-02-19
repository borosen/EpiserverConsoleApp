using Episerver.Console.Client.App.Infrastructure;
using EPiServer.Framework.Initialization;
using EPiServer.Web.Hosting;
using StructureMap;

namespace Episerver.Console.Client
{
    public class Program
    {
        public static IContainer Container = null;

        public static void Main(string[] args)
        {
            GenericHostingEnvironment.Instance = new EPiServerHostingEnvironment();
            InitializationModule.FrameworkInitialization(HostType.TestFramework);
            System.Console.WriteLine("EPiServer initialized");

            Container.GetInstance<IApplication>().Run(); // is set via initialization module

            InitializationModule.FrameworkUninitialize();
            System.Console.WriteLine("EPiServer uninitialized");
        }
    }
}
