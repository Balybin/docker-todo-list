namespace DockerTodoList.Domain
{
    public class TodoListItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        private TodoListItem() { }
        internal TodoListItem(User user, string title)
        {
            Title = title;
            Content = string.Empty;
            User = user ?? throw new ArgumentException(nameof(user));
            UserId = User.Id;
        }

        public void setTitle(string title)
        {
            Title = title;
        }

        public void setContent(string content)
        {
            Content = content;
        }
    }
}
