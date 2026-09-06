using System.Net;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Presentation.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Constants = DevSu.Bank.Customers.Common.Constants;

namespace DevSu.Bank.Customers.Presentation.Api.UnitTest.Middleware
{
    public class ErrorHandlerMiddlewareFacts
    {
        private readonly Mock<RequestDelegate> _requestDelegate = new();
        private readonly Mock<ILogger<ErrorHandlerMiddleware>> _logger = new();

        [Fact]
        public async Task Test_InvokeAsync_Success()
        {
            //Arrange 
            _requestDelegate.Setup(requestDelegate => requestDelegate.Invoke(It.IsAny<HttpContext>()))
                .Returns(Task.CompletedTask);

            var errorHandlerMiddleware = new ErrorHandlerMiddleware(_requestDelegate.Object, _logger.Object);

            //Act
            var result = errorHandlerMiddleware.InvokeAsync(It.IsAny<HttpContext>());
            await result;

            //Assert
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public async Task Test_InvokeAsync_ThrowException_ReturnInternalServerError_Success()
        {
            //Arrange 
            var httpRequest = new DefaultHttpContext();
            var expectedHttpStatusCode = (int)HttpStatusCode.InternalServerError;
            _requestDelegate.Setup(requestDelegate => requestDelegate.Invoke(It.IsAny<HttpContext>()))
                .Throws(new Exception("Error"));

            var errorHandlerMiddleware = new ErrorHandlerMiddleware(_requestDelegate.Object, _logger.Object);

            //Act
            await errorHandlerMiddleware.InvokeAsync(httpRequest);

            //Assert
            Assert.Equal(expectedHttpStatusCode, httpRequest.Response.StatusCode);
            Assert.Equal(Constants.MimeTypes.Json, httpRequest.Response.ContentType);
        }

        [Fact]
        public async Task Test_InvokeAsync_ThrowDomainValidationException_ReturnBadRequest_Success()
        {
            //Arrange 
            var httpRequest = new DefaultHttpContext();
            var expectedHttpStatusCode = (int)HttpStatusCode.BadRequest;
            _requestDelegate.Setup(requestDelegate => requestDelegate.Invoke(It.IsAny<HttpContext>()))
                .Throws(new DomainValidationException("Error"));

            var errorHandlerMiddleware = new ErrorHandlerMiddleware(_requestDelegate.Object, _logger.Object);

            //Act
            await errorHandlerMiddleware.InvokeAsync(httpRequest);

            //Assert
            Assert.Equal(expectedHttpStatusCode, httpRequest.Response.StatusCode);
            Assert.Equal(Constants.MimeTypes.Json, httpRequest.Response.ContentType);
        }

        [Fact]
        public async Task Test_InvokeAsync_ThrowNotFoundException_ReturnNotFound_Success()
        {
            //Arrange 
            var httpRequest = new DefaultHttpContext();
            var expectedHttpStatusCode = (int)HttpStatusCode.NotFound;
            _requestDelegate.Setup(requestDelegate => requestDelegate.Invoke(It.IsAny<HttpContext>()))
                .Throws(new NotFoundException("Error"));

            var errorHandlerMiddleware = new ErrorHandlerMiddleware(_requestDelegate.Object, _logger.Object);

            //Act
            await errorHandlerMiddleware.InvokeAsync(httpRequest);

            //Assert
            Assert.Equal(expectedHttpStatusCode, httpRequest.Response.StatusCode);
            Assert.Equal(Constants.MimeTypes.Json, httpRequest.Response.ContentType);
        }

        [Fact]
        public async Task Test_InvokeAsync_ThrowApplicationValidationException_ReturnBadRequest_Success()
        {
            //Arrange 
            var httpRequest = new DefaultHttpContext();
            var expectedHttpStatusCode = (int)HttpStatusCode.BadRequest;
            _requestDelegate.Setup(requestDelegate => requestDelegate.Invoke(It.IsAny<HttpContext>()))
                .Throws(new ApplicationValidationException("Error"));

            var errorHandlerMiddleware = new ErrorHandlerMiddleware(_requestDelegate.Object, _logger.Object);

            //Act
            await errorHandlerMiddleware.InvokeAsync(httpRequest);

            //Assert
            Assert.Equal(expectedHttpStatusCode, httpRequest.Response.StatusCode);
            Assert.Equal(Constants.MimeTypes.Json, httpRequest.Response.ContentType);
        }
    }
}
