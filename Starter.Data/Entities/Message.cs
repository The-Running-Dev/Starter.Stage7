namespace Starter.Data.Entities
{
    /// <summary>
    /// Implements the Message entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Message<T>
    {
        public MessageCommand Command { get; set; }

        public T Entity { get; set; }

        public string Type { get; set; }

        public Message(MessageCommand command, T entity)
        {
            Command = command;
            Entity = entity;
            Type = typeof(T).ToString();
        }
    }
}