namespace Hotels.Domain.Common.Interfaces;

public interface IKey<TKey> where TKey : notnull
{
    TKey Id { get; }
}
