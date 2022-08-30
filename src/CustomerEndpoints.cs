using Microsoft.AspNetCore.OutputCaching;

namespace aspnetcore_outputcaching_with_redis
{
    public static class CustomerEndpoints
    {
        public static void RegisterCustomerEndpoints(this WebApplication app)
        {
            app.MapPost("customers", async (CustomerDto customerDto, ICustomerService service,
                IOutputCacheStore cacheStore, CancellationToken cancellationToken) =>
            {
                var customer = await service.CreateAsync(customerDto);

                await cacheStore.EvictByTagAsync("customers", cancellationToken);

                return Results.Created($"customers/{customer.Id}", customer);
            });

            app.MapGet("customers/{id:guid}", async (Guid id, ICustomerService service) =>
            {
                var customer = await service.GetAsync(id);

                if (customer is null) return Results.NotFound();

                return Results.Ok(customer);
            }).CacheOutput(x => x.AddPolicy<ByIdCachePolicy>());

            app.MapGet("customers", async (string name, ICustomerService service) =>
            {
                var customers = await service.GetAllAsync(name);

                return Results.Ok(customers);
            }).CacheOutput(x => x.Tag("customers").VaryByQuery("name").Expire(TimeSpan.FromSeconds(30)));

            app.MapPut("customers/{id:guid}", async (Guid id, CustomerDto customerDto, ICustomerService service,
                IOutputCacheStore cacheStore, CancellationToken cancellationToken) =>
            {
                var customer = await service.UpdateAsync(id, customerDto);

                await cacheStore.EvictByTagAsync(id.ToString(), cancellationToken);

                return Results.Ok(customer);
            });

            app.MapDelete("customers/{id:guid}", async (Guid id, ICustomerService service,
                IOutputCacheStore cacheStore, CancellationToken cancellationToken) =>
            {
                await service.DeleteAsync(id);

                await cacheStore.EvictByTagAsync("customers", cancellationToken);

                return Results.NoContent();
            });
        }
    }
}