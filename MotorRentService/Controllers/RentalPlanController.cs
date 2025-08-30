using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorRentService.Data;

namespace MotorRentService.Controllers
{
    public class RentalPlanController : Controller
    {
        private readonly IRentalPlanRepository _repository;
        private readonly IMapper _mapper;

        public RentalPlanController(IRentalPlanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


    }
}
