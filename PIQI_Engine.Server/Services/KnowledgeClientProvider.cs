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

        Task<HttpResponseMessage> CheckLabResultPlausibilityAsync(string dobString, string testCode, string resultValue, List<Tuple<string, string>> coordinateParams);

        Task<HttpResponseMessage> CheckLabDevicePlausibilityAsync(string testCode, string refRangeLow, string refRangeHigh, string unit, List<Tuple<string, string>> coordinateParams);

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

        public async Task<HttpResponseMessage> CheckLabResultPlausibilityAsync(string dobString, string testCode, string resultValue, List<Tuple<string, string>> coordinateParams)
        {
            string stamp = coordinateParams.FirstOrDefault(p => p.Item1 == "STAMP_COORDINATE_IDENTIFIER")?.Item2 ?? "";
            string lang = coordinateParams.FirstOrDefault(p => p.Item1 == "LANGUAGE_COORDINATE_IDENTIFIER")?.Item2 ?? "";
            string nav = coordinateParams.FirstOrDefault(p => p.Item1 == "NAVIGATION_COORDINATE_IDENTIFIER")?.Item2 ?? "";

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/plausibility/lab-result?dob={dobString}&testCode={testCode}&resultValue={resultValue}&stamp={stamp}&lang={lang}&nav={nav}");
            var response = await Client.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> CheckLabDevicePlausibilityAsync(string testCode, string refRangeLow, string refRangeHigh, string unit, List<Tuple<string, string>> coordinateParams)
        {
            string stamp = coordinateParams.FirstOrDefault(p => p.Item1 == "STAMP_COORDINATE_IDENTIFIER")?.Item2 ?? "";
            string lang = coordinateParams.FirstOrDefault(p => p.Item1 == "LANGUAGE_COORDINATE_IDENTIFIER")?.Item2 ?? "";
            string nav = coordinateParams.FirstOrDefault(p => p.Item1 == "NAVIGATION_COORDINATE_IDENTIFIER")?.Item2 ?? "";

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/plausibility/lab-device?testCode={testCode}&refRangeLow={refRangeLow}&refRangeHigh={refRangeHigh}&unit={unit}&stamp={stamp}&lang={lang}&nav={nav}");
            var response = await Client.SendAsync(request);
            return response;
        }
    }
}