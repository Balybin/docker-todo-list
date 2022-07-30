namespace DockerTodoList.Domain
{
    public class User
    {
        private readonly List<TodoListItem> _todoListItems = new();
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyCollection<TodoListItem> TodoListItems => _todoListItems;

        public User(string name)
        {
            Name = name;
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public TodoListItem AddTodoListItem(string title)
        {
            var todoListItem = new TodoListItem(this, title);
            _todoListItems.Add(todoListItem);
            return todoListItem;
        }
        public void RemoveTodoListItem(TodoListItem todoListItem)
        {
            _todoListItems.Remove(todoListItem);
        }
    }
}