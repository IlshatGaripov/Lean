/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuantConnect.Brokerages.Binance.Messages
{
    /// <summary>
    /// The result of placing a new order
    /// </summary>
    public class BinancePlacedOrder
    {
        /// <summary>
        /// The symbol the order is for
        /// </summary>
        public string Symbol { get; set; } = "";

        /// <summary>
        /// The order id as assigned by Binance
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Id of the order list this order belongs to
        /// </summary>
        public long OrderListId { get; set; }

        /// <summary>
        /// The order id as assigned by the client
        /// </summary>
        public string ClientOrderId { get; set; } = "";

        /// <summary>
        /// Original order id
        /// </summary>
        [JsonProperty("origClientOrderId")]
        public string OriginalClientOrderId { get; set; } = "";

        /// <summary>
        /// The time the order was placed
        /// </summary>
        [JsonProperty("transactTime")]
        public long CreateTimeStamp { get; set; }

        /// <summary>
        /// The price of the order
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The original quantity of the order
        /// </summary>
        [JsonProperty("origQty")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The quantity of the order that is executed
        /// </summary>
        [JsonProperty("executedQty")]
        public decimal QuantityFilled { get; set; }

        /// <summary>
        /// Cummulative amount
        /// </summary>
        [JsonProperty("cummulativeQuoteQty")]
        public decimal QuoteQuantityFilled { get; set; }

        /// <summary>
        /// The original quote order quantity
        /// </summary>
        [JsonProperty("origQuoteOrderQty")]
        public decimal QuoteQuantity { get; set; }

        /// <summary>
        /// The current status of the order
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// For what time the order lasts
        /// </summary>
        public string TimeInForce { get; set; }

        /// <summary>
        /// The type of the order
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The side of the order
        /// </summary>
        public string Side { get; set; }
        
        /// <summary>
        /// Fills for the order
        /// </summary>
        public IEnumerable<BinanceOrderTrade> Fills { get; set; }

        /// <summary>
        /// Stop price for the order
        /// </summary>
        public decimal StopPrice { get; set; }

        /// <summary>
        /// Only present if a margin trade happened
        /// </summary>
        /// 
        public decimal MarginBuyBorrowAmount { get; set; }

        /// <summary>
        /// Only present if a margin trade happened
        /// </summary>
        public string MarginBuyBorrowAsset { get; set; }
    }
}
