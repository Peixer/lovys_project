using Core.Calendar.Models;
using Core.Calendar.Repositories;

namespace Core.Calendar.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;

        public AvailabilityService(IAvailabilityRepository availabilityRepository, IUserRepository userRepository)
        {
            _availabilityRepository = availabilityRepository;
            _userRepository = userRepository;
        }

        public async void InsertAvailability(Availability availability, string username)
        {
            availability.User = await _userRepository.FindUserByUsername(username);
            await _availabilityRepository.Insert(availability);
        }
    }
}