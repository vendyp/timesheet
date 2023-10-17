using FluentValidation;

namespace TimesheetService.WebApi.Validators;

public class AsciiOnlyValidator : AbstractValidator<string>
{
    public AsciiOnlyValidator()
    {
        RuleFor(x => x)
            .Must(AsciiOnly)
            .WithMessage("The string must contain only non-unicode characters.");
    }

    private static bool AsciiOnly(string value)
    {
        // ascii and extended ascii chars
        // https://theasciicode.com.ar/
        return value.All(ch => ch < 256);
    }
}