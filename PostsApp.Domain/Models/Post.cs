﻿using System.ComponentModel.DataAnnotations;

namespace PostsApp.Domain.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public User User { get; set; }
}