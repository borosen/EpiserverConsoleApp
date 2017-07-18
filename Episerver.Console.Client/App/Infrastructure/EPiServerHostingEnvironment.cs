using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using EPiServer;
using EPiServer.Web.Hosting;

namespace Episerver.Console.Client.App.Infrastructure
{
    /// <summary>
    /// A minimal hosting environment intended to run without a web context
    /// </summary>
    public class EPiServerHostingEnvironment : IHostingEnvironment
    {
        /// <summary>
        /// Gets the virtual path provider for this application.
        /// </summary>
        /// <value>
        /// The virtual path provider.
        /// </value>
        public VirtualPathProvider VirtualPathProvider { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the application.
        /// </summary>
        /// <value>
        /// The application ID.
        /// </value>
        public string ApplicationID => string.Empty;

        /// <summary>
        /// Gets the physical path on disk to the application's directory.
        /// </summary>
        /// <value>
        /// The application physical path.
        /// </value>
        public string ApplicationPhysicalPath => Global.BaseDirectory;

        /// <summary>
        /// Gets the root virtual path of the application.
        /// </summary>
        /// <value>
        /// The application virtual path.
        /// </value>
        public string ApplicationVirtualPath => "/";

        /// <summary>
        /// Registers a new virtual path provider with the ASP.NET compilation system.
        /// </summary>
        /// <param name="virtualPathProvider">The virtual path provider.</param>
        public void RegisterVirtualPathProvider(VirtualPathProvider virtualPathProvider)
        {
            SetupProviderChain(virtualPathProvider);
        }

        /// <summary>
        /// Maps a virtual path to a physical path on the server.
        /// </summary>
        /// <param name="virtualPath">The virtual path (absolute or relative).</param>
        /// <returns>
        /// The physical path on the server specified by the virtualPath parameter.
        /// </returns>
        public string MapPath(string virtualPath)
        {
            var absolutePath = VirtualPathUtility
                .ToAbsolute(virtualPath, ApplicationVirtualPath)
                .Replace('/', '\\');

            return Path.Combine(
                ApplicationPhysicalPath, absolutePath);
        }

        private void SetupProviderChain(VirtualPathProvider virtualPathProvider)
        {
            var previous = typeof(VirtualPathProvider).GetField("_previous", BindingFlags.NonPublic | BindingFlags.Instance);
            previous?.SetValue(virtualPathProvider, VirtualPathProvider);
            VirtualPathProvider = virtualPathProvider;
        }
    }
}