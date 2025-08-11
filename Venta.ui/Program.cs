using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Venta.Infrastructure.External;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = new Uri(builder.Configuration["AzureKeyVault:Uri"] ?? string.Empty);
builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());

var aadClientId = builder.Configuration["azure-client-id"];
var aadClientSecret = builder.Configuration["azure-client-secret"];
var tenant = builder.Configuration["azure-tenant-id"];


var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');



builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        options.ClientId = aadClientId;
        options.ClientSecret = aadClientSecret;
        options.TenantId = tenant;
        builder.Configuration.GetSection("AzureAd").Bind(options);
        // Configurar para preservar tokens REALES según documento del laboratorio
        options.SaveTokens = true;
        options.UseTokenLifetime = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.UsePkce = true;

        options.Events = new OpenIdConnectEvents
        {
            OnTokenResponseReceived = context =>
            {
                // Capturar tokens cuando llegan del endpoint
                var accessToken = context.TokenEndpointResponse.AccessToken;
                var idToken = context.TokenEndpointResponse.IdToken;
                var refreshToken = context.TokenEndpointResponse.RefreshToken;

                // Crear lista de tokens para almacenar
                var tokens = new List<AuthenticationToken>();

                if (!string.IsNullOrEmpty(accessToken))
                    tokens.Add(new AuthenticationToken { Name = "access_token", Value = accessToken });
                if (!string.IsNullOrEmpty(idToken))
                    tokens.Add(new AuthenticationToken { Name = "id_token", Value = idToken });
                if (!string.IsNullOrEmpty(refreshToken))
                    tokens.Add(new AuthenticationToken { Name = "refresh_token", Value = refreshToken });

                // Almacenar tokens en las propiedades
                context.Properties.StoreTokens(tokens);

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Los tokens ya están guardados en OnTokenResponseReceived
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddDataProtection(options =>
{
    // Nombre único de aplicación para aislamiento
    options.ApplicationDiscriminator = builder.Configuration["DataProtection:ApplicationName"];
})
.SetDefaultKeyLifetime(TimeSpan.Parse(builder.Configuration["DataProtection:KeyLifetime"]))
.PersistKeysToAzureBlobStorage(
    builder.Configuration["azure-kv-storage-cs"],
    "dataprotection-keys",
    "keys.xml")
.SetApplicationName(builder.Configuration["DataProtection:ApplicationName"]);




builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthenticatedUserPolicy", policy => policy.RequireAuthenticatedUser());

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/", "AuthenticatedUserPolicy");
});


builder.Services.AddExternalServices(builder.Configuration);


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministratorOnly", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("AdministratorOrUser", policy => policy.RequireAuthenticatedUser());
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

string nonce = string.Empty;
app.Use(async (context, next) =>
{
   nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
   context.Items["nonce"] = nonce;
   await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/Error", "?errorCode={0}");
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    // Capturar el nonce y agregarlo a las cookies
    context.Response.Headers.Append("Content-Security-Policy",
        $"script-src 'self' 'nonce-{nonce}'; " +
        $"style-src 'self' 'nonce-{nonce}'; " +
        $"img-src 'self' data:; " +
        $"font-src 'self'; " +
        $"frame-ancestors 'none'; " +
        $"form-action 'self'");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Remove("X-powered-By");
    context.Response.Headers.Remove("Server");
        
    await next.Invoke();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
