using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Auth.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .Length(0, 255);
        RuleFor(user => user.Password)
            .NotEmpty();
        RuleFor(user => user.Username)
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await unitOfWork.Users.AnyAsync(user => user.Username == username);
            }).WithMessage(AuthValidationMessages.Occupied);
    }
} 