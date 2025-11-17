using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // entity -> dto
            CreateMap<Event, EventDto>();
            CreateMap<TicketType, TicketTypeDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // dto -> entity
            CreateMap<CreateEventDto, Event>();
            CreateMap<CreateBookingDto, Booking>();
        }
    }
}
