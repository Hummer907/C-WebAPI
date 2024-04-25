

public interface ITaskService
{

    public ToDo? getToDoById(int id);
    public List<ToDo> getTodos();
    public void deleteToDoById(int id);
    public void addToDo(ToDo toDo);

}

