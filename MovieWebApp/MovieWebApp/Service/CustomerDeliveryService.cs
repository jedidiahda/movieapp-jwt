using MovieWebApp.Models;
using MovieWebApp.Repository;
using MovieWebApp.DTO;
using AutoMapper;

namespace MovieWebApp.Service
{
    public class CustomerDeliveryService
    {

        private readonly ICustomerDeliveryRepository _customerDeliverRepository;
        private readonly IMapper _mapper;

        public CustomerDeliveryService(IMapper mapper, ICustomerDeliveryRepository customerDeliveryRepository)
        {
            _mapper = mapper;
            _customerDeliverRepository = customerDeliveryRepository;
        }


        public async Task<List<CustomerDeliveryDTO>> GetValidCustomerDelivery() => await _customerDeliverRepository.GetValidCustomerDelivery();

        public async Task SendDvdToCustomer(int susubscriptionId, string code,int dvdCatalogId) => await _customerDeliverRepository.SendDvdToCustomer(susubscriptionId, code, dvdCatalogId);

        public async Task<List<CustomerReturnDTO>> GetDvdstatuses() => await _customerDeliverRepository.GetDvdstatuses();

        public async Task ReturnDVDFromCustomer(int susubscriptionId, string code, int dvdCatalogId) => await _customerDeliverRepository.ReturnDVDFromCustomer(susubscriptionId, code, dvdCatalogId);
    }
}
