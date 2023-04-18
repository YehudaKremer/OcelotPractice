var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var documents = new List<Document>(){
     new Document{ Id = 1, Name = "Document1" },
     new Document{ Id = 2, Name = "Document2" }
};

app.MapGet("/documents", () =>
{
    return documents;
});

app.Run();
