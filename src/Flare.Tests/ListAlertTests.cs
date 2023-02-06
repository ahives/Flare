namespace Flare.Tests;

[TestFixture]
public class ListAlertTests
{
    FlareConfig _config;

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
    }

    [Test]
    public async Task Test1()
    {
        var result = await new FlareClient(_config)
            .API<Alert>()
            .List(x =>
            {
                x.Offset(5);
                x.Limit(100);
                x.Order(OrderType.Asc);
                x.Sort(SortableFields.Status);
                x.SearchIdentifier(NewId.NextGuid());
            });
        
        Console.WriteLine(result.DebugInfo.URL);
    }
}