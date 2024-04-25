

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
using C_WebAPI.Endpoints;


var builder = WebApplication.CreateBuilder(args);

//dependancy injection
builder.Services.AddSingleton<ITaskService>(new InMemoryTaskService());


var app = builder.Build();
//redirect 
app.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions().AddRedirect("tasks","todos"));


//middle ware
app.Use(async (context ,next) =>
{
    System.Console.WriteLine(context.Request.Method,"    ",context.Request.Path,"    ",context.Request.Body);
   await next(context);
    System.Console.WriteLine("Middle ware finished logging in ");

});
var todoList = new List<ToDo>();

//group routes
app.ToDoEndpoints();

app.Run();




