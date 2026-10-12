namespace Common.DropBox
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    public class DropBoxApiClientFromSwagger
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public DropBoxApiClientFromSwagger(HttpClient httpClient, string baseUrl = "https://api.dropboxapi.com/2")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            BaseUrl = baseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(baseUrl));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };
        }

        public string BaseUrl { get; }

        public async Task<Account> GetCurrentAccountAsync(CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(BaseUrl + "/"), "users/get_current_account"));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await DeserializeAsync<Account>(response, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListFolderResult> ListFolderAsync(ListFolderArg body, CancellationToken cancellationToken = default)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(BaseUrl + "/"), "files/list_folder"));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.Content = new StringContent(JsonSerializer.Serialize(body, _jsonOptions), Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await DeserializeAsync<ListFolderResult>(response, cancellationToken).ConfigureAwait(false);
        }

        public async Task<object?> UploadFileAsync(UploadFileArg uploadArg, HttpContent fileContent, CancellationToken cancellationToken = default)
        {
            if (uploadArg == null)
            {
                throw new ArgumentNullException(nameof(uploadArg));
            }

            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(BaseUrl + "/"), "files/upload"));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.Headers.Add("Dropbox-API-Arg", JsonSerializer.Serialize(uploadArg, _jsonOptions));
            request.Content = fileContent;
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await DeserializeAsync<object>(response, cancellationToken).ConfigureAwait(false);
        }

        public async Task<object?> CreateSharedLinkWithSettingsAsync(SharedLinkRequest body, CancellationToken cancellationToken = default)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(BaseUrl + "/"), "sharing/create_shared_link_with_settings"));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.Content = new StringContent(JsonSerializer.Serialize(body, _jsonOptions), Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await DeserializeAsync<object>(response, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("Response content was empty or could not be deserialized.");
        }
    }

    public class Account
    {
        [JsonPropertyName("account_id")]
        public string? AccountId { get; set; }

        [JsonPropertyName("name")]
        public Name? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("email_verified")]
        public bool? EmailVerified { get; set; }

        [JsonPropertyName("profile_photo_url")]
        public string? ProfilePhotoUrl { get; set; }

        [JsonPropertyName("disabled")]
        public bool? Disabled { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("given_name")]
        public string? GivenName { get; set; }

        [JsonPropertyName("surname")]
        public string? Surname { get; set; }

        [JsonPropertyName("familiar_name")]
        public string? FamiliarName { get; set; }

        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }
    }

    public class ListFolderArg
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("recursive")]
        public bool Recursive { get; set; }

        [JsonPropertyName("include_media_info")]
        public bool IncludeMediaInfo { get; set; }

        [JsonPropertyName("include_deleted")]
        public bool IncludeDeleted { get; set; }
    }

    public class ListFolderResult
    {
        [JsonPropertyName("entries")]
        public IEnumerable<object>? Entries { get; set; }

        [JsonPropertyName("cursor")]
        public string? Cursor { get; set; }

        [JsonPropertyName("has_more")]
        public bool? HasMore { get; set; }
    }

    public class UploadFileArg
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("autorename")]
        public bool? Autorename { get; set; }

        [JsonPropertyName("mute")]
        public bool? Mute { get; set; }
    }

    public class SharedLinkRequest
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("settings")]
        public SharedLinkSettings? Settings { get; set; }
    }

    public class SharedLinkSettings
    {
        [JsonPropertyName("requested_visibility")]
        public string? RequestedVisibility { get; set; }
    }
}
