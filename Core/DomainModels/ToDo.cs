using System.ComponentModel.DataAnnotations.Schema;

namespace OasisoftTask.Core.DomainModels
{
    public class ToDo : BaseEntity
    {
        public string Title { get; private set; }
        public bool Completed { get; private set; }
        public int UserId { get; private set; }
        public ToDo() { }

        public ToDo(string title, int userId, bool completed)
        {
            Title = title;
            UserId = userId;
            Completed = completed;
        }
        public void UpdateData(string title, int userId, bool completed)
        {
            Title = title;
            UserId = userId;
            Completed = completed;
        }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUserObj { get; set; }
    }
}
