using Timesheet.WebApi.Contracts.Requests;
using Xunit.Abstractions;

namespace Timesheet.UnitTests.Validators;

public class ChangePasswordRequestValidatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ChangePasswordRequestValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> GetInvalidRequests =>
        new List<object[]>
        {
            //all null
            new object[]
            {
                new ChangePasswordRequest
                {
                    CurrentPassword = null,
                    NewPassword = null,
                }
            },

            //not same
            new object[]
            {
                new ChangePasswordRequest
                {
                    CurrentPassword = "abcde",
                    NewPassword = "abcde",
                }
            }
        };

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ChangePasswordRequestValidator_IsValid_Should_Be_False(ChangePasswordRequest request)
    {
        var validator = new ChangePasswordRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void ChangePasswordRequestValidator_IsValid_Should_Be_True()
    {
        const string pass = "hahahahahahahaha";
        const string pass2 = "hahahahahahahahahahaha";

        var request = new ChangePasswordRequest();
        request.CurrentPassword = pass;
        request.NewPassword = pass2;

        var validator = new ChangePasswordRequestValidator();
        var validationResult = validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            foreach (var item in validationResult.Errors)
            {
                var msg = $"Prop: {item.PropertyName}, Msg: {item.ErrorMessage}";
                _testOutputHelper.WriteLine(msg);
            }
        }

        validationResult.IsValid.ShouldBeTrue();
    }
}