using AutoMapper;
using Hotels.Application.Dtos;
using Hotels.Application.Dtos.Comforts;
using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.Contacts;
using Hotels.Application.Dtos.Feeds;
using Hotels.Application.Dtos.PaidServices;
using Hotels.Application.Dtos.Regions;
using Hotels.Application.Dtos.Reviews;
using Hotels.Application.Dtos.StaticFiles;
using Hotels.Application.Dtos.Subobjects;
using Hotels.Application.Dtos.Subscriptions;
using Hotels.Application.Dtos.Types;
using Hotels.Application.Dtos.Types.SubobjectTypes;
using Hotels.Application.Dtos.Users;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.Contacts;
using Hotels.Domain.Entities.EntityTypes;
using Hotels.Domain.Entities.Feeds;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.Reviews;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Entities.WeekRates;

namespace Hotels.Application.Mappings;

public class ApplicationDtoMapperProfile : Profile
{
    public ApplicationDtoMapperProfile()
    {
        CreateMap<Partner, PartnerDto>();
        CreateMap<TravelAgent, TravelAgentDto>();

        CreateMap<Housing, HousingDto>();
        CreateMap<Room, RoomDto>();
        CreateMap<Subobject, SubobjectDto>().ReverseMap();
        CreateMap<Booking, BookingDto>();
        CreateMap<ApplicationObjectContact, ApplicationObjectContactDto>();
        CreateMap<CafeContact, CafeContactDto>();
        CreateMap<Feed, ApplicationNamedEntityDto>();
        CreateMap<ObjectComfort, ApplicationNamedEntityDto>();
        CreateMap<SubobjectComfort, ApplicationNamedEntityDto>();
        CreateMap<ObjectType, ObjectTypeDto>();
        CreateMap<Nearby, NearbyDto>();
        CreateMap<ObjectFeed, ApplicationNamedEntityDto>();
        CreateMap<City, CityDto>();
        CreateMap<PartnerReview, PartnerReviewDto>();
        CreateMap<Tourist, TouristDto>();
        CreateMap<Toilet, ToiletDto>();
        CreateMap<Bathroom, BathroomDto>();
        CreateMap<CountrySubject, CountrySubjectDto>();
        CreateMap<HousingType, HousingTypeDto>();
        CreateMap<RoomType, RoomTypeDto>();
        CreateMap<SubobjectComfort, SubobjectComfortDto>();
        CreateMap<SubobjectFeed, SubobjectFeedDto>();
        // ImageLinks
        CreateMap<ObjectImageLink, ObjectImageLinkDto>();
        CreateMap<ReviewImageLink, ReviewImageLinkDto>();
        CreateMap<SubobjectImageLink, SubobjectImageLinkDto>();
        CreateMap<TourImageLink, TourImageLinkDto>();
        CreateMap<AttractionImageLink, AttractionImageLinkDto>();
        CreateMap<CafeImageLink, CafeImageLinkDto>();
        CreateMap<CafeMenuFileLink, CafeMenuFileLinkDto>();
        CreateMap<NearbyImageLink, NearbyImageLinkDto>();

        CreateMap<Toilet, ToiletDto>();
        CreateMap<ObjectComfort, ObjectComfortDto>();
        CreateMap<ObjectFeed, ObjectFeedDto>();
        CreateMap<ObjectType, ObjectTypeDto>();
        CreateMap<Tour, TourDto>();
        CreateMap<TravelAgentLogoLink, TravelAgentLogoLinkDto>();
        CreateMap<TourType, TourTypeDto>();
        CreateMap<TourReview, TourReviewDto>();
        CreateMap<SubobjectWeekRate, SubobjectWeekRateDto>();
        CreateMap<WeekRate, WeekRateDto>();
        CreateMap<TravelAgentTimeLimitedPaidService, TravelAgentTimeLimitedPaidServiceDto>();
        CreateMap<CafeTimeLimitedPaidService, CafeTimeLimitedPaidServiceDto>();
        CreateMap<TravelAgentSubscription, TravelAgentSubscriptionDto>();
        CreateMap<CafeSubscription, CafeSubscriptionDto>();
        CreateMap<Attraction, AttractionDto>();
        CreateMap<Cafe, CafeDto>();
    }
}
