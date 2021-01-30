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
using QuantConnect.Brokerages.Binance.Enums;

namespace QuantConnect.Brokerages.Binance.Messages
{
    /// <summary>
    /// Information about an account
    /// </summary>
    public class BinanceSpotAccount
    {
        /// <summary>
        /// Commission percentage to pay when making trades
        /// </summary>
        public decimal MakerCommission { get; set; }

        /// <summary>
        /// Commission percentage to pay when taking trades
        /// </summary>
        public decimal TakerCommission { get; set; }

        /// <summary>
        /// Commission percentage to buy when buying
        /// </summary>
        public decimal BuyerCommission { get; set; }

        /// <summary>
        /// Commission percentage to buy when selling
        /// </summary>
        public decimal SellerCommission { get; set; }

        /// <summary>
        /// Boolean indicating if this account can trade
        /// </summary>
        public bool CanTrade { get; set; }

        /// <summary>
        /// Boolean indicating if this account can withdraw
        /// </summary>
        public bool CanWithdraw { get; set; }

        /// <summary>
        /// Boolean indicating if this account can deposit
        /// </summary>
        public bool CanDeposit { get; set; }

        /// <summary>
        /// The unix timestamp of the update
        /// </summary>
        public long UpdateTime { get; set; }

        /// <summary>
        /// The type of account
        /// </summary>
        public BinanceAccountType AccountType { get; set; }

        /// <summary>
        /// Permissions types
        /// </summary>
        public IEnumerable<BinanceAccountType> Permissions { get; set; } = new List<BinanceAccountType>();

        /// <summary>
        /// List of assets with their current balances
        /// </summary>
        public IEnumerable<BinanceSpotBalance> Balances { get; set; } = new List<BinanceSpotBalance>();
    }
}
