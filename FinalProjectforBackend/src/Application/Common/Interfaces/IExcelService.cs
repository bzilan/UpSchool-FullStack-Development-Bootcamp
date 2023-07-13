using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IExcelService
    {
        Task<byte[]> GenerateExcelFile(List<Product> products);
    }
}
