namespace Flare;

using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Extensions;
using Serialization;

public abstract class FlareHttpClient
{
    readonly HttpClient _client;
    readonly IDictionary<string, Error> _errors;

    protected FlareHttpClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _errors = new Dictionary<string, Error>
        {
            {nameof(MissingMethodException), new() {Reason = "Could not properly handle '.' and/or '/' characters in URL."}},
            {nameof(HttpRequestException), new() {Reason = "Request failed due to network connectivity, DNS failure, server certificate validation, or timeout."}},
            {nameof(JsonException), new() {Reason = "The JSON is invalid or T is not compatible with the JSON."}},
            {nameof(Exception), new() {Reason = "Something went bad in FlareHttpClient."}},
            {nameof(TaskCanceledException), new() {Reason = "Request failed due to timeout."}}
        };
    }

    protected async Task<Maybe<IReadOnlyList<T>>> GetAllRequest<T>(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failures<T>(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(rawResponse.ToObject<List<T>>().GetDataOrEmpty(), Debug.WithoutErrors(url, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe<T>> GetRequest<T>(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed<T>(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(rawResponse.ToObject<T>().GetDataOrDefault(), Debug.WithoutErrors(url, rawResponse))!;
        }
        catch (MissingMethodException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe> GetRequest(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(Debug.WithoutErrors(url, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failed(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe> DeleteRequest(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(Debug.WithoutErrors(url, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failed(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe<T>> DeleteRequest<T>(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed<T>(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(rawResponse.ToObject<T>().GetDataOrDefault(), Debug.WithoutErrors(url, rawResponse))!;
        }
        catch (MissingMethodException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe> PutRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;
        string rawRequest = null!;

        try
        {
            rawRequest = request.ToJsonString(Serializer.Options);
            var content = GetRequestContent(rawRequest);
            var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(Debug.WithoutErrors(url, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failed(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe> PutRequest(string url, string request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var content = GetRequestContent(request);
            var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(Debug.WithoutErrors(url, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failed(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe<T>> PostRequest<T, TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;
        string rawRequest = null!;

        try
        {
            rawRequest = request.ToJsonString(Serializer.Options);
            var content = GetRequestContent(rawRequest);
            var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed<T>(Debug.WithErrors(url, rawRequest, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(rawResponse.ToObject<T>().GetDataOrDefault(), Debug.WithoutErrors(url, rawRequest, rawResponse))!;
        }
        catch (MissingMethodException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe> PostRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;
        string rawRequest = null!;

        try
        {
            rawRequest = request.ToJsonString(Serializer.Options);
            var content = GetRequestContent(rawRequest);
            var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(Debug.WithoutErrors(url, rawRequest, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failed(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe<IReadOnlyList<T>>> PostListRequest<T, TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;
        string rawRequest = null!;

        try
        {
            rawRequest = request.ToJsonString(Serializer.Options);
            var content = GetRequestContent(rawRequest);
            var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failures<T>(Debug.WithErrors(url, rawRequest, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(rawResponse.ToObject<List<T>>().GetDataOrEmpty(), Debug.WithoutErrors(url, rawRequest, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failures<T>(Debug.WithErrors(url, rawRequest, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    protected async Task<Maybe> PostEmptyRequest(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.PostAsync(url, null, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return !response.IsSuccessStatusCode
                ? Response.Failed(Debug.WithErrors(url, rawResponse, new List<Error> {GetError(response.StatusCode)}))
                : Response.Success(Debug.WithoutErrors(url, rawResponse));
        }
        catch (MissingMethodException e)
        {
            return Response.Failed(Debug.WithErrors(url, e, new List<Error> {_errors[nameof(MissingMethodException)]}));
        }
        catch (HttpRequestException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(HttpRequestException)]}));
        }
        catch (JsonException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(JsonException)]}));
        }
        catch (TaskCanceledException e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(TaskCanceledException)]}));
        }
        catch (Exception e)
        {
            return Response.Failed(Debug.WithErrors(url, rawResponse, e, new List<Error> {_errors[nameof(Exception)]}));
        }
    }

    HttpContent GetRequestContent(string request)
    {
        byte[] requestBytes = Encoding.UTF8.GetBytes(request);
        var content = new ByteArrayContent(requestBytes);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return content;
    }

    protected Error GetError(HttpStatusCode statusCode)
    {
        throw new NotSupportedException();
    }
}