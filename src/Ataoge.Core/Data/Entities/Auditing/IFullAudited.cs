namespace Ataoge.Data.Entities.Auditing
{
    /// <summary>
    /// This interface ads <see cref="IDeletionAudited"/> to <see cref="IAudited"/> for a fully audited entity.
    /// </summary>
    public interface IFullAudited<TKey> : IAudited<TKey>, IDeletionAudited<TKey>
        where TKey : struct
    {
        
    }

    public interface IFullAuditedString : IAuditedString, IDeletionAuditedString
    {
        
    }
}