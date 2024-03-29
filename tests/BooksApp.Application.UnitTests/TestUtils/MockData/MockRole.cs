﻿using PostsApp.Domain.Models;

namespace Application.UnitTest.TestUtils.MockData;

public static class MockRole
{
    public static Role GetRole(string role) => new Role
    {
        Id = 0,
        Name = role
    };
}