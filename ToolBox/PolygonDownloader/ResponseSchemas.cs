using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuantConnect.ToolBox.PolygonDownloader
{
    /// <summary>
    /// Response structrue received from 'Historic Trades' endpoint ( v1 )
    /// https://polygon.io/docs/#!/Stocks--Equities/get_v1_historic_trades_symbol_date
    /// </summary>
    [Serializable]
    public class EquityHistoricTradesV1
    {
        /// <summary>
        /// Date that was evaluated from the request
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        /// Map for shortened result keys <see cref="StocksV1Trade"/>
        /// </summary>
        public JToken Map { get; set; }

        /// <summary>
        /// Milliseconds of latency for the query results from DB
        /// </summary>
        public int Latency { get; set; }

        /// <summary>
        /// Status of this requests response
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Symbol that was evaluated from the request
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Historic trades array
        /// </summary>
        public StocksV1Trade[] Ticks { get; set; }

        /// <summary>
        /// Ticks type - 'trades'
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// A trade structure returned as part of response from 'Historic Trades' or "Last Trade for a Symbol' endp. ( v1 )
    /// https://polygon.io/docs/#!/Stocks--Equities/get_v1_historic_trades_symbol_date
    /// https://polygon.io/docs/#!/Stocks--Equities/get_v1_last_stocks_symbol
    /// </summary>
    [Serializable]
    public class StocksV1Trade
    {
        /// <summary>
        /// Condition 1
        /// </summary>
        public int C1 { get; set; }

        /// <summary>
        /// Condition 2
        /// </summary>
        public int C2 { get; set; }

        /// <summary>
        /// Condition 3
        /// </summary>
        public int C3 { get; set; }

        /// <summary>
        /// Condition 4
        /// </summary>
        public int C4 { get; set; }

        /// <summary>
        /// Exchange
        /// </summary>
        public int E { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal P { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public int S { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public long T { get; set; }
    }

    /// <summary>
    /// Response structrue received from '(v2) Historic Trades' endpoint
    /// https://polygon.io/docs/#!/Stocks--Equities/get_v2_ticks_stocks_trades_ticker_date
    /// </summary>
    [Serializable]
    public class EquityHistoricTradesV2
    {
        /// <summary>
        /// Historic trades array
        /// </summary>
        [JsonProperty("results")]
        public StocksV2Trade[] Results { get; set; }

        /// <summary>
        /// If this query was executed successfully
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Map for shortened result keys <see cref="StocksV2Trade"/>
        /// </summary>
        [JsonProperty("map")]
        public JToken Map { get; set; }

        /// <summary>
        /// Ticker symbol that was evaluated from the request
        /// </summary>
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Total number of results in this response.
        /// </summary>
        [JsonProperty("results_count")]
        public int ResultsCount { get; set; }

        /// <summary>
        /// Milliseconds of latency for the query results from DB
        /// </summary>
        [JsonProperty("db_latency")]
        public int Latency { get; set; }
    }

    /// <summary>
    /// A structure of trade object returned from '(v2) Historic Trades' endp.
    /// https://polygon.io/docs/#!/Stocks--Equities/get_v2_ticks_stocks_trades_ticker_date
    /// </summary>
    [Serializable]
    public class StocksV2Trade
    {
        /// <summary>
        /// Nanosecond accuracy TRF(Trade Reporting Facility) Unix Timestamp
        /// </summary>
        [JsonProperty("f")]
        public long NanoTradRepFacTs { get; set; }

        /// <summary>
        /// Nanosecond accuracy Participant/Exchange Unix Timestamp
        /// </summary>
        [JsonProperty("y")]
        public long NanoParticExchTs { get; set; }

        /// <summary>
        /// Nanosecond accuracy SIP Unix Timestamp
        /// </summary>
        [JsonProperty("t")]
        public long NanoSipTs { get; set; }

        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonProperty("q")]
        public int SequenceNum { get; set; }

        /// <summary>
        /// Trade ID
        /// </summary>
        [JsonProperty("i")]
        public string TradeId { get; set; }

        /// <summary>
        /// Exchange ID
        /// </summary>
        [JsonProperty("x")]
        public int Exchange { get; set; }

        /// <summary>
        /// Size/Volume of the trade
        /// </summary>
        [JsonProperty("s")]
        public int Size { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("p")]
        public decimal Price { get; set; }

        /// <summary>
        /// Trade conditions ( see Mapping.TradeConditions )
        /// </summary>
        [JsonProperty("c")]
        public int[] Conditions { get; set; }

        /// <summary>
        /// Tape where trade occured. ( 1,2 = CTA, 3 = UTP )
        /// </summary>
        [JsonProperty("z")]
        public int WhichTape { get; set; }
    }

    /// <summary>
    /// Json structure of a single exchange record returned by API.
    /// </summary>
    [Serializable]
    public class ExchangeTexture
    {
        /// <summary>
        /// Exchange id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Exchange organization type -  exchange/ TRF.
        /// </summary>
        public string Infrastructure { get; set; }

        /// <summary>
        /// Market type - equities/ index/ etc.
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// Exchange Market Identifier Code
        /// </summary>
        public string Mic { get; set; }

        /// <summary>
        /// Full name of exchange
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Very short - one letter - denomination of the exchange
        /// </summary>
        public string Tape { get; set; }

        /// <summary>
        /// Exchange code
        /// </summary>
        public string Code { get; set; }
    }
}
