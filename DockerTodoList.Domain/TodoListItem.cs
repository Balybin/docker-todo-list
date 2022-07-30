namespace DockerTodoList.Domain
{
    public class TodoListItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }

        internal TodoListItem(string title) 
        { 
            Title = title;
            Content = string.Empty;
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
