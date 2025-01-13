using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.BookEndpoints;

public record GetBookByIdRequest(Guid Id);

public class GetBookByIdValidator : Validator<GetBookByIdRequest>
{
    public GetBookByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}