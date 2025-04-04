﻿using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using RiverBooks.Books.BookEndpoints;
using Xunit.Abstractions;

namespace RiverBooks.Books.Test.Endpoints;

public class BookGetById(Fixture fixture, ITestOutputHelper outputHelper) : TestClass<Fixture>(fixture, outputHelper)
{
    [Theory]
    [InlineData("A89F6CD7-4693-457B-9009-02205DBBFE45", "The Fellowship of the Ring")]
    [InlineData("E4FA19BF-6981-4E50-A542-7C9B26E9EC31", "The Two Towers")]
    [InlineData("17C61E41-3953-42CD-8F88-D3F698869B35", "The Return of the King")]
    public async Task ReturnsExpectedBookGivenId(string validId, string expectedTitle)
    {
        //arrange
        Guid id = new(validId);
        GetBookByIdRequest request = new(id);

        //get data
        var testResult = await Fixture.Client.GETAsync<GetById, GetBookByIdRequest, BookDto>(request);

        //validate
        testResult.Response.EnsureSuccessStatusCode();
        testResult.Result.Title.Should().Be(expectedTitle);
    }
}
