using StoreApp.Data.Concrete;

namespace StoreApp.Data.Abstract;

public interface IStoreRepository
{
    IQueryable<Product>Products {get;}
    IQueryable<Category> Categories { get; }

    void CreateBuilder(Product entity);
    int GetProductCount(string category);
    Product? GetOneProduct(int Id);
    void UpdateOneProduct(Product entity);
    void DeleteOneProduct(int id);
    IEnumerable<Product>GetProductsByCategory(string category, int page, int pageSize);
}