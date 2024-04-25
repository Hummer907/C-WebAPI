




class InMemoryTaskService :ITaskService
{

    private readonly List<ToDo> _todoList = new List<ToDo>();

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
    public void addToDo(ToDo toDo)
    {
        _todoList.Add(toDo);
    }


}

