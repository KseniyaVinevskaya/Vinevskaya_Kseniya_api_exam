using TaskManagerApi;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Исходные данные
List<Status> statuses = new()
{
    new("Открыта"),
    new("В процессе"),
    new("Завершена")
};

List<BTask> tasks = new()
{
    new("Сдать отчёт", "Подготовить и отправить финальный отчёт", statuses[0]),
    new("Позвонить клиенту", "Уточнить детали по новому заказу", statuses[1]),
    new("Обновить сайт", "Разместить новую новость на главной странице", statuses[2])
};

//  Получить список задач
app.MapGet("/tasks", () => tasks);

// Создать задачу
app.MapPost("/tasks/{title}/{description}/{status}", (string title, string description, string status) =>
{
    var neededStatus = statuses.FirstOrDefault(s => s.Name == status);
    if (neededStatus is null) return Results.BadRequest("Недопустимый статус");

    var task = new BTask(title, description, neededStatus);
    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
});

// Обновить задачу
app.MapPut("/tasks/{id}", async (int id, HttpContext context) =>
{
    var updatedTask = await context.Request.ReadFromJsonAsync<BTask>();
    var existingTask = tasks.FirstOrDefault(t => t.Id == id);
    if (existingTask is null) return Results.NotFound();

    existingTask.Title = updatedTask!.Title;
    existingTask.Description = updatedTask.Description;
    existingTask.Status = updatedTask.Status;

    return Results.Ok(existingTask);
});

// 🔹 Удалить задачу
app.MapDelete("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    tasks.Remove(task);
    return Results.NoContent();
});

app.Run();
