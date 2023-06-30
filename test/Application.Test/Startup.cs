using Application.Test.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Test;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTestMockServices();
    }
}