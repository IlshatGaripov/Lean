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

namespace QuantConnect.Brokerages.Binance.Enums
{
    /// <summary>
    /// The status of an order
    /// </summary>
    public enum BinanceOrderStatus
    {
        /// <summary>
        /// Order is new
        /// </summary>
        New,

        /// <summary>
        /// Order is partly filled, still has quantity left to fill
        /// </summary>
        PartiallyFilled,

        /// <summary>
        /// The order has been filled and completed
        /// </summary>
        Filled,

        /// <summary>
        /// The order has been canceled
        /// </summary>
        Canceled,

        /// <summary>
        /// The order is in the process of being canceled  (currently unused)
        /// </summary>
        PendingCancel,

        /// <summary>
        /// The order has been rejected
        /// </summary>
        Rejected,

        /// <summary>
        /// The order has expired
        /// </summary>
        Expired,

        /// <summary>
        /// Liquidation with Insurance Fund
        /// </summary>
        Insurance,

        /// <summary>
        /// Counterparty Liquidation
        /// </summary>
        Adl
    }
}
