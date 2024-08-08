namespace GymMangamentSystem.Apis.Extention
{
    public static class SwaggerServiceExtention
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection service)
        {
            service.AddEndpointsApiExplorer();

            service.AddSwaggerGen();

            return service;
        }
        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger();

            app.UseSwaggerUI();

            return app;
        }

    }
}
