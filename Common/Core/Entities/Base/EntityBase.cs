namespace Common.Core.Entities.Base
{
    public abstract class EntityBase<TKey,TName> : IEntityBase<TKey,TName>
    {
        public  TKey Id { get;  set; }
        public  TName Name { get; set; }
    }
}