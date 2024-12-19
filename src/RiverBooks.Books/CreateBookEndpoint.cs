﻿using FastEndpoints;

namespace RiverBooks.Books;
internal class CreateBookEndpoint(IBookeService bookService) : Endpoint<CreateBookRequest, BookDto>
{
    private readonly IBookeService _bookService = bookService;

    public override void Configure()
    {
        Post("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
    {
        BookDto newBookDto = new(req.Id ?? Guid.NewGuid(), req.Title, req.Author, req.Price);

        await _bookService.CreateBookAsync(newBookDto);

        await SendCreatedAtAsync<GetBookByIdEndpoint>(new { newBookDto.Id}, newBookDto, cancellation: ct);
    }
}
