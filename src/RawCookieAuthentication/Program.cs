using RawCookieAuthentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.Use((context, next) =>
{
    var authService = context.RequestServices.GetService<AuthService>()!;
    
    if (!authService.IsAuthenticated())
        authService.SetPrincipal();

    return next();
});

app.MapGet("/login", (AuthService authService) =>
{
    authService.SignIn();

    return Results.Ok("authenticated");
});

app.MapGet("/username", (AuthService authService) =>
{
    if (!authService.IsAuthenticated())
        return Results.Unauthorized();

    var user = authService.GetPrincipal();
    return Results.Ok(user.FindFirst("user")?.Value);
});

app.MapGet("/logout", (AuthService authService) =>
{
    authService.SignOut();

    return Results.Ok();
});

app.Run();