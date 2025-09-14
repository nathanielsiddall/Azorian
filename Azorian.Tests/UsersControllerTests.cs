using Azorian.Controllers;
using Azorian.Data;
using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UsersControllerTests
{
    private static AzorianContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AzorianContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AzorianContext(options);
    }

    [Fact]
    public async Task CRUD_Workflow_CreatesAuditLogs()
    {
        using var context = CreateContext();
        var service = new UserService(context);
        var controller = new UsersController(service);

        // Create
        var createResult = await controller.CreateUser(new User { Name = "Alice" });
        var created = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        var createdUser = Assert.IsType<User>(created.Value);
        Assert.Equal("Alice", createdUser.Name);
        Assert.Single(context.Users);
        Assert.Single(context.AuditLogs);
        Assert.Equal("CreateUser", context.AuditLogs.Single().Action);

        // Update
        var updatedUser = new User { Id = createdUser.Id, Name = "Alice Updated" };
        var updateResult = await controller.UpdateUser(createdUser.Id, updatedUser);
        var ok = Assert.IsType<OkObjectResult>(updateResult.Result);
        var returned = Assert.IsType<User>(ok.Value);
        Assert.Equal("Alice Updated", returned.Name);
        Assert.Equal(2, context.AuditLogs.Count());
        Assert.Contains(context.AuditLogs, l => l.Action == "UpdateUser");

        // Delete
        var deleteResult = await controller.DeleteUser(createdUser.Id);
        Assert.IsType<NoContentResult>(deleteResult);
        Assert.Empty(context.Users);
        Assert.Equal(3, context.AuditLogs.Count());
        Assert.Contains(context.AuditLogs, l => l.Action == "DeleteUser");
    }

    [Fact]
    public void WriteEndpoints_HaveAuthorizeAdminRole()
    {
        var type = typeof(UsersController);
        foreach (var method in new[] { "CreateUser", "UpdateUser", "DeleteUser" })
        {
            var mi = type.GetMethod(method);
            var attr = Assert.Single(mi!.GetCustomAttributes(typeof(AuthorizeAttribute), false));
            var auth = Assert.IsType<AuthorizeAttribute>(attr);
            Assert.Equal("Admin", auth.Roles);
        }

        foreach (var method in new[] { "GetUsers", "GetUser" })
        {
            var mi = type.GetMethod(method);
            var attrs = mi!.GetCustomAttributes(typeof(AuthorizeAttribute), false);
            Assert.True(attrs.Length == 0 || attrs.Cast<AuthorizeAttribute>().All(a => a.Roles != "Admin"));
        }
    }

    [Fact]
    public async Task GetEndpoints_ReturnUsers()
    {
        using var context = CreateContext();
        context.Users.Add(new User { Name = "Bob" });
        await context.SaveChangesAsync();
        var service = new UserService(context);
        var controller = new UsersController(service);

        var listResult = await controller.GetUsers();
        var listOk = Assert.IsType<OkObjectResult>(listResult.Result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(listOk.Value);
        Assert.Single(users);

        var singleResult = await controller.GetUser(context.Users.Single().Id);
        var singleOk = Assert.IsType<OkObjectResult>(singleResult.Result);
        var user = Assert.IsType<User>(singleOk.Value);
        Assert.Equal("Bob", user.Name);
    }
}
