using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoList.Models;

public sealed class TodoModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTime _createdAt = DateTime.Now;
    private DateTime? _dueDate;
    private TaskPriority _priority = TaskPriority.Medium;
    private TaskStatus _status = TaskStatus.NotStarted;

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title
    {
        get => _title;
        set
        {
            var title = value?.Trim() ?? string.Empty;
            if (title.Length is 0 or > 200)
                throw new ArgumentException("Title must contain from 1 to 200 characters.");
            SetField(ref _title, title);
        }
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value?.Trim() ?? string.Empty);
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetField(ref _createdAt, value);
    }

    public DateTime? DueDate
    {
        get => _dueDate;
        set => SetField(ref _dueDate, value);
    }

    public TaskPriority Priority
    {
        get => _priority;
        set => SetField(ref _priority, value);
    }

    public TaskStatus Status
    {
        get => _status;
        set => SetField(ref _status, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
