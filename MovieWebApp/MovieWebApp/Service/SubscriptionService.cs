using MovieWebApp.DTO;
using MovieWebApp.Repository;
using MovieWebApp.Models;
using AutoMapper;

namespace MovieWebApp.Service
{
    public class SubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public SubscriptionService(IMapper mapper, ISubscriptionRepository subscriptionRepository)
        {
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<IEnumerable<SubscriptionDTO>> GetAll()
        {
            IEnumerable<Subscription> subscriptions = await _subscriptionRepository.GetAll();
            return _mapper.Map<List<SubscriptionDTO>>(subscriptions);
        }

        public async Task<SubscriptionDTO> Save(SubscriptionDTO subscriptionDTO)
        {
            var subscription = await _subscriptionRepository.Save(_mapper.Map<Subscription>(subscriptionDTO));
            return _mapper.Map<SubscriptionDTO>(subscription);
        }

        public async Task<SubscriptionDTO> Update(int subscriptionId, SubscriptionDTO subscriptionDTO)
        {
            var subscription = await _subscriptionRepository.Update(subscriptionId, _mapper.Map<Subscription>(subscriptionDTO));
            return _mapper.Map<SubscriptionDTO>(subscription);
        }

        public async Task<SubscriptionDTO> GetSubscription(int subscriptionId)
        {
            return _mapper.Map<SubscriptionDTO>(await _subscriptionRepository.Get(subscriptionId));
        }

        public async Task Delete(int subcriptionId)
        {
            await _subscriptionRepository.Delete(subcriptionId);
        }

        public async Task<CustomerSubscription> Subscribe(CustomerSubscriptionDTO customerSubscriptionDTO)
        {

            var customerSub = await _subscriptionRepository.Subscribe(_mapper.Map<CustomerSubscription>(customerSubscriptionDTO),_mapper.Map<Payment>(customerSubscriptionDTO.payment));
            return customerSub;
        }

        public async Task<CustomerSubscriptionDTO> GetAvailableScription(int customerId, DateTime date)
        {
            return _mapper.Map<CustomerSubscriptionDTO>( await _subscriptionRepository.GetAvailableScription(customerId,date));
        }
        public async Task<IEnumerable<DvdStatusDTO>> GetCustomerDvdStatus(int customerSubId)
        {
            return await _subscriptionRepository.GetCustomerDvdStatus(customerSubId);
        }
    }
}
