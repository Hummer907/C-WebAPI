

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITaskService>(new InMemoryTaskService());


var app = builder.Build();

app.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions().AddRedirect("tasks","todos"));

app.Use(async (context ,next) =>
{
    System.Console.WriteLine(context.Request.Method,"    ",context.Request.Path,"    ",context.Request.Body);
   await next(context);
    System.Console.WriteLine("Middle ware finished logging in ");

});
var todoList = new List<ToDo>();

app.MapGet("/todos", (ITaskService service) =>
{
    return service.getTodos();
});

app.MapPost("/todos/", (ToDo task,ITaskService service) =>
{
    service.addToDo(task);
    
    return Results.Created("./todos/{id}", task);
}).AddEndpointFilter(async (context ,next) => {
    var posting_taks = context.GetArgument<ToDo>(0);

    var errors = new Dictionary<string, string[]>();

    if(posting_taks == null)
    {
        errors.Add("data",new string[] { "no data provided" });
        return Results.ValidationProblem(errors);
    }
    
    if (posting_taks.isComplete == true)
    {

        errors.Add("data", new string[] { "can not add completed task" });
        return Results.ValidationProblem(errors);
    }

    return await next(context); 

});

app.MapGet("/todos/{id}", (int id,ITaskService service) =>
{
    var target_todo = service.getToDoById(id);
    return target_todo == null ? Results.NotFound() : Results.Ok(target_todo);
});

app.MapDelete("/todos/{id}", (int id, ITaskService service) =>
{
    service.deleteToDoById(id);
    return Results.Ok("task deleted succesfuly");
});

app.Run();

public record ToDo(int id, string name, DateTime dueDate, bool isComplete);


public interface ITaskService{

    public ToDo? getToDoById(int id);
    public List<ToDo> getTodos();
    public void deleteToDoById(int id);
    public void addToDo(ToDo toDo);

}

class InMemoryTaskService : ITaskService
{

    private readonly List<ToDo> _todoList = new List<ToDo>() ;

   public ToDo? getToDoById(int id)
    {

        return _todoList.SingleOrDefault(t => id == t.id);
    }
   public List<ToDo> getTodos()
    {
        return _todoList;
    }
   public void deleteToDoById(int id)
    {
        _todoList.RemoveAll(t => id == t.id);
    }
  public  void addToDo(ToDo toDo)
    {
        _todoList.Add(toDo);
    }


}