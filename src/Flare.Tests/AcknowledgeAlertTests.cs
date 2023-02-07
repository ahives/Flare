using Flare.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Flare.Tests;

[TestFixture]
public class AcknowledgeAlertTests
{
    FlareConfig _config;
    IServiceCollection _services;

    [SetUp]
    public void Setup()
    {
        _config = new FlareConfigProvider()
            .Configure(p =>
            {
                p.Client(c =>
                {
                    c.ConnectTo("");
                    c.UsingApiKey("");
                    c.UsingCredentials("guest", "guest");
                });
            });

        _services = new ServiceCollection()
            .AddFlare();
    }

    [Test]
    public async Task Test1()
    {
        var result = await new FlareClient(_config)
            .API<Alert>()
            .Acknowledge(NewId.NextGuid(), x =>
                {
                    x.User("Flare");
                    x.Source("Flare");
                    x.Note("");
                },
                q =>
                {
                    q.SearchIdentifierType(AcknowledgeSearchIdentifierType.Id);
                });
        
        Console.WriteLine(result.DebugInfo.URL);
        Console.WriteLine(result.DebugInfo.Request);
    }

    [Test]
    public async Task Test2()
    {
        var result = await new FlareClient(_config)
            .API<Alert>()
            .Acknowledge(NewId.NextGuid(), x =>
                {
                    x.User("Flare");
                    x.Source("Flare");
                    x.Note("");
                },
                q =>
                {
                    q.SearchIdentifierType(AcknowledgeSearchIdentifierType.Id);
                });
        
        Console.WriteLine(result.DebugInfo.URL);
        Console.WriteLine(result.DebugInfo.Request);
    }
}