
namespace Common
{
    public abstract class BaseEntity<TKey, TName>
    {
        public TKey Id { get; set; }
        public TName Name { get; set; }

    }
}