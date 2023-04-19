var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var users = new List<User>(){
    new User{ Id = 1, Name = "User1" },
    new User{ Id = 2 , Name = "User2" }
};

app.MapGet("/users", (HttpContext context) =>
{
    return users;
});

app.MapGet("/users/{id}", (int id) =>
{
    return users.Find(i => i.Id == id);
});

app.Run();
