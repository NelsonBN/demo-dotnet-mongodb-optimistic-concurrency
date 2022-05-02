using System.Reflection;
using Demo.WebAPI.DataBase;
using Demo.WebAPI.MediatorDemo;
using MediatR;

namespace Demo.WebAPI;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();


        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped<IMongoDBContext, MongoDBContext>();
        services.AddScoped<IDemoProblemRepository, DemoProblemRepository>();
        services.AddScoped<IWithDBRaceConditionRepository, WithDBRaceConditionRepository>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorPipeline<,>));
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
