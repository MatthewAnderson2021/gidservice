using GudelIdService.Domain.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace GudelIdService.Implementation.Services
{
    public class ConfigService : IConfigService
    {
        private IConfiguration _config;
        private IConverterServiceFactory<int> _converterFactory;

        private const string CREATE_CRON_INTERVAL = nameof(CREATE_CRON_INTERVAL);
        private const string CREATE_CRON_AMOUNT = nameof(CREATE_CRON_AMOUNT);
        private const string BODY_PARSE_LIMIT = nameof(BODY_PARSE_LIMIT);
        private const string PORT = nameof(PORT);
        public const string MYSQLCONNSTR_DEFAULT = nameof(MYSQLCONNSTR_DEFAULT);
        public const string LANG_DEFAULT = "de-DE";
        public static readonly string[] KNOWN_LANGS = { "de-DE", "en-US" };

        public ConfigService(IConfiguration config, IConverterServiceFactory<int> converterFactory)
        {
            _config = config;
            _converterFactory = converterFactory;
        }

        public string Get(string key) => Environment.GetEnvironmentVariable(key) ?? _config[key];

        public int HttpPort() =>
            _converterFactory
                .GetConverter()
                .WithDefaultValue(3003)
                .WithConvertAction(() => int.Parse(Environment.GetEnvironmentVariable(PORT) ?? _config[PORT]))
                .Execute();

        public int CreateCronInterval() => int.Parse(Environment.GetEnvironmentVariable(CREATE_CRON_INTERVAL) ?? Get(CREATE_CRON_INTERVAL));

        public int CreateCronAmount() => int.Parse(Environment.GetEnvironmentVariable(CREATE_CRON_AMOUNT) ?? Get(CREATE_CRON_AMOUNT));

        public int MaxBodyParseLimit() => int.Parse(Environment.GetEnvironmentVariable(BODY_PARSE_LIMIT) ?? Get(BODY_PARSE_LIMIT));
    }
}