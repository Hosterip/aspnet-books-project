﻿using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Genres.Queries.GetAllGenres;

internal class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<GenreResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGenresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GenreResult>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var rawGenres = await _unitOfWork.Genres.GetAllAsync(cancellationToken);
        var genres = rawGenres
            .Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name });
        return genres;
    }
}