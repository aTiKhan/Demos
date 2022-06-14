using System.Data;
using System.Data.Common;
using Coderr.Client;
using Coderr.Client.Serilog.ContextProviders;
using MvcDemo.App.TodoItems;
using Serilog;

namespace MvcDemo.Data.TodoItems;

public class TaskRepository : ITaskRepository
{
    private readonly IDbTransaction _transaction;

    public TaskRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<TodoTask> Get(int id)
    {
        Log.Debug("Fetching task {id}", id);

        await using var cmd = _transaction.CreateCommand();
        cmd.CommandText = "SELECT * FROM Tasks WHERE Id = @id";
        cmd.AddParameter("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new InvalidOperationException("Failed to find entity " + id);

        var item = MapRow(reader);
        return item;
    }

    public async Task<IReadOnlyList<TodoTask>> List(int userId)
    {
        Log.Debug("Fetching using user {userId}", userId);

        await using var cmd = _transaction.CreateCommand();
        cmd.CommandText = "SELECT * FROM Tasks WHERE IsCompleted = 0 AND UserId = @userId ORDER BY CreatedAtUtc";
        cmd.AddParameter("userId", userId);

        var items = new List<TodoTask>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var item = MapRow(reader);
            items.Add(item);
        }

        return items;
    }

    public async Task Create(TodoTask task)
    {
        Log.Logger.Debug("Creating task1 {task}", task);
        Log.Logger.Debug("Creating task2");
        Log.Debug("Creating task3 {task}", task);
        Log.Debug("Creating task4");
        var p = LogsProvider.Instance;
        var config = Err.Configuration;
        await using var cmd = _transaction.CreateCommand();

        cmd.CommandText =
            "INSERT INTO Tasks (UserId, Title, Description, CreatedAtUtc) VALUES(@userId, @title, @description, GetUtcDate()); select SCOPE_IDENTITY()";
        cmd.AddParameter("userId", task.UserId);
        cmd.AddParameter("title", task.UserId);
        cmd.AddParameter("description", task.UserId);
        var value = await cmd.ExecuteScalarAsync();
        task.SetProperty(x => x.Id, value);
    }

    public async Task Update(TodoTask task)
    {
        Log.Debug("Updating task {task}", task);

        await using var cmd = _transaction.CreateCommand();

        cmd.CommandText =
            "UPDATE Tasks SET Title=@title, Description=@description";

        if (task.CompletedAtUtc.HasValue)
        {
            cmd.CommandText += ", IsCompleted=1, CompletedAtUtc=@completedAtUtc";
            cmd.AddParameter("IsCompleted", task.IsCompleted);
            cmd.AddParameter("CompletedAtUtc", task.CompletedAtUtc.Value);
        }

        cmd.CommandText += " WHERE Id = @id";

        cmd.AddParameter("id", task.Id);
        cmd.AddParameter("title", task.Title);
        cmd.AddParameter("description", task.Description);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task Delete(TodoTask task)
    {
        Log.Debug("Deleting task {task}", task);

        await using var cmd = _transaction.CreateCommand();

        cmd.CommandText =
            "DELETE FROM Tasks WHERE Id = @id";
        cmd.AddParameter("id", task.Id);
        await cmd.ExecuteNonQueryAsync();
    }

    private static TodoTask MapRow(DbDataReader reader)
    {
        var item = new TodoTask(reader.GetString("Title"), reader.GetString("Description"));
        item.SetProperty(x => x.Id, reader.GetInt("Id"));
        item.SetProperty(x => x.CompletedAtUtc, reader.GetDateTimeNullable("CompletedAtUtc"));
        item.SetProperty(x => x.IsCompleted, reader.GetBoolean("CompletedAtUtc"));
        item.SetProperty(x => x.CreatedAtUtc, reader.GetDateTime("CreatedAtUtc"));
        item.UserId = reader.GetInt("UserId");
        return item;
    }
}