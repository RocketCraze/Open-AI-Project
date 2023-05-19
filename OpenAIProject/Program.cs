namespace OpenAIProject
{
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using OpenAIProject.Data;
    using OpenAIProject.Interfaces;
    using OpenAIProject.Services;
    using FluentValidation;
    using OpenAIProject.Models;
    using OpenAIProject.Validators;

    public static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = configBuilder.Build();

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IEditService, EditService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IValidator<ChatGPTMessage>, ChatGPTValidator>();
            builder.Services.AddScoped<IValidator<DaVinciEdit>, DaVinciValidator>();
            builder.Services.AddScoped<IValidator<ImageGenerationAI>, ImageGenerationValidator>();

            builder.Services
            .AddControllersWithViews()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

           

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "ChatGPT",
                pattern: "{controller=ChatGPT}/{action=Index}/{id?}");

            app.Run();
        }
    }
    
}


