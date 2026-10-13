using System.Windows;
using TodoList.Models;

namespace TodoList;

public partial class TaskWindow : Window
{
    public TodoModel Task { get; }

    public TaskWindow(TodoModel? task = null)
    {
        InitializeComponent();
        Task = task is null ? new TodoModel() : new TodoModel
        {
            Id = task.Id, Title = task.Title, Description = task.Description,
            CreatedAt = task.CreatedAt, DueDate = task.DueDate,
            Priority = task.Priority, Status = task.Status
        };
        PriorityBox.ItemsSource = Enum.GetValues<TaskPriority>();
        StatusBox.ItemsSource = Enum.GetValues<TaskStatus>();
        TitleBox.Text = Task.Title;
        DescriptionBox.Text = Task.Description;
        DueDatePicker.SelectedDate = Task.DueDate;
        PriorityBox.SelectedItem = Task.Priority;
        StatusBox.SelectedItem = Task.Status;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Task.Title = TitleBox.Text;
            Task.Description = DescriptionBox.Text;
            Task.DueDate = DueDatePicker.SelectedDate;
            Task.Priority = (TaskPriority)PriorityBox.SelectedItem!;
            Task.Status = (TaskStatus)StatusBox.SelectedItem!;
            DialogResult = true;
        }
        catch (Exception error)
        {
            MessageBox.Show(error.Message, "Invalid task", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
