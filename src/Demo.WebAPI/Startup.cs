using Demo.WebAPI.DataBase;
using Demo.WebAPI.Interfaces;

namespace Demo.WebAPI;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();


        services.AddScoped<IMongoDBContext, MongoDBContext>();
        services.AddScoped<IDemoProblemRepository, DemoProblemRepository>();
        services.AddScoped<IWithDBRaceConditionRepository, WithDBRaceConditionRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseEndpoints(endpoints
            => endpoints.MapControllers() // Mapping all controller
        );
    }
}
