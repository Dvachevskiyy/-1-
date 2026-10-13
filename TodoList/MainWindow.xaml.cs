using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TodoList.Models;
using TodoList.Repositories;
using TodoList.Services;

namespace TodoList;

public partial class MainWindow : Window
{
    private readonly TodoService _service = new(new JsonTaskRepository());
    private readonly ObservableCollection<TodoModel> _tasks = [];

    public MainWindow()
    {
        InitializeComponent();
        foreach (var task in _service.GetAll()) _tasks.Add(task);
        TasksGrid.ItemsSource = _tasks;
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new TaskWindow { Owner = this };
        if (dialog.ShowDialog() != true) return;
        try { _service.Add(dialog.Task); _tasks.Add(dialog.Task); }
        catch (Exception error) { ShowError(error); }
    }

    private void EditTask_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not TodoModel selected) return;
        var dialog = new TaskWindow(selected) { Owner = this };
        if (dialog.ShowDialog() != true) return;
        try
        {
            _service.Update(dialog.Task);
            var index = _tasks.IndexOf(selected);
            if (index >= 0) _tasks[index] = dialog.Task;
        }
        catch (Exception error) { ShowError(error); }
    }

    private void DeleteTask_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not TodoModel selected) return;
        if (MessageBox.Show($"Delete '{selected.Title}'?", "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        _service.Delete(selected.Id);
        _tasks.Remove(selected);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try { _service.Save(); MessageBox.Show("Tasks saved.", "Todo List"); }
        catch (Exception error) { ShowError(error); }
    }

    private void TasksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

    private static void ShowError(Exception error) => MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
}
