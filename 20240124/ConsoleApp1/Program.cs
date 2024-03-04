using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var builder= Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
builder.Services
    .AddHostedService<HostClass>()
    .AddSingleton<IMethod>(sp => sp.GetRequiredService<INewMethod>())
    .AddSingleton<INewMethod, MethodClass>();
        
var host = builder.Build();
await host.RunAsync();


internal interface IMethod {}

internal interface INewMethod: IMethod {}

internal class MethodClass : INewMethod, IAsyncDisposable
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    public async ValueTask DisposeAsync()
    {   
        await _lock.WaitAsync();
        _lock.Release();
        _lock.Dispose();
    }
}


class HostClass(
    IMethod method,
    IHostApplicationLifetime appLifetime) : IHostedService
{

    public Task StartAsync(CancellationToken cancellationToken)
    {
        appLifetime.StopApplication();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

