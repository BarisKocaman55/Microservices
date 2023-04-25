using AutoMapper;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Domain.Order.Aggragate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Domain.Order.Aggragate.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<AddressDto, AddressDto>().ReverseMap();
        }
    }
}
