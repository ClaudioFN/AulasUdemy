using ApiCatalogoMinimal.Models;
using ApiCatalogoMinimal.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogoMinimal.ApiEndpoints;

public static class AutenticacaoEndpoints
{
    public static void MapAutenticacaoEndpoints(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Login Inválido!");
            }

            if (userModel.UserName == "claudio" && userModel.Password == "senha@123")
            {
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"], userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Login Inválido Identificado!");
            }
        }).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status200OK).WithName("Login").WithTags("Autenticacao");

    }
}
