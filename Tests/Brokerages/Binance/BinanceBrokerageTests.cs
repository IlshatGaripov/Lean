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

using System;
using System.Linq;
using QuantConnect.Interfaces;
using QuantConnect.Securities;
using NUnit.Framework;
using QuantConnect.Brokerages.Binance;
using QuantConnect.Configuration;
using Moq;
using QuantConnect.Brokerages;
using QuantConnect.Tests.Common.Securities;
using QuantConnect.Orders;
using QuantConnect.Logging;
using System.Threading;
using QuantConnect.Lean.Engine.DataFeeds;
using QuantConnect.Util;

namespace QuantConnect.Tests.Brokerages.Binance
{
    [TestFixture, Explicit("This test requires a configured and testable Binance practice account")]
    public partial class BinanceBrokerageTests : BrokerageTests
    {
        private const string Pair = "ETHUSDT";
        private const AccountType BinanceAccountType = AccountType.Margin;

        private decimal _highPrice, _lowPrice;
        private decimal _minLot;
        private static readonly Symbol _StaticSymbol = Symbol.Create(Pair, SecurityType.Crypto, Market.Binance);
        private BinanceRestApiClient _binanceApi;
        private Mock<IAlgorithm> _algorithm;
        private OrderTestParameters[] _orderTestParameters;

        /// <summary>
        /// Gets Binance symbol mapper
        /// </summary>
        protected ISymbolMapper SymbolMapper => new SymbolPropertiesDatabaseSymbolMapper(Market.Binance);

        /// <summary>
        /// Gets the symbol to be traded, must be shortable
        /// </summary>
        protected override Symbol Symbol => _StaticSymbol;

        /// <summary>
        /// Gets the security type associated with the <see cref="BrokerageTests.Symbol" />
        /// </summary>
        protected override SecurityType SecurityType => SecurityType.Crypto;

        /// <summary>
        /// Gets the default order quantity.
        /// </summary>
        protected override decimal GetDefaultQuantity() => _minLot;

        /// <summary>
        /// Returns wether or not the brokers order methods implementation are async
        /// </summary>
        protected override bool IsAsync() => false;


        [SetUp]
        public void SetUpBinanceBrokerageTests()
        {
            // Set up brokerage account type
            var binanceBrokerage = (BinanceBrokerage)Brokerage;
            binanceBrokerage.AccountType = BinanceAccountType;

            // Call api to get recent prices
            var price = _binanceApi.GetTickers().FirstOrDefault(x => x.Symbol == Pair)?.Price;
            if (price != null)
            {
                // Min order amount should be no less than 10 USDT
                _minLot = Math.Round((decimal) (15 / price), 4);
                _highPrice = Math.Round( 1.01m * price.Value, 2);
                _lowPrice = Math.Round( 0.99m * price.Value, 2);
            }

            _orderTestParameters = new OrderTestParameters[]
            {
                new MarketOrderTestParameters(_StaticSymbol),
                new LimitOrderTestParameters(_StaticSymbol, _highPrice, _lowPrice),
                new StopLimitOrderTestParameters(_StaticSymbol, _highPrice, _lowPrice)
            };
        }

        /// <summary>
        /// Creates the brokerage under test and connects it
        /// </summary>
        /// <param name="orderProvider"></param>
        /// <param name="securityProvider"></param>
        /// <returns></returns>
        protected override IBrokerage CreateBrokerage(IOrderProvider orderProvider, ISecurityProvider securityProvider)
        {
            var securities = new SecurityManager(new TimeKeeper(DateTime.UtcNow, TimeZones.NewYork))
            {
                { Symbol, CreateSecurity(Symbol) }
            };

            var transactions = new SecurityTransactionManager(null, securities);
            transactions.SetOrderProcessor(new FakeOrderProcessor());

            _algorithm = new Mock<IAlgorithm>();
            _algorithm.Setup(a => a.Transactions).Returns(transactions);
            _algorithm.Setup(a => a.BrokerageModel).Returns(new BinanceBrokerageModel());
            _algorithm.Setup(a => a.Portfolio).Returns(new SecurityPortfolioManager(securities, transactions));

            var apiKey = Config.Get("binance-api-key");
            var apiSecret = Config.Get("binance-api-secret");

            _binanceApi = new BinanceRestApiClient(
                new SymbolPropertiesDatabaseSymbolMapper(Market.Binance),
                _algorithm.Object?.Portfolio,
                apiKey,
                apiSecret);

            return new BinanceBrokerage(
                    apiKey,
                    apiSecret,
                    _algorithm.Object,
                    new AggregationManager()
                );
        }

        /// <summary>
        /// Gets the current market price of the specified security
        /// </summary>
        protected override decimal GetAskPrice(Symbol symbol)
        {
            var brokerageSymbol = SymbolMapper.GetBrokerageSymbol(symbol);
            var prices = _binanceApi.GetTickers();
            return prices
                .FirstOrDefault(t => t.Symbol == brokerageSymbol)
                .Price;
        }

        [Test]
        public void CancelOrders()
        {
            _orderTestParameters.DoForEach(base.CancelOrders);
        }

        [Test]
        public void LongFromZero()
        {
            _orderTestParameters.DoForEach(base.LongFromZero);
        }

        [Test]
        public void CloseFromLong()
        {
            _orderTestParameters.DoForEach(base.CloseFromLong);
        }

        [Test]
        public void ShortFromZero()
        {
            _orderTestParameters.DoForEach(base.ShortFromZero);
        }

        [Test]
        public void CloseFromShort()
        {
            _orderTestParameters.DoForEach(base.CloseFromShort);
        }

        [Test]
        public void ShortFromLong()
        {
            _orderTestParameters.DoForEach(base.ShortFromLong);
        }

        [Test]
        public void LongFromShort()
        {
            _orderTestParameters.DoForEach(base.LongFromShort);
        }

        [Test, Ignore("Holdings are now set to 0 swaps at the start of each launch. Not meaningful.")]
        public override void GetAccountHoldings()
        {
            Log.Trace("");
            Log.Trace("GET ACCOUNT HOLDINGS");
            Log.Trace("");
            var before = Brokerage.GetAccountHoldings();
            Assert.AreEqual(0, before.Count());

            PlaceOrderWaitForStatus(new MarketOrder(Symbol, GetDefaultQuantity(), DateTime.Now));
            Thread.Sleep(3000);

            var after = Brokerage.GetAccountHoldings();
            Assert.AreEqual(0, after.Count());
        }

        [Test]
        public void CanPlaceOrderFromMarginAccount()
        {
            //var order = new LimitOrder(Symbol, -_minLot, _highPrice, DateTime.UtcNow);
            var order = new MarketOrder(Symbol, _minLot, DateTime.UtcNow);

            // Api call
            Brokerage.PlaceOrder(order);
        }
        
        protected override void ModifyOrderUntilFilled(Order order, OrderTestParameters parameters, double secondsTimeout = 90)
        {
            Assert.Pass("Order update not supported. Please cancel and re-create.");
        }
    }
}
