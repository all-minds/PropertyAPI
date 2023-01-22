using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PropertyAPI.Repositories;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.Authority = "https://securetoken.google.com/carboncreditsfiap";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/carboncreditsfiap",
            ValidateAudience = true,
            ValidAudience = "carboncreditsfiap",
            ValidateLifetime = true
        };
    });

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<PropertyRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
var firebaseAuthPath = "FirebaseAuth/carboncreditsfiap-firebase-adminsdk-dn8z4-162474e67b.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", firebaseAuthPath);     
FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault()
            });

app.Run();
