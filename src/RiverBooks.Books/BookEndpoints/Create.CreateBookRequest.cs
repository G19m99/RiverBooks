using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.BookEndpoints;

public record CreateBookRequest(Guid? Id, string Title, string Author, decimal Price);

public class CreateBookValidator : Validator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Title field required");


        RuleFor(x => x.Author)
            .NotNull()
            .NotEmpty()
            .WithMessage("Author field required");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Book Price may not be negative");
    }
}
