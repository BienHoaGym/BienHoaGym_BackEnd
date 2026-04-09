using FluentValidation.TestHelper;
using Gym.Application.Validators;
using Gym.Application.Validators.Subscriptions;
using Gym.Application.DTOs.Members;
using Gym.Application.DTOs.Subscriptions;
using Xunit;

namespace BienHoaGym.Tests;

public class ValidatorTests
{
    private readonly CreateMemberValidator _memberValidator;
    private readonly CreateSubscriptionValidator _subscriptionValidator;

    public ValidatorTests()
    {
        _memberValidator = new CreateMemberValidator();
        _subscriptionValidator = new CreateSubscriptionValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateMemberValidator_WhenFullNameIsEmpty_ShouldHaveError(string name)
    {
        var model = new CreateMemberDto { FullName = name };
        var result = _memberValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    public void CreateMemberValidator_WhenEmailIsInvalid_ShouldHaveError(string email)
    {
        var model = new CreateMemberDto { Email = email };
        var result = _memberValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void CreateMemberValidator_WhenPhoneNumberIsNot10Digits_ShouldHaveError()
    {
        var model = new CreateMemberDto { PhoneNumber = "123456789" }; // 9 digits
        var result = _memberValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public async Task CreateMemberValidator_WhenMemberIsUnder12_ShouldHaveError()
    {
        var model = new CreateMemberDto { DateOfBirth = DateTime.UtcNow.AddYears(-10) };
        var result = _memberValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }
}
