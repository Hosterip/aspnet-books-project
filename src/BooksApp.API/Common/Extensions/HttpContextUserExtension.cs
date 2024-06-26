﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PostsApp.Common.Constants;

namespace PostsApp.Common.Extensions;

public static class HttpContextUserExtension
{
    public static string? GetUsername(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    
    public static string? GetRole(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
    }
    
    // You must be already sure that user is exists before using it 
    public static string? GetId(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == AdditionalClaimTypes.Id)?.Value;
    }
    public static string? GetSecurityStamp(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == AdditionalClaimTypes.SecurityStamp)?.Value;
    }

    public static void ChangeUsername(this HttpContext httpContext, string valueOfClaim)
    {
        httpContext.ChangeClaim(ClaimTypes.NameIdentifier, valueOfClaim);
    }
    
    public static void ChangeRole(this HttpContext httpContext, string valueOfClaim)
    {
        httpContext.ChangeClaim(ClaimTypes.Role, valueOfClaim);
    }
    
    public static void ChangeSecurityStamp(this HttpContext httpContext, string valueOfClaim)
    {
        httpContext.ChangeClaim(AdditionalClaimTypes.SecurityStamp, valueOfClaim);
    }

    public static async Task Login(this HttpContext httpContext, string id, string username, string role, string securityStamp)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(AdditionalClaimTypes.SecurityStamp, securityStamp),
            new Claim(AdditionalClaimTypes.Id, id),
        };
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity));
    }
    
    private static void ChangeClaim(this HttpContext httpContext, string typeOfClaim, string valueOfClaim)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        
        identity!.RemoveClaim(identity.FindFirst(typeOfClaim));
        identity.AddClaim(new Claim(typeOfClaim, valueOfClaim));
    } 
}