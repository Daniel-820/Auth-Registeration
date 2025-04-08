using System.Security.Claims;
using AuthWebapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuthWebapi.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/UserProfile", GetUserProfile);
            app.MapGet("/myEnd", MyGet);
            return app;
        }


       
        private static async Task<IResult> GetUserProfile(
            ClaimsPrincipal user,
            UserManager<AppUser> userManager)
        {
            string userID=user.Claims.First(x=>x.Type== "userID").Value;
            var userDetails = await userManager.FindByIdAsync(userID);
            return Results.Ok(
                new
                {
                    email = userDetails?.Email,
                    fullName = userDetails?.FullName
                });
        }

        [AllowAnonymous]
        private static string MyGet()
        {
            return "Hi api working!";
        }

    }
}
