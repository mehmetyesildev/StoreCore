using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StoreApp.Data.Abstract;

namespace StoreApp.Data.Concrete;

public class EFStoreRepostory : IStoreRepository
{
    private StoreDbContext _context;
    public EFStoreRepostory(StoreDbContext context)
    {
        _context=context;
    }
    public IQueryable<Product> Products => _context.Products;
    public IQueryable<Category> Categories => _context.Categories;
    public void CreateBuilder(Product entity)
    {
        _context.Products.Add(entity);
        _context.SaveChanges();
    }

    public void DeleteOneProduct(int id)
    {
        var product=_context.Products.Find(id);
        if(product!=null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    public Product? GetOneProduct(int Id)
    {
        return _context.Products.Include(p => p.Categories).FirstOrDefault(p => p.Id == Id);
    }

    public int GetProductCount(string category)
    {
        return category == null ? Products.Count()
                : Products.Include(p => p.Categories)
                .Where(p => p.Categories.Any(a => a.Url == category)).Count();
    }

    public IEnumerable<Product> GetProductsByCategory(string category, int page, int pageSize)
    {
        var products = Products;
        if (!string.IsNullOrEmpty(category))
        {
            products = products.Include(p => p.Categories).Where(p => p.Categories.Any(a => a.Url == category));
        }
        return products.Skip((page - 1) * pageSize).Take(pageSize);
        
    }

    public void UpdateOneProduct(Product entity)
    {
        _context.Products.Update(entity);
        _context.SaveChanges();
    }
}