using FranDictProt.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using static System.Net.WebRequestMethods;

namespace FranDictProt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddFluentUIComponents();

            builder.Services.AddSingleton<IParagraphRepository,ParagraphRepository>();
            builder.Services.AddSingleton<IWordKeyRepository,WordKeyRepository>();
            builder.Services.AddSingleton<IMorphology,Morphology>();

            builder.Services.AddTransient<IControl,Control>();

            builder.WebHost.UseUrls("http://0.0.0.0:5000");
            //builder.Services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 5000; // Or your specific HTTPS port
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
