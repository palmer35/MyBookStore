using Microsoft.AspNetCore.Builder;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers(); 
    }
}
