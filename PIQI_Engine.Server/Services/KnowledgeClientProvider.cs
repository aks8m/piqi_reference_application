namespace PIQI_Engine.Server.Services
{
    /// <summary>
    /// Contract for a provider that supplies a configured Knowledge <see cref="HttpClient"/>.
    /// </summary>
    public interface IKnowledgeClientProvider
    {
        /// <summary>
        /// Gets the configured <see cref="HttpClient"/> instance for Knowledge server communication.
        /// </summary>
        HttpClient Client { get; }

        Task<HttpResponseMessage> checkPlausabilityAsync(Guid patternId, List<string> codes);
    }

    /// <summary>
    /// Provides an HTTP client configured for interacting with a Knowledge server.
    /// </summary>
    public class KnowledgeClientProvider : IKnowledgeClientProvider
    {
        /// <summary>
        /// Gets the configured <see cref="HttpClient"/> instance for Knowledge interactions.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeClientProvider"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> instance provided by dependency injection.</param>
        public KnowledgeClientProvider(HttpClient client)
        {
            Client = client;
        }

        public async Task<HttpResponseMessage> checkPlausabilityAsync(Guid patternId, List<string> codes)
        {
            

            return null;
        }
    }
}