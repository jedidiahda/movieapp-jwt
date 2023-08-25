using MovieWebApp.Models;
using MovieWebApp.DTO;

namespace MovieWebApp.Repository
{
    public interface ICustomerDeliveryRepository
    {

        public abstract Task<List<CustomerDeliveryDTO>> GetValidCustomerDelivery();
        public abstract Task<List<CustomerReturnDTO>> GetDvdstatuses();
        public abstract Task SendDvdToCustomer(int susubscriptionId,string code,int dvdCatalogId);
        public abstract Task ReturnDVDFromCustomer(int susubscriptionId, string code, int dvdCatalogId);
    }
}
