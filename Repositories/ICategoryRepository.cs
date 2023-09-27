using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICategoryRepository
    {
        void SaveCategory(Category category);
        Category GetCategoryById(int id);
        List<Category> GetCategories();
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        List<Product> GetProducts(int categoryId);
    }
}
