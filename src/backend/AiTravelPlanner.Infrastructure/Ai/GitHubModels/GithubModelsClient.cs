using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed class GitHubModelsClient : IGitHubModelsClient
{
    private readonly HttpClient _httpClient;
    private readonly GitHubModelsOptions _options;
    private readonly ILogger<GitHubModelsClient> _logger;

    public GitHubModelsClient(
        HttpClient httpClient,
        IOptions<GitHubModelsOptions> options,
        ILogger<GitHubModelsClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> CompleteChatAsync(
        IReadOnlyList<GitHubModelsMessage> messages,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Token))
        {
            throw new InvalidOperationException("GitHub Models token is not configured.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint);

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.Token);

        request.Headers.Add("X-GitHub-Api-Version", _options.ApiVersion);

        request.Content = JsonContent.Create(new
        {
            model = _options.Model,
            messages = messages.Select(message => new
            {
                role = message.Role,
                content = message.Content
            }),
            max_tokens = _options.MaxTokens,
            temperature = _options.Temperature
        });

        _logger.LogInformation(
            "Calling GitHub Models. Model: {Model}",
            _options.Model
        );

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation(
            "GitHub Models responded with status code {StatusCode}.",
            response.StatusCode
        );

        var completion = await response.Content
            .ReadFromJsonAsync<GitHubModelsChatCompletionResponse>(cancellationToken);

        return completion?.Choices.FirstOrDefault()?.Message.Content
            ?? throw new InvalidOperationException("GitHub Models returned an empty response.");
    }
}