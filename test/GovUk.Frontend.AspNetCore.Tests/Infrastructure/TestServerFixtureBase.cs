using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GovUk.Frontend.AspNetCore.Tests.Infrastructure
{
    public abstract class TestServerFixtureBase : IDisposable
    {
        private readonly IDisposable _host;

        protected TestServerFixtureBase(
            Action<IServiceCollection> configureServices,
            Action<IApplicationBuilder> configure)
        {
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
