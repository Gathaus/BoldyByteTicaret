using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Web.Models;

public class ProductsViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Brand> Brands { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    
    // Filter properties
    public string? SearchTerm { get; set; }
    public string? Style { get; set; }
    public List<string> SelectedFinishes { get; set; } = new();
    public List<string> SelectedColors { get; set; } = new();
    public List<string> SelectedRooms { get; set; } = new();
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalProducts { get; set; }
    
    // Pagination info
    public int TotalPages => (int)Math.Ceiling(TotalProducts / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

public class ProductFilterModel
{
    public string? Style { get; set; }
    public List<string> Finishes { get; set; } = new();
    public List<string> Colors { get; set; } = new();
    public List<string> Rooms { get; set; } = new();
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}