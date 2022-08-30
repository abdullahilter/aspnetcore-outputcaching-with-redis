using Mapster;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_outputcaching_with_redis;

public class CustomerService : ICustomerService
{
    private readonly TestContext _context;

    public CustomerService(TestContext context)
    {
        _context = context;
    }

    public async Task<Customer> CreateAsync(CustomerDto customerDto)
    {
        var customer = customerDto.Adapt<Customer>();

        customer.CreatedOn = DateTime.UtcNow;

        await _context.AddAsync(customer);

        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> GetAsync(Guid id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

        return customer;
    }

    public async Task<List<Customer>> GetAllAsync(string name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var allCustomers = await _context.Customers.ToListAsync();

            return allCustomers;
        }

        var matchedCustomers = await _context.Customers.Where(x => x.Name.Contains(name)).ToListAsync();

        return matchedCustomers;
    }

    public async Task<Customer> UpdateAsync(Guid id, CustomerDto customerDto)
    {
        var customer = await GetAsync(id);

        customer.Adapt(customerDto);

        customer.ModifiedOn = DateTime.UtcNow;

        _context.Update(customer);

        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await GetAsync(id);

        _context.Remove(customer);

        await _context.SaveChangesAsync();
    }
}