using MovieWebApp.Repository;
using MovieWebApp.DTO;
using AutoMapper;
using MovieWebApp.Models;

namespace MovieWebApp.Service
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper,ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDTO> Save(CustomerDTO customerDTO)
        {
            var customer = await _customerRepository.Save(_mapper.Map<Customer>(customerDTO));
            customerDTO.Id = customer.Id;
            return customerDTO;
        }
        public async Task<CustomerDTO> Get(int customerId)
        {
            return _mapper.Map<CustomerDTO>(await _customerRepository.Get(customerId));
        }

        public async Task<CustomerDTO> Get(string email)
        {
            return _mapper.Map<CustomerDTO>(await _customerRepository.Get(email));
        }

        public async Task<CustomerDTO> Update(int customerId, CustomerDTO customerDTO)
        {
            var customer = await _customerRepository.Update(customerId, _mapper.Map<Customer>(customerDTO));
            return _mapper.Map<CustomerDTO>(customer);
        }
        public async Task<IEnumerable<CustomerDTO>> GetAll(int limit, int pageNumber)
        {
            var customerList = await _customerRepository.GetAll(limit, pageNumber);
            return _mapper.Map<List<CustomerDTO>>(customerList);
        }
    }
}
