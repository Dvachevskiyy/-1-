using TodoList.Models;
using TodoList.Repositories;

namespace TodoList.Services;

public sealed class TodoService
{
    private readonly ITaskRepository _repository;
    private readonly List<TodoModel> _tasks;

    public TodoService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = repository.Load().ToList();
    }

    public IReadOnlyList<TodoModel> GetAll() => _tasks;

    public void Add(TodoModel task)
    {
        Validate(task);
        if (task.Id == Guid.Empty) task.Id = Guid.NewGuid();
        _tasks.Add(task);
        Save();
    }

    public void Update(TodoModel task)
    {
        Validate(task);
        var index = _tasks.FindIndex(item => item.Id == task.Id);
        if (index < 0) throw new InvalidOperationException("Task was not found.");
        _tasks[index] = task;
        Save();
    }

    public void Delete(Guid id)
    {
        _tasks.RemoveAll(task => task.Id == id);
        Save();
    }

    public void ChangeStatus(Guid id, TaskStatus status)
    {
        var task = Find(id);
        task.Status = status;
        Save();
    }

    public void Save() => _repository.Save(_tasks);

    private TodoModel Find(Guid id) => _tasks.FirstOrDefault(task => task.Id == id)
        ?? throw new InvalidOperationException("Task was not found.");

    private static void Validate(TodoModel task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("A task title is required.");
        if (task.DueDate.HasValue && task.DueDate.Value < task.CreatedAt.Date)
            throw new ArgumentException("Due date cannot be earlier than creation date.");
    }
}
