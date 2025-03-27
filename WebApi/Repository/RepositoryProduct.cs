using Microsoft.EntityFrameworkCore;
using WebApi.Config;
using WebApi.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Repository
{
    public class RepositoryProduct : IRepositoryProduct
    {
        private readonly DbContextOptions<ContextModel> _OptionBuilder;

        public RepositoryProduct()
        {
            _OptionBuilder = new DbContextOptions<ContextModel>();
        }

        public async Task Add(ProductModel model)
        {
            using (var data = new ContextModel(_OptionBuilder))
            {
                await data.Set<ProductModel>().AddAsync(model);
                await data.SaveChangesAsync();
            }
        }

        public async Task Delete(ProductModel model)
        {
            using (var data = new ContextModel(_OptionBuilder))
            {
                data.Set<ProductModel>().Remove(model);
                await data.SaveChangesAsync();
            }
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            using (var data = new ContextModel(_OptionBuilder))
            {
                return await data.Set<ProductModel>().FindAsync(id);
               
            }
        }

        public async Task<List<ProductModel>> ListAll()
        {
            using (var data = new ContextModel(_OptionBuilder))
            {
                return await data.Set<ProductModel>().ToListAsync();

            }
        }

        public async Task Update(ProductModel model)
        {
            using (var data = new ContextModel(_OptionBuilder))
            {
                data.Set<ProductModel>().Update(model);
                await data.SaveChangesAsync();
            }
        }
    }
}
