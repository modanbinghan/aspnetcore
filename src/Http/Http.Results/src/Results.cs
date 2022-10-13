// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Pipelines;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// A factory for <see cref="IResult"/>.
/// </summary>
public static partial class Results
{
    /// <summary>
    /// Creates an <see cref="IResult"/> that on execution invokes <see cref="AuthenticationHttpContextExtensions.ChallengeAsync(HttpContext, string?, AuthenticationProperties?)" />.
    /// <para>
    /// The behavior of this method depends on the <see cref="IAuthenticationService"/> in use.
    /// <see cref="StatusCodes.Status401Unauthorized"/> and <see cref="StatusCodes.Status403Forbidden"/>
    /// are among likely status results.
    /// </para>
    /// </summary>
    /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the authentication
    /// challenge.</param>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Challenge(
        AuthenticationProperties? properties = null,
        IList<string>? authenticationSchemes = null)
        => TypedResults.Challenge(properties, authenticationSchemes);

    /// <summary>
    /// Creates a <see cref="IResult"/> that on execution invokes <see cref="AuthenticationHttpContextExtensions.ForbidAsync(HttpContext, string?, AuthenticationProperties?)"/>.
    /// <para>
    /// By default, executing this result returns a <see cref="StatusCodes.Status403Forbidden"/>. Some authentication schemes, such as cookies,
    /// will convert <see cref="StatusCodes.Status403Forbidden"/> to a redirect to show a login page.
    /// </para>
    /// </summary>
    /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the authentication
    /// challenge.</param>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    /// <remarks>
    /// Some authentication schemes, such as cookies, will convert <see cref="StatusCodes.Status403Forbidden"/> to
    /// a redirect to show a login page.
    /// </remarks>
    public static IResult Forbid(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null)
        => TypedResults.Forbid(properties, authenticationSchemes);

    /// <summary>
    /// Creates an <see cref="IResult"/> that on execution invokes <see cref="AuthenticationHttpContextExtensions.SignInAsync(HttpContext, string?, ClaimsPrincipal, AuthenticationProperties?)" />.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> containing the user claims.</param>
    /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the sign-in operation.</param>
    /// <param name="authenticationScheme">The authentication scheme to use for the sign-in operation.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult SignIn(
        ClaimsPrincipal principal,
        AuthenticationProperties? properties = null,
        string? authenticationScheme = null)
        => TypedResults.SignIn(principal, properties, authenticationScheme);

    /// <summary>
    /// Creates an <see cref="IResult"/> that on execution invokes <see cref="AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, string?, AuthenticationProperties?)" />.
    /// </summary>
    /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the sign-out operation.</param>
    /// <param name="authenticationSchemes">The authentication scheme to use for the sign-out operation.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult SignOut(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null)
        => TypedResults.SignOut(properties, authenticationSchemes);

    /// <summary>
    /// Writes the <paramref name="content"/> string to the HTTP response.
    /// <para>
    /// This is equivalent to <see cref="Text(string?, string?, Encoding?)"/>.
    /// </para>
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <param name="contentEncoding">The content encoding.</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
    /// <remarks>
    /// If encoding is provided by both the 'charset' and the <paramref name="contentEncoding"/> parameters, then
    /// the <paramref name="contentEncoding"/> parameter is chosen as the final encoding.
    /// </remarks>
    public static IResult Content(string? content, string? contentType, Encoding? contentEncoding)
        => Content(content, contentType, contentEncoding, null);

    /// <summary>
    /// Writes the <paramref name="content"/> string to the HTTP response.
    /// <para>
    /// This is equivalent to <see cref="Text(string?, string?, Encoding?, int?)"/>.
    /// </para>
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <param name="contentEncoding">The content encoding.</param>
    /// <param name="statusCode">The status code to return.</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
    /// <remarks>
    /// If encoding is provided by both the 'charset' and the <paramref name="contentEncoding"/> parameters, then
    /// the <paramref name="contentEncoding"/> parameter is chosen as the final encoding.
    /// </remarks>
    public static IResult Content(string? content, string? contentType = null, Encoding? contentEncoding = null, int? statusCode = null)
        => TypedResults.Content(content, contentType, contentEncoding, statusCode);

    /// <summary>
    /// Writes the <paramref name="content"/> string to the HTTP response.
    /// <para>
    /// This is an alias for <see cref="Content(string?, string?, Encoding?)"/>.
    /// </para>
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <param name="contentEncoding">The content encoding.</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
    /// <remarks>
    /// If encoding is provided by both the 'charset' and the <paramref name="contentEncoding"/> parameters, then
    /// the <paramref name="contentEncoding"/> parameter is chosen as the final encoding.
    /// </remarks>
    public static IResult Text(string? content, string? contentType, Encoding? contentEncoding)
        => Text(content, contentType, contentEncoding, null);

    /// <summary>
    /// Writes the <paramref name="content"/> string to the HTTP response.
    /// <para>
    /// This is an alias for <see cref="Content(string?, string?, Encoding?, int?)"/>.
    /// </para>
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <param name="contentEncoding">The content encoding.</param>
    /// <param name="statusCode">The status code to return.</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
    /// <remarks>
    /// If encoding is provided by both the 'charset' and the <paramref name="contentEncoding"/> parameters, then
    /// the <paramref name="contentEncoding"/> parameter is chosen as the final encoding.
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult Text(string? content, string? contentType = null, Encoding? contentEncoding = null, int? statusCode = null)
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        => TypedResults.Text(content, contentType, contentEncoding, statusCode);

    /// <summary>
    /// Writes the <paramref name="utf8Content"/> UTF-8 encoded text to the HTTP response.
    /// </summary>
    /// <param name="utf8Content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <param name="statusCode">The status code to return.</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads
    public static IResult Text(ReadOnlySpan<byte> utf8Content, string? contentType = null, int? statusCode = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads
        => TypedResults.Text(utf8Content, contentType, statusCode);

    /// <summary>
    /// Writes the <paramref name="content"/> string to the HTTP response.
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
    public static IResult Content(string? content, MediaTypeHeaderValue contentType)
        => TypedResults.Content(content, contentType);

    /// <summary>
    /// Creates a <see cref="IResult"/> that serializes the specified <paramref name="data"/> object to JSON.
    /// </summary>
    /// <param name="data">The object to write as JSON.</param>
    /// <param name="options">The serializer options to use when serializing the value.</param>
    /// <param name="contentType">The content-type to set on the response.</param>
    /// <param name="statusCode">The status code to set on the response.</param>
    /// <returns>The created <see cref="JsonHttpResult{TValue}"/> that serializes the specified <paramref name="data"/>
    /// as JSON format for the response.</returns>
    /// <remarks>Callers should cache an instance of serializer settings to avoid
    /// recreating cached data with each call.</remarks>
    public static IResult Json(object? data, JsonSerializerOptions? options = null, string? contentType = null, int? statusCode = null)
        => Json<object>(data, options, contentType, statusCode);

    /// <summary>
    /// Creates a <see cref="IResult"/> that serializes the specified <paramref name="data"/> object to JSON.
    /// </summary>
    /// <param name="data">The object to write as JSON.</param>
    /// <param name="options">The serializer options to use when serializing the value.</param>
    /// <param name="contentType">The content-type to set on the response.</param>
    /// <param name="statusCode">The status code to set on the response.</param>
    /// <returns>The created <see cref="JsonHttpResult{TValue}"/> that serializes the specified <paramref name="data"/>
    /// as JSON format for the response.</returns>
    /// <remarks>Callers should cache an instance of serializer settings to avoid
    /// recreating cached data with each call.</remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult Json<TValue>(TValue? data, JsonSerializerOptions? options = null, string? contentType = null, int? statusCode = null)
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        => TypedResults.Json(data, options, contentType, statusCode);

    /// <summary>
    /// Writes the byte-array content to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// <para>
    /// This API is an alias for <see cref="Bytes(byte[], string, string?, bool, DateTimeOffset?, EntityTagHeaderValue?)"/>.</para>
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> associated with the file.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult File(
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        byte[] fileContents,
        string? contentType = null,
        string? fileDownloadName = null,
        bool enableRangeProcessing = false,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null)
        => TypedResults.File(fileContents, contentType, fileDownloadName, enableRangeProcessing, lastModified, entityTag);

    /// <summary>
    /// Writes the byte-array content to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// <para>
    /// This API is an alias for <see cref="File(byte[], string, string?, bool, DateTimeOffset?, EntityTagHeaderValue?)"/>.</para>
    /// </summary>
    /// <param name="contents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> associated with the file.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Bytes(
        byte[] contents,
        string? contentType = null,
        string? fileDownloadName = null,
        bool enableRangeProcessing = false,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null)
        => TypedResults.Bytes(contents, contentType, fileDownloadName, enableRangeProcessing, lastModified, entityTag);

    /// <summary>
    /// Writes the byte-array content to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// </summary>
    /// <param name="contents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> associated with the file.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult Bytes(
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        ReadOnlyMemory<byte> contents,
        string? contentType = null,
        string? fileDownloadName = null,
        bool enableRangeProcessing = false,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null)
        => TypedResults.Bytes(contents, contentType, fileDownloadName, enableRangeProcessing, lastModified, entityTag);

    /// <summary>
    /// Writes the specified <see cref="System.IO.Stream"/> to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// <para>
    /// This API is an alias for <see cref="Stream(Stream, string, string?, DateTimeOffset?, EntityTagHeaderValue?, bool)"/>.
    /// </para>
    /// </summary>
    /// <param name="fileStream">The <see cref="System.IO.Stream"/> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The the file name to be used in the <c>Content-Disposition</c> header.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.
    /// Used to configure the <c>Last-Modified</c> response header and perform conditional range requests.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> to be configure the <c>ETag</c> response header
    /// and perform conditional requests.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult File(
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        Stream fileStream,
        string? contentType = null,
        string? fileDownloadName = null,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false)
        => TypedResults.File(fileStream, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);

    /// <summary>
    /// Writes the specified <see cref="System.IO.Stream"/> to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// <para>
    /// This API is an alias for <see cref="File(Stream, string, string?, DateTimeOffset?, EntityTagHeaderValue?, bool)"/>.
    /// </para>
    /// </summary>
    /// <param name="stream">The <see cref="System.IO.Stream"/> to write to the response.</param>
    /// <param name="contentType">The <c>Content-Type</c> of the response. Defaults to <c>application/octet-stream</c>.</param>
    /// <param name="fileDownloadName">The the file name to be used in the <c>Content-Disposition</c> header.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.
    /// Used to configure the <c>Last-Modified</c> response header and perform conditional range requests.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> to be configure the <c>ETag</c> response header
    /// and perform conditional requests.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    /// <remarks>
    /// The <paramref name="stream" /> parameter is disposed after the response is sent.
    /// </remarks>
    public static IResult Stream(
        Stream stream,
        string? contentType = null,
        string? fileDownloadName = null,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false)
        => TypedResults.Stream(stream, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);

    /// <summary>
    /// Writes the contents of specified <see cref="System.IO.Pipelines.PipeReader"/> to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// </summary>
    /// <param name="pipeReader">The <see cref="System.IO.Pipelines.PipeReader"/> to write to the response.</param>
    /// <param name="contentType">The <c>Content-Type</c> of the response. Defaults to <c>application/octet-stream</c>.</param>
    /// <param name="fileDownloadName">The the file name to be used in the <c>Content-Disposition</c> header.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.
    /// Used to configure the <c>Last-Modified</c> response header and perform conditional range requests.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> to be configure the <c>ETag</c> response header
    /// and perform conditional requests.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    /// <remarks>
    /// The <paramref name="pipeReader" /> parameter is completed after the response is sent.
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult Stream(
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        PipeReader pipeReader,
        string? contentType = null,
        string? fileDownloadName = null,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false)
        => TypedResults.Stream(pipeReader, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);

    /// <summary>
    /// Allows writing directly to the response body.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// </summary>
    /// <param name="streamWriterCallback">The callback that allows users to write directly to the response body.</param>
    /// <param name="contentType">The <c>Content-Type</c> of the response. Defaults to <c>application/octet-stream</c>.</param>
    /// <param name="fileDownloadName">The the file name to be used in the <c>Content-Disposition</c> header.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.
    /// Used to configure the <c>Last-Modified</c> response header and perform conditional range requests.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> to be configure the <c>ETag</c> response header
    /// and perform conditional requests.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult Stream(
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        Func<Stream, Task> streamWriterCallback,
        string? contentType = null,
        string? fileDownloadName = null,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null)
        => TypedResults.Stream(streamWriterCallback, contentType, fileDownloadName, lastModified, entityTag);

    /// <summary>
    /// Writes the file at the specified <paramref name="path"/> to the response.
    /// <para>
    /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
    /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
    /// </para>
    /// </summary>
    /// <param name="path">The path to the file. When not rooted, resolves the path relative to <see cref="IWebHostEnvironment.WebRootFileProvider"/>.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult File(
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        string path,
        string? contentType = null,
        string? fileDownloadName = null,
        DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false)
        => Path.IsPathRooted(path)
            ? TypedResults.PhysicalFile(path, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)
            : TypedResults.VirtualFile(path, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);

    /// <summary>
    /// Redirects to the specified <paramref name="url"/>.
    /// <list type="bullet">
    /// <item>
    /// <description>When <paramref name="permanent"/> and <paramref name="preserveMethod"/> are set, sets the <see cref="StatusCodes.Status308PermanentRedirect"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>When <paramref name="preserveMethod"/> is set, sets the <see cref="StatusCodes.Status307TemporaryRedirect"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>When <paramref name="permanent"/> is set, sets the <see cref="StatusCodes.Status301MovedPermanently"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>Otherwise, configures <see cref="StatusCodes.Status302Found"/>.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="url">The URL to redirect to.</param>
    /// <param name="permanent">Specifies whether the redirect should be permanent (301) or temporary (302).</param>
    /// <param name="preserveMethod">If set to true, make the temporary redirect (307) or permanent redirect (308) preserve the initial request method.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Redirect(string url, bool permanent = false, bool preserveMethod = false)
        => TypedResults.Redirect(url, permanent, preserveMethod);

    /// <summary>
    /// Redirects to the specified <paramref name="localUrl"/>.
    /// <list type="bullet">
    /// <item>
    /// <description>When <paramref name="permanent"/> and <paramref name="preserveMethod"/> are set, sets the <see cref="StatusCodes.Status308PermanentRedirect"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>When <paramref name="preserveMethod"/> is set, sets the <see cref="StatusCodes.Status307TemporaryRedirect"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>When <paramref name="permanent"/> is set, sets the <see cref="StatusCodes.Status301MovedPermanently"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>Otherwise, configures <see cref="StatusCodes.Status302Found"/>.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="localUrl">The local URL to redirect to.</param>
    /// <param name="permanent">Specifies whether the redirect should be permanent (301) or temporary (302).</param>
    /// <param name="preserveMethod">If set to true, make the temporary redirect (307) or permanent redirect (308) preserve the initial request method.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult LocalRedirect(string localUrl, bool permanent = false, bool preserveMethod = false)
        => TypedResults.LocalRedirect(localUrl, permanent, preserveMethod);

    /// <summary>
    /// Redirects to the specified route.
    /// <list type="bullet">
    /// <item>
    /// <description>When <paramref name="permanent"/> and <paramref name="preserveMethod"/> are set, sets the <see cref="StatusCodes.Status308PermanentRedirect"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>When <paramref name="preserveMethod"/> is set, sets the <see cref="StatusCodes.Status307TemporaryRedirect"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>When <paramref name="permanent"/> is set, sets the <see cref="StatusCodes.Status301MovedPermanently"/> status code.</description>
    /// </item>
    /// <item>
    /// <description>Otherwise, configures <see cref="StatusCodes.Status302Found"/>.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="permanent">Specifies whether the redirect should be permanent (301) or temporary (302).</param>
    /// <param name="preserveMethod">If set to true, make the temporary redirect (307) or permanent redirect (308) preserve the initial request method.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult RedirectToRoute(string? routeName = null, object? routeValues = null, bool permanent = false, bool preserveMethod = false, string? fragment = null)
        => TypedResults.RedirectToRoute(routeName, routeValues, permanent, preserveMethod, fragment);

    /// <summary>
    /// Creates an <see cref="IResult"/> object by specifying a <paramref name="statusCode"/>.
    /// </summary>
    /// <param name="statusCode">The status code to set on the response.</param>
    /// <returns>The created <see cref="IResult"/> object for the response.</returns>
    public static IResult StatusCode(int statusCode)
        => TypedResults.StatusCode(statusCode);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status404NotFound"/> response.
    /// </summary>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
    public static IResult NotFound(object? value = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        => NotFound<object>(value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status404NotFound"/> response.
    /// </summary>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult NotFound<TValue>(TValue? value)
        => value is null ? TypedResults.NotFound() : TypedResults.NotFound(value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status401Unauthorized"/> response.
    /// </summary>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Unauthorized()
        => TypedResults.Unauthorized();

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status400BadRequest"/> response.
    /// </summary>
    /// <param name="error">An error object to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
    public static IResult BadRequest(object? error = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        => BadRequest<object>(error);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status400BadRequest"/> response.
    /// </summary>
    /// <param name="error">An error object to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult BadRequest<TValue>(TValue? error)
        => error is null ? TypedResults.BadRequest() : TypedResults.BadRequest(error);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status409Conflict"/> response.
    /// </summary>
    /// <param name="error">An error object to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
    public static IResult Conflict(object? error = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        => Conflict<object>(error);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status409Conflict"/> response.
    /// </summary>
    /// <param name="error">An error object to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Conflict<TValue>(TValue? error)
        => error is null ? TypedResults.Conflict() : TypedResults.Conflict(error);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status204NoContent"/> response.
    /// </summary>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult NoContent()
        => TypedResults.NoContent();

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
    public static IResult Ok(object? value = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        => Ok<object>(value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Ok<TValue>(TValue? value)
        => value is null ? TypedResults.Ok() : TypedResults.Ok(value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status422UnprocessableEntity"/> response.
    /// </summary>
    /// <param name="error">An error object to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
    public static IResult UnprocessableEntity(object? error = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        => UnprocessableEntity<object>(error);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status422UnprocessableEntity"/> response.
    /// </summary>
    /// <param name="error">An error object to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult UnprocessableEntity<TValue>(TValue? error)
        => error is null ? TypedResults.UnprocessableEntity() : TypedResults.UnprocessableEntity(error);

    /// <summary>
    /// Produces a <see cref="ProblemDetails"/> response.
    /// </summary>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
    /// <param name="extensions">The value for <see cref="ProblemDetails.Extensions" />.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Problem(
        string? detail = null,
        string? instance = null,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        IDictionary<string, object?>? extensions = null)
        => TypedResults.Problem(detail, instance, statusCode, title, type, extensions);

    /// <summary>
    /// Produces a <see cref="ProblemDetails"/> response.
    /// </summary>
    /// <param name="problemDetails">The <see cref="ProblemDetails"/>  object to produce a response from.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Problem(ProblemDetails problemDetails)
        => TypedResults.Problem(problemDetails);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status400BadRequest"/> response
    /// with a <see cref="HttpValidationProblemDetails"/> value.
    /// </summary>
    /// <param name="errors">One or more validation errors.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
    /// <param name="statusCode">The status code.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title" />. Defaults to "One or more validation errors occurred."</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
    /// <param name="extensions">The value for <see cref="ProblemDetails.Extensions" />.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult ValidationProblem(
        IDictionary<string, string[]> errors,
        string? detail = null,
        string? instance = null,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        IDictionary<string, object?>? extensions = null)
    {
        // TypedResults.ValidationProblem() does not allow setting the statusCode so we do this manually here
        var problemDetails = new HttpValidationProblemDetails(errors)
        {
            Detail = detail,
            Instance = instance,
            Type = type,
            Status = statusCode,
        };

        problemDetails.Title = title ?? problemDetails.Title;

        if (extensions is not null)
        {
            foreach (var extension in extensions)
            {
                problemDetails.Extensions.Add(extension);
            }
        }

        return TypedResults.Problem(problemDetails);
    }

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="uri">The URI at which the content has been created.</param>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Created(string uri, object? value)
        => Created<object>(uri, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="uri">The URI at which the content has been created.</param>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Created<TValue>(string uri, TValue? value)
        => value is null ? TypedResults.Created(uri) : TypedResults.Created(uri, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="uri">The URI at which the content has been created.</param>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Created(Uri uri, object? value)
        => Created<object>(uri, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="uri">The URI at which the content has been created.</param>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Created<TValue>(Uri uri, TValue? value)
        => value is null ? TypedResults.Created(uri) : TypedResults.Created(uri, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult CreatedAtRoute(string? routeName = null, object? routeValues = null, object? value = null)
        => CreatedAtRoute<object>(routeName, routeValues, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The value to be included in the HTTP response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult CreatedAtRoute<TValue>(string? routeName = null, object? routeValues = null, TValue? value = default)
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        => value is null ? TypedResults.CreatedAtRoute(routeName, routeValues) : TypedResults.CreatedAtRoute(value, routeName, routeValues);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status202Accepted"/> response.
    /// </summary>
    /// <param name="uri">The URI with the location at which the status of requested content can be monitored.</param>
    /// <param name="value">The optional content value to format in the response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
    public static IResult Accepted(string? uri = null, object? value = null)
        => Accepted<object>(uri, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status202Accepted"/> response.
    /// </summary>
    /// <param name="uri">The URI with the location at which the status of requested content can be monitored.</param>
    /// <param name="value">The optional content value to format in the response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult Accepted<TValue>(string? uri = null, TValue? value = default)
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        => value is null ? TypedResults.Accepted(uri) : TypedResults.Accepted(uri, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status202Accepted"/> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
    public static IResult AcceptedAtRoute(string? routeName = null, object? routeValues = null, object? value = null)
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        => AcceptedAtRoute<object>(routeName, routeValues, value);

    /// <summary>
    /// Produces a <see cref="StatusCodes.Status202Accepted"/> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the response body.</param>
    /// <returns>The created <see cref="IResult"/> for the response.</returns>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    public static IResult AcceptedAtRoute<TValue>(string? routeName = null, object? routeValues = null, TValue? value = default)
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        => value is null ? TypedResults.AcceptedAtRoute(routeName, routeValues) : TypedResults.AcceptedAtRoute(value, routeName, routeValues);

    /// <summary>
    /// Produces an empty result response, that when executed will do nothing.
    /// </summary>
    public static IResult Empty { get; } = TypedResults.Empty;

    /// <summary>
    /// Provides a container for external libraries to extend
    /// the default `Results` set with their own samples.
    /// </summary>
    public static IResultExtensions Extensions { get; } = new ResultExtensions();
}