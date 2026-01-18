using System.ComponentModel.DataAnnotations;

namespace StoreApp.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    public string? image { get; set; }
    public IFormFile? File { get; set; }
    public int[] SelectedCategoryIds { get; set; } = Array.Empty<int>();
}
public class ProductsListViewModel
{
    public IEnumerable<ProductViewModel> Products { get; set; }=Enumerable.Empty<ProductViewModel>();
    public PageInfo PageInfo { get; set; }=new();

}
public class PageInfo
{
    public int TotalItems { get; set; }//kac tane eleman var 

    public int ItemsPerPage { get; set; }// sayfa basın eleman sayısı
    public int Totalpages =>(int)Math.Ceiling((decimal)TotalItems/ItemsPerPage); // toplam sayfa sayısı
    public int CurrentPage { get; set; }//secili olan sayfa
}