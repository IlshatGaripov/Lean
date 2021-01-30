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

namespace QuantConnect.Brokerages.Binance.Messages
{
    /// <summary>
    /// Information about an asset balance
    /// </summary>
    public class BinanceSpotBalance
    {
        /// <summary>
        /// The asset this balance is for
        /// </summary>
        public string Asset { get; set; } = "";

        /// <summary>
        /// The amount that isn't locked in a trade
        /// </summary>
        public decimal Free { get; set; }

        /// <summary>
        /// The amount that is currently locked in a trade
        /// </summary>
        public decimal Locked { get; set; }

        /// <summary>
        /// The total balance of this asset (Free + Locked)
        /// </summary>
        public decimal Total => Free + Locked;
    }
}
