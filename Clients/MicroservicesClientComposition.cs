using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Reflection;

namespace GrpcTest.Clients
{
    public static class MicroservicesClientComposition
    {
        private static SslCredentials CreateSslCredentials(string cert)
        {
            var cacert = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, cert));
            return new SslCredentials(cacert);
        }

        public static TClient CreateMicroserviceClient<TClient>(string cert, string host, int port) where TClient : class
        {
            var channel = new Grpc.Core.Channel(host, port, CreateSslCredentials(cert));
            var invoker = channel.Intercept(m => { m.Add("client-source", Assembly.GetCallingAssembly().GetName().Name); return m; });
            return (TClient)Activator.CreateInstance(typeof(TClient), invoker);
        }

        public static IServiceCollection AddMicroserviceClient<TClient>(this IServiceCollection services, string cert, string host, int port) where TClient : class
        {
            services.AddSingleton(x => CreateMicroserviceClient<TClient>(cert, host, port));
            return services;
        }
    }
}
