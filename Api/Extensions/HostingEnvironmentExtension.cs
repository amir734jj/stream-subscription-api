using Microsoft.AspNetCore.Hosting;

namespace API.Extensions
{
    public static class HostingEnvironmentExtension
    {
        /// <summary>
        /// Test whether environment is localhost
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static bool IsLocalhost(this IHostingEnvironment environment) => environment.IsEnvironment("Localhost");

        /// <summary>
        /// Test whether environment is development
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static bool IsDevelopment(this IHostingEnvironment environment) => environment.IsEnvironment("Development");

        /// <summary>
        /// Test whether environment is stage
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static bool IsStage(this IHostingEnvironment environment) => environment.IsEnvironment("Stage");
    }
}