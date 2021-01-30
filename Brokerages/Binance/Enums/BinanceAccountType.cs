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
    /// Type of account
    /// </summary>
    public enum BinanceAccountType
    {
        /// <summary>
        /// Spot account type
        /// </summary>
        Spot,

        /// <summary>
        /// Margin account type
        /// </summary>>
        Margin,

        /// <summary>
        /// Futures account type
        /// </summary>
        Futures,

        /// <summary>
        /// Leveraged account type
        /// </summary>
        Leveraged
    }
}
