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
    /// Information about margin account
    /// </summary>
    public class BinanceMarginAccount
    {
        /// <summary>
        /// Boolean indicating if this account can borrow
        /// </summary>
        public bool BorrowEnabled { get; set; }

        /// <summary>
        /// Boolean indicating if this account can trade
        /// </summary>
        public bool TradeEnabled { get; set; }

        /// <summary>
        /// Boolean indicating if this account can transfer
        /// </summary>
        public bool TransferEnabled { get; set; }

        /// <summary>
        /// Aggregate level of margin
        /// </summary>
        public decimal MarginLevel { get; set; }

        /// <summary>
        /// Aggregate total balance as BTC
        /// </summary>
        public decimal TotalAssetOfBtc { get; set; }

        /// <summary>
        /// Aggregate total liability balance of BTC
        /// </summary>
        public decimal TotalLiabilityOfBtc { get; set; }

        /// <summary>
        /// Aggregate total available net balance of BTC
        /// </summary>
        public decimal TotalNetAssetOfBtc { get; set; }

        /// <summary>
        /// Balance list
        /// </summary>
        [JsonProperty("userAssets")]
        public IEnumerable<BinanceMarginBalance> Balances { get; set; } = new List<BinanceMarginBalance>();
    }
}
