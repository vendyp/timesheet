using Timesheet.WebApi.Contracts.Requests;

namespace Timesheet.UnitTests.Validators;

public class SignInValidatorTests
{
    public static IEnumerable<object[]> GetInvalidRequests =>
        new List<object[]>
        {
            //all null
            new object[]
            {
                new SignInRequest
                {
                    Username = null,
                    Password = null
                }
            },

            new object[]
            {
                new SignInRequest
                {
                    Username = "用户名",
                    Password = "Request@1234"
                }
            },

            new object[]
            {
                new SignInRequest
                {
                    Username = new string('a', 101),
                    Password = "Request@1234"
                }
            }
        };

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void SignInvalidator_IsValid_Should_Be_False(SignInRequest request)
    {
        var validator = new SignInRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void SignInValidator_IsValid_Should_Be_True()
    {
        var request = new SignInRequest
        {
            Username = "admin",
            Password = "Admin@1234"
        };
        
        var validator = new SignInRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeTrue();
    }
}