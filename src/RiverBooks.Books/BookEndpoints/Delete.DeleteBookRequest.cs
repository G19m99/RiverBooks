using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.BookEndpoints;

public record DeleteBookRequest(Guid Id);

public class DeleteBookValidator : Validator<DeleteBookRequest>
{
    public DeleteBookValidator()
    {
        RuleFor(x => x.Id)
           .NotNull()
           .NotEmpty()
           .NotEqual(Guid.Empty);
    }
}
