﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Queries.GetImage;

namespace PostsApp.Controllers;

[Route("[controller]")]
public class ImagesController : Controller
{
    private readonly ISender _sender;

    public ImagesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{imageName}")]
    public async Task<IActionResult> GetImage(string imageName, CancellationToken cancellationToken)
    {
        var query = new GetImageQuery
        {
            ImageName = imageName
        };
        var result = await _sender.Send(query, cancellationToken);
        return File(
            result.FileStream,
            $"image/{result.FileInfo.Extension.Replace(".", "")}"
        );
    }
}