namespace Flare.Alert.Tests;

using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;

public class FlareApiTesting
{
    protected ServiceCollection GetContainerBuilder(string file, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var services = new ServiceCollection();

        services.AddSingleton<IFlareClient>(x =>
        {
            string data = File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/{file}");
                    
            return new FlareClient(GetClient(data, statusCode));
        });
            
        return services;
    }

    protected ServiceCollection GetContainerBuilder(HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var services = new ServiceCollection();

        services.AddSingleton<IFlareClient>(x => new FlareClient(GetClient(string.Empty, statusCode)));

        return services;
    }

    protected ServiceCollection GetContainerBuilder()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IFlareClient>(x => new FlareClient(GetClient()));

        return services;
    }

    protected HttpClient GetClient(string data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var mock = new Mock<HttpMessageHandler>();

        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(data)
                })
            .Verifiable();

        var uri = new Uri("http://localhost/");
        var client = new HttpClient(mock.Object){BaseAddress = uri};
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }

    protected HttpClient GetClient()
    {
        var uri = new Uri("http://localhost/");
        var client = new HttpClient(new HttpClientHandler()){BaseAddress = uri};
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }
}