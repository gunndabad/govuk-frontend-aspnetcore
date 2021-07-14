using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
#if !NETCOREAPP2_1
using Microsoft.Extensions.Hosting;
#endif

namespace GovUk.Frontend.AspNetCore.Tests.Infrastructure
{
    public abstract class TestServerFixtureBase : IDisposable
    {
        private readonly IDisposable _host;

        protected TestServerFixtureBase(
            Action<IServiceCollection> configureServices,
            Action<IApplicationBuilder> configure)
        {
#if NETCOREAPP2_1
            var server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    configureServices(services);
                })
                .Configure(app =>
                {
                    configure(app);
                }));

            _host = server;
            HttpClient = server.CreateClient();
            Services = server.Host.Services;
#else
            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            configureServices(services);
                        })
                        .Configure(app =>
                        {
                            configure(app);
                        });
                })
                .Start();

            _host = host;
            HttpClient = host.GetTestClient();
            Services = host.Services;
#endif
        }

        public HttpClient HttpClient { get; private set; }

        public IServiceProvider Services { get; private set; }

        public virtual void Dispose()
        {
            HttpClient.Dispose();
            _host.Dispose();
        }
    }
}
