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

    protected async Task<Result<IReadOnlyList<T>>> GetAllRequest<T>(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error> {GetError(response.StatusCode)}}};

            var data = rawResponse.ToObject<List<T>>();

            return new SuccessfulResult<IReadOnlyList<T>> {Data = data.GetDataOrEmpty(), DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result<T>> GetRequest<T>(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult<T>{DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            var data = rawResponse.ToObject<T>();
                
            return new SuccessfulResult<T>{Data = data, DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result> GetRequest(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult{DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};
                
            return new SuccessfulResult{DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result> DeleteRequest(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult{DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            return new SuccessfulResult{DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result> PutRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            string requestContent = request.ToJsonString(Deserializer.Options);
            var content = GetRequestContent(requestContent);
            var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult{DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            return new SuccessfulResult{DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result> PutRequest(string url, string request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var content = GetRequestContent(request);
            var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult{DebugInfo = new() {URL = url, Request = request, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            return new SuccessfulResult{DebugInfo = new() {URL = url, Request = request, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result<T>> PostRequest<T, TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            string requestContent = request.ToJsonString(Deserializer.Options);
            var content = GetRequestContent(requestContent);
            var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult<T>{DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            var data = rawResponse.ToObject<T>();

            return new SuccessfulResult<T>{Data = data.GetDataOrDefault(), DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult<T> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result> PostRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            string requestContent = request.ToJsonString(Deserializer.Options);
            var content = GetRequestContent(requestContent);
            var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult{DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            return new SuccessfulResult{DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult{DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult{DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult{DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult{DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult{DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result<IReadOnlyList<T>>> PostListRequest<T, TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            string requestContent = request.ToJsonString(Deserializer.Options);
            var content = GetRequestContent(requestContent);
            var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error> {GetError(response.StatusCode)}}};

            var data = rawResponse.ToObject<List<T>>();

            return new SuccessfulResult<IReadOnlyList<T>> {Data = data.GetDataOrEmpty(), DebugInfo = new() {URL = url, Request = requestContent, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult<IReadOnlyList<T>> {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    protected async Task<Result> PostEmptyRequest(string url, CancellationToken cancellationToken = default)
    {
        string rawResponse = null!;

        try
        {
            var response = await _client.PostAsync(url, null, cancellationToken).ConfigureAwait(false);

            rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error> { GetError(response.StatusCode) }}};

            return new SuccessfulResult {DebugInfo = new() {URL = url, Response = rawResponse, Errors = new List<Error>()}};
        }
        catch (MissingMethodException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(MissingMethodException)]}}};
        }
        catch (HttpRequestException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(HttpRequestException)]}}};
        }
        catch (JsonException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(JsonException)]}}};
        }
        catch (TaskCanceledException e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(TaskCanceledException)]}}};
        }
        catch (Exception e)
        {
            return new FaultedResult {DebugInfo = new() {URL = url, Response = rawResponse, Exception = e.Message, StackTrace = e.StackTrace, Errors = new List<Error> {_errors[nameof(Exception)]}}};
        }
    }

    HttpContent GetRequestContent(string request)
    {
        byte[] requestBytes = Encoding.UTF8.GetBytes(request);
        var content = new ByteArrayContent(requestBytes);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return content;
    }

    protected abstract Error GetError(HttpStatusCode statusCode);
}