using System.Text.Json;
using System.Text.Json.Serialization;
using TodoList.Models;

namespace TodoList.Repositories;

public sealed class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public JsonTaskRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "tasks.json");
    }

    public IReadOnlyList<TodoModel> Load()
    {
        if (!File.Exists(_filePath)) return [];

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TodoModel>>(json, _options) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public void Save(IEnumerable<TodoModel> tasks)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        var json = JsonSerializer.Serialize(tasks, _options);
        File.WriteAllText(_filePath, json);
    }
}
