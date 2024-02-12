using StackExchange.Redis;
using System;
using WeatherApi.Settings;
using Microsoft.Extensions.Options;

namespace WeatherApi.Helper {
    public class ConnectionHelper: IConnectionHelper {
        private readonly IOptions<RedisSettings> _options;
        private Lazy <ConnectionMultiplexer> lazyConnection;
        public ConnectionHelper(IOptions<RedisSettings> options) {
            _options = options;
        }
        public  ConnectionMultiplexer getConnection() {
                lazyConnection = new Lazy <ConnectionMultiplexer> (() => {
                return ConnectionMultiplexer.Connect( _options.Value.RediusUrl);
            });
            return lazyConnection.Value;
        }
    }
}