using Challenge.Models;
using Challenge.QueueMessage;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Challenge.UT.QueueMessage
{
    [TestClass]
    public class ReceiverShould
    {
        public Mock<IHubContext<ChatHub.ChatHub>> _HubContext;
        public Receiver _Receiver;

        [TestInitialize]
        public void TestInitialize()
        {
            _HubContext = new Mock<IHubContext<ChatHub.ChatHub>>();
            _Receiver = new Receiver(_HubContext.Object);
        }

        [TestMethod]
        public void Return_ErrorMessage()
        {
            var stockQuoteResponse = new StockQuoteResponse();
            var strErrorMessage = "This is an Error Message";
            stockQuoteResponse.ErrorMessage = strErrorMessage;
            var response = _Receiver.GetPostMessage(stockQuoteResponse);
            Assert.AreEqual(response, strErrorMessage);
        }

        [TestMethod]
        public void Return_StockQuote()
        {
            var stockQuoteResponse = new StockQuoteResponse();
            stockQuoteResponse.Open = 12.23;
            stockQuoteResponse.Symbol = "aapl.eur";
            var expectedResponse = $"{stockQuoteResponse.Symbol} quote is ${stockQuoteResponse.Open} per share";
            var response = _Receiver.GetPostMessage(stockQuoteResponse);
            Assert.AreEqual(response, expectedResponse);
        }
    }
}
