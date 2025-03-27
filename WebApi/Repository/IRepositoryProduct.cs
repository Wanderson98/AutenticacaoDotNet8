using WebApi.Entities;

namespace WebApi.Repository
{
    public interface IRepositoryProduct
    {
        Task Add(ProductModel model);
        Task Update(ProductModel model);
        Task Delete(ProductModel model);
        Task<ProductModel> GetProductById(int id);
        Task<List<ProductModel>> ListAll();

    }
}
