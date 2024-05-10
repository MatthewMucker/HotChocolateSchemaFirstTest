using customers.Types;

using HotChocolate;
using Microsoft.EntityFrameworkCore;

using System.Runtime.CompilerServices;

System.Console.WriteLine($"Args: {args}");

//Create a sample database if first cmdline arg is 'createDB'
if (args.Length > 0 && args[0] == "createDB")
{
    DatabaseCreator.Create("CustomersAddresses.sqlite", 2000);
    Console.WriteLine("Created new database.");
}

var builder = WebApplication.CreateBuilder(args);

//See https://www.youtube.com/watch?v=BcTPIGLYB0I regarding thread safety of DB Context.


builder.Services
    .AddDbContextPool<ApplicationDbContext>(options => options.UseSqlite("Data Source = CustomersAddresses.sqlite"))
    .AddGraphQLServer()
    //Various things I have tried...
    //.BindRuntimeType<AddCustomerInput>()
    //.AddTypes() //Need ModuleInfo.cs with [assembly: Module("CustomerAddressTypes")] and reference to 
    //.BindRuntimeType<Customer>()
    //.BindRuntimeType<Address>()
    .AddQueryType<Query>()
    .RegisterDbContext<ApplicationDbContext>(HotChocolate.Data.DbContextKind.Resolver)
    .AddDocumentFromFile("customers_schema.graphql");

var app = builder.Build();

app.MapGet("/", () => { 
    return Results.Content("""Visit <a href="graphql">/graphql</a> for the Banana Cake Pop interface.""", "text/html");
});

app.MapGraphQL();
app.Run();
