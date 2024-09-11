using OasisoftTask.API.Middlewares;

namespace OasisoftTask.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _corsPolicy = "CORSPOLICY";
            builder.Services.Register_DbContext(builder.Configuration);
            builder.Services.Register_Identity();
            builder.Services.Register_JWTConfig(builder.Configuration);
            builder.Services.Register_SingletonServices();
            builder.Services.Register_Repository();
            builder.Services.AddMappings();
            builder.Services.Register_Service();
            builder.Services.Register_Cors(_corsPolicy);

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Register_SwaggerAndSecurity();
            var app = builder.Build();
            await app.MainSeed();
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My service");
                c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
            });
            //}
            app.UseCors(_corsPolicy);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SecurityStampMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
