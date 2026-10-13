using TodoList.Models;

namespace TodoList.Repositories;

public interface ITaskRepository
{
    IReadOnlyList<TodoModel> Load();
    void Save(IEnumerable<TodoModel> tasks);
}
