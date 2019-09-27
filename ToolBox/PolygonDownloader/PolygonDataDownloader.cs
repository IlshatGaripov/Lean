using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using QuantConnect.Data;
using QuantConnect.Data.Market;
using QuantConnect.Securities;
using RestSharp;

namespace QuantConnect.ToolBox.PolygonDownloader
{
    /// <summary>
    /// Polygon API data downloader class
    /// </summary>
    public class PolygonDataDownloader
    {
        // Readonly variables (initialized in constructor)
        private readonly string _apiKey;
        private readonly RestClient _restClient;
        private readonly MarketHoursDatabase _mhdb;

        // Polygon API constants
        private const string RestBaseUrl = "https://api.polygon.io";
        private const int MaxResponseSizeEquity = 50000;
        private const int MaxResponseSizeForex = 10000;
    

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonDataDownloader"/> class
        /// </summary>
        /// <param name="apiKey">Api key to authorize endpoint requests</param>
        public PolygonDataDownloader(string apiKey)
        {
            _apiKey = apiKey;
            _restClient = new RestClient(RestBaseUrl);

            // market hours database is in Globals.DataFolder
            _mhdb = MarketHoursDatabase.FromDataFolder();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // .NET 4.5 doesn't use TLS 1.2 by default we have to enable it explicitly:
            // https://stackoverflow.com/questions/49885842/c-sharp-web-request-w-restsharp-the-request-was-aborted-could-not-create-ssl
        }

        /// <summary>
        /// (v1) Get historical ticks enumerable for a single equity symbol on a single day.
        /// </summary>
        /// <remarks>Uses REST API v1</remarks>
        /// <param name="symbol">Symbol for the data</param>
        /// <param name="timeUtc">Start time of the data in UTC</param>
        /// <returns>Enumerable of base data for this symbol</returns>
        public IEnumerable<BaseData> DownloadHistoricEquityTradesV1(Symbol symbol, DateTime timeUtc)
        {
            // we are trying to fetch as much ticks as possible per every request
            // offset is used to shift to the next page of results
            var ticksCount = MaxResponseSizeEquity;
            long offset = 0;

            // API returns equity ticks Unix timestamped,
            // we need to convert them to the time zone 
            // in which Lean expects to see it
            var dataTimeZone = _mhdb.GetDataTimeZone(symbol.ID.Market, symbol, symbol.SecurityType);

            // symbol must be converted to Upper otherwise API will return a null array of ticks
            var symbolToUpper = symbol.Value.LazyToUpper();
            var dateFormatted = timeUtc.ToStringInvariant("yyyy-M-d");

            // continue as long as we are getting the max number of ticks per response
            while (ticksCount == MaxResponseSizeEquity)
            {
                var endpoint =
                    $"v1/historic/trades/{symbolToUpper}/{dateFormatted}" +
                    $"?offset={offset}&limit={MaxResponseSizeEquity}&apiKey={_apiKey}";

                // execute request
                var request = new RestRequest(endpoint, Method.GET);
                var response = _restClient.Execute(request);

                // deserialize response string to an object and get ticks
                var responseObject = JsonConvert.DeserializeObject<EquityHistoricTradesV1>(response.Content);
                var historicTrades = responseObject.Ticks;

                // break if current page contains no ticks - unlikely but possible scenario
                if (historicTrades == null) break;

                // register amount of ticks received
                ticksCount = historicTrades.Length;

                foreach (var trade in historicTrades)
                {
                    // timestamp offset, used for pagination. see polygon.io documentation.
                    offset = trade.T;
                    // convert time from utc to data time zone
                    var unixTime = Time.UnixMillisecondTimeStampToDateTime(trade.T);
                    var dataTimeZoneTime = unixTime.ConvertFromUtc(dataTimeZone);

                    yield return new Tick
                    {
                        // map id {Int32} to the tape string literal
                        Exchange = Mapping.IdToTape[trade.E],    
                        Value = trade.P,
                        Quantity = trade.S,
                        Time = dataTimeZoneTime,
                        // API returns multiple trade conditions, which one to choose? 
                        SaleCondition = ""
                    };
                }
            }
        }

