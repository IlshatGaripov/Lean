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

using Newtonsoft.Json;

namespace QuantConnect.Brokerages.Binance.Messages
{
    /// <summary>
    /// Trade info
    /// </summary>
    public class BinanceOrderTrade
    {
        /// <summary>
        /// The id of the trade
        /// </summary>
        public long TradeId { get; set; }

        /// <summary>
        /// Price of the trade
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity of the trade
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Commission paid over this trade
        /// </summary>
        public decimal Commission { get; set; }

        /// <summary>
        /// The asset the commission is paid in
        /// </summary>
        public string CommissionAsset { get; set; } = "";
    }
}
