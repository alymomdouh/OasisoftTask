namespace OasisoftTask.Applications.Dtos.ToDoDtos
{
    public record ListToDo
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}
