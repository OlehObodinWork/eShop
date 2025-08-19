
using Pgvector;
namespace Catalog.API.Services
{
    public interface IPrimaryCatalogAI
    {
        bool IsEnabled { get; }

        /// <summary>Gets an embedding vector for the specified text.</summary>
        ValueTask<Vector> GetEmbeddingAsync(string text);

        /// <summary>Gets an embedding vector for the specified catalog item.</summary>
        ValueTask<Vector> GetEmbeddingAsync(PrimaryCatalogItem item);

        /// <summary>Gets embedding vectors for the specified catalog items.</summary>
        ValueTask<IReadOnlyList<Vector>> GetEmbeddingsAsync(IEnumerable<PrimaryCatalogItem> item);
    }
}
