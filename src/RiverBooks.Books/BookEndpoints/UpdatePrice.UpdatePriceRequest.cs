using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.BookEndpoints;

public record UpdatePriceRequest(Guid Id, decimal NewPrice);

public class UpdateBookPriceRequestValidatior : Validator<UpdatePriceRequest>
{
    public UpdateBookPriceRequestValidatior()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Book id required");

        RuleFor(x => x.NewPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Book price may not be negative");
    }
}
