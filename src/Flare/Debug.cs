namespace Flare;

using System.Collections.ObjectModel;
using Model;

public static class Debug
{
    public static DebugInfo WithoutErrors(string url, string responseJson) =>
        new()
        {
            URL = url,
            Response = responseJson,
            Errors = ReadOnlyCollection<Error>.Empty
        };

    public static DebugInfo WithoutErrors(string url, string requestJson, string responseJson) =>
        new()
        {
            URL = url,
            Request = requestJson,
            Response = responseJson,
            Errors = ReadOnlyCollection<Error>.Empty
        };

    public static DebugInfo WithErrors(string url, string responseJson,
        IReadOnlyList<Error> errors) =>
        new()
        {
            URL = url,
            Response = responseJson,
            Errors = errors
        };

    public static DebugInfo WithErrors(string url, string requestJson, string responseJson,
        IReadOnlyList<Error> errors) =>
        new()
        {
            URL = url,
            Request = requestJson,
            Response = responseJson,
            Errors = errors
        };

    public static DebugInfo WithErrors<TException>(string url, TException exception, IReadOnlyList<Error> errors)
        where TException : Exception =>
        new()
        {
            URL = url,
            Exception = exception.Message,
            StackTrace = exception.StackTrace,
            Errors = errors
        };

    public static DebugInfo WithErrors<TException>(string url, string requestJson, string responseJson,
        TException exception, IReadOnlyList<Error> errors) where TException : Exception =>
        new()
        {
            URL = url,
            Request = requestJson,
            Response = responseJson,
            Exception = exception.Message,
            StackTrace = exception.StackTrace,
            Errors = errors
        };

    public static DebugInfo WithErrors<TException>(string url, string responseJson,
        TException exception, IReadOnlyList<Error> errors) where TException : Exception =>
        new()
        {
            URL = url,
            Response = responseJson,
            Exception = exception.Message,
            StackTrace = exception.StackTrace,
            Errors = errors
        };

    public static DebugInfo WithErrors(string url, IReadOnlyList<Error> errors) =>
        new()
        {
            URL = url,
            Errors = errors
        };
}