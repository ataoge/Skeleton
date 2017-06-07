namespace Ataoge.Data.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// Related properties automatically set when saving/updating <see cref="Entity"/> objects.
    /// </summary>
    public interface IAudited<TKey> : ICreationAudited<TKey>, IModificationAudited<TKey>
        where TKey : struct
    {

    }

    public interface IAuditedString : ICreationAudited<string>, IModificationAuditedString
    {

    }
}