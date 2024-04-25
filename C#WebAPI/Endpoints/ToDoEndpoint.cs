namespace C_WebAPI.Endpoints
{
    public static class ToDoEndpoint
    {
        public static RouteGroupBuilder ToDoEndpoints(this WebApplication app)
        {

            var todo_group = app.MapGroup("todos");

            todo_group.MapGet("/", (ITaskService service) =>
            {
                return service.getTodos();
            });

            todo_group.MapPost("/", (ToDo task, ITaskService service) =>
            {
                service.addToDo(task);

                return Results.Created("/{id}", task);
            }).AddEndpointFilter(async (context, next) => {
                var posting_taks = context.GetArgument<ToDo>(0);

                var errors = new Dictionary<string, string[]>();

                if (posting_taks == null)
                {
                    errors.Add("data", new string[] { "no data provided" });
                    return Results.ValidationProblem(errors);
                }

                if (posting_taks.isComplete == true)
                {

                    errors.Add("data", new string[] { "can not add completed task" });
                    return Results.ValidationProblem(errors);
                }

                return await next(context);

            }).WithParameterValidation();

            todo_group.MapGet("/{id}", (int id, ITaskService service) =>
            {
                var target_todo = service.getToDoById(id);
                return target_todo == null ? Results.NotFound() : Results.Ok(target_todo);
            });

            todo_group.MapDelete("/{id}", (int id, ITaskService service) =>
            {
                service.deleteToDoById(id);
                return Results.Ok("task deleted succesfuly");
            });
            return todo_group;
        }
    }
}
