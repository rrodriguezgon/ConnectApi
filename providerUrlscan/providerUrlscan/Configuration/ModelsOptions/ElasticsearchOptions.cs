using Serilog.Events;

namespace providerUrlscan.Configuration.ModelsOptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ElasticsearchOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string UriString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LogEventLevel? MinimumLogEventLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoRegisterTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IndexFormat { get; set; }
    }
}
