using Microsoft.AspNetCore.Identity;
using RoverCore.Abstractions.Seeder;
using CoverMe.Domain.Entities.Identity;
using CoverMe.Infrastructure.Common.Seeder.Services;

namespace CoverMe.Infrastructure.Identity.Seeding;

public class ApplicationRoleSeed : ISeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public int Priority { get; } = 1001;

    public ApplicationRoleSeed(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public void CreateRoles()
    {
        var roles = new List<string>
        {
            "Admin",
            "User"
        };

        foreach (var roleName in roles)
        {
            if (!_roleManager.RoleExistsAsync(roleName).Result)
            {
                var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = roleName };

                _roleManager.CreateAsync(role).Wait();
            }
        }
    }

    public Task SeedAsync()
    {
        CreateRoles();

        return Task.CompletedTask;
    }
}