using MovieWebApp.Models;
using MovieWebApp.DTO;

namespace MovieWebApp.Repository
{
    public interface ICustomerDeliveryRepository
    {

        Task<List<CustomerDeliveryDTO>> GetValidCustomerDelivery();
        Task<List<CustomerReturnDTO>> GetDvdstatuses();
        Task SendDvdToCustomer(int susubscriptionId,string code,int dvdCatalogId);
        Task ReturnDVDFromCustomer(int susubscriptionId, string code, int dvdCatalogId);
    }
}
