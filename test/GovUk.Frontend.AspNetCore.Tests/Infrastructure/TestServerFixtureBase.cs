using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.Infrastructure
{
    public abstract class TestServerFixtureBase : IAsyncLifetime, IDisposable
    {
        private IDisposable _host;

        protected TestServerFixtureBase()
        {
        }

        public HttpClient HttpClient { get; private set; }

        public IServiceProvider Services { get; private set; }

        public virtual void Dispose()
        {
            HttpClient?.Dispose();
            _host?.Dispose();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public Task InitializeAsync()
        {
            // Host setup is done here to avoid calling virtual methods from the constructor

            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            ConfigureServices(services);
                        })
                        .Configure(app =>
                        {
                            Configure(app);
                        });
                })
                .Start();

            _host = host;
            HttpClient = host.GetTestClient();
            Services = host.Services;

            return Task.CompletedTask;
        }

        protected virtual void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddGovUkFrontend();
        }
    }
}