        /// <summary>
        /// (v2) Get historical ticks enumerable for a single equity symbol on a single day.
        /// </summary>
        /// <remarks>Uses REST API v2</remarks>
        /// <param name="symbol">Symbol for the data</param>
        /// <param name="timeUtc">Start time of the data in UTC</param>
        /// <returns>Enumerable of base data for this symbol</returns>
        public IEnumerable<BaseData> DownloadHistoricEquityTradesV2(Symbol symbol, DateTime timeUtc)
        {
            // we are trying to fetch as much ticks as possible per every request
            // offset is used to shift to the next page of results
            var ticksCount = MaxResponseSizeEquity;
            long offset = 0;

            // API returns equity ticks Unix timestamped,
            // we need to convert them to the time zone 
            // in which Lean expects to see it
            var dataTimeZone = _mhdb.GetDataTimeZone(symbol.ID.Market, symbol, symbol.SecurityType);

            // symbol must be converted to Upper otherwise API will return a null array of ticks
            var symbolToUpper = symbol.Value.LazyToUpper();

            // date farmatting is differet from from v1 (!)
            var dateFormatted = timeUtc.ToStringInvariant("yyyy-MM-dd");

            // continue as long as we are getting the max number of ticks per response
            while (ticksCount == MaxResponseSizeEquity)
            {
                var endpoint =
                    $"v2/ticks/stocks/trades/{symbolToUpper}/{dateFormatted}" +
                    $"?offset={offset}&limit={MaxResponseSizeEquity}&apiKey={_apiKey}";

                // execute request
                var request = new RestRequest(endpoint, Method.GET);
                var response = _restClient.Execute(request);

                // deserialize response string to an object and get ticks
                var responseObject = JsonConvert.DeserializeObject<EquityHistoricTradesV2>(response.Content);
                var historicTrades = responseObject.Results;

                // break if current page contains no ticks - unlikely but possible scenario
                if (historicTrades == null) break;

                // register amount of ticks received
                ticksCount = historicTrades.Length;

                foreach (var trade in historicTrades)
                {
                    // timestamp offset, used for pagination. see polygon.io documentation.
                    offset = trade.NanoSipTs;

                    /* todo:
                     below is a sketch - to make the things work for better precision we need to improve tick object
                     to store and read from csv. the time as DateTime.Ticks count - currently time is stored as ms count
                    // convert nanoseconds to datetime;
                    // for this truncate nano-timestamp to 100 nanoseconds precision and convert to datetime
                    // finally convert time from utc to previously defined data time zone
                    var epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var unixTime = epochTime.AddTicks(trade.NanoSipTs / 100);
                    var dataTimeZoneTime = unixTime.ConvertFromUtc(dataTimeZone);
                    */

                    // truncate nanoseconds to milliseconds; as this will be rounded later when reading the data anyway
                    var msTimestampFromNano = trade.NanoSipTs / 1000000;
                    var unixTime = Time.UnixMillisecondTimeStampToDateTime(msTimestampFromNano);
                    var dataTimeZoneTime = unixTime.ConvertFromUtc(dataTimeZone);

                    yield return new Tick
                    {
                        // map id {Int32} to the tape string literal
                        Exchange = Mapping.IdToTape[trade.Exchange],
                        Value = trade.Price,
                        Quantity = trade.Size,
                        Time = dataTimeZoneTime,
                        
                        // API returns multiple trade conditions, which one we choose? 
                        SaleCondition = ""
                    };
                }

            }
        }

        /// <summary>
        /// Get historical dividends and splits for the symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IEnumerable<BaseData> DownloadStockDividendsAndSplits(Symbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
