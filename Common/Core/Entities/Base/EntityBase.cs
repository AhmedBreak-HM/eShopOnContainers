namespace Common.Core.Entities.Base
{
    public abstract class EntityBase<TKey,TName> : IEntityBase<TKey,TName>
    {
        public  TKey Id { get; protected set; }
        public  TName Name { get;protected set; }
    }
}