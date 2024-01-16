using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Moq;

namespace RawCookieAuthentication.Tests;

public class AuthServiceTests
{
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly IHttpContextAccessor _accessor;

    public AuthServiceTests()
    {
        // Arrange
        var dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
        var dataProtectorMock = new Mock<IDataProtector>();
        dataProtectionProviderMock.Setup(provider => provider.CreateProtector(It.IsAny<string>())).Returns(dataProtectorMock.Object);
        _dataProtectionProvider  = dataProtectionProviderMock.Object;
        
        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.SetupGet(accessor => accessor.HttpContext).Returns(new DefaultHttpContext());
        _accessor = accessorMock.Object;
    }

    [Fact]
    public void SignIn_SetsAuthCookieInResponseHeaders()
    {
        // Arrange
        var authService = new AuthService(_dataProtectionProvider, _accessor);
        
        // Act
        authService.SignIn();

        // Assert
        Assert.NotNull(_accessor.HttpContext);
        Assert.NotEmpty(_accessor.HttpContext.Response.Headers.SetCookie);
    }
    
    
    [Fact]
    public void SignOut_SetsAuthCookieInResponseHeaders()
    {
        // Arrange
        var authService = new AuthService(_dataProtectionProvider, _accessor);
        
        // Act
        authService.SignOut();

        // Assert
        Assert.NotNull(_accessor.HttpContext);
        Assert.NotEmpty(_accessor.HttpContext.Response.Headers.SetCookie);
    }
}