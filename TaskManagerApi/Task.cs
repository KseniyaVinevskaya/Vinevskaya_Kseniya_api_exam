using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
namespace TaskManagerApi
{
    public class BTask
    {
        private static int Count = 0;
        public int Id { get; private set; }
        [Required(ErrorMessage = "Название обязательно для заполнения.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина Названия должна быть от 2 до 100 символов.")]
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

        public BTask(string title, string description, Status status)
        {
            Count++;
            Id = Count;
            Title = title;
            Description = description;
            Status = status;
        }
    }
}
