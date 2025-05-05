using AutoMapper;
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
using Hotels.Presentation.DtoBs;
using Hotels.Presentation.DtoBs.Comforts;
using Hotels.Presentation.DtoBs.Common;
using Hotels.Presentation.DtoBs.Contacts;
using Hotels.Presentation.DtoBs.Feeds;
using Hotels.Presentation.DtoBs.Images;
using Hotels.Presentation.DtoBs.PaidServices;
using Hotels.Presentation.DtoBs.Regions;
using Hotels.Presentation.DtoBs.Reviews;
using Hotels.Presentation.DtoBs.Subobjects;
using Hotels.Presentation.DtoBs.Types;
using Hotels.Presentation.DtoBs.Types.Subobjects;
using Hotels.Presentation.DtoBs.Users;

namespace Hotels.Presentation.Mappers;

public class ApplicationDtoBMapperProfile : Profile
{
    public ApplicationDtoBMapperProfile()
    {
        CreateMap<PartnerDtoB, Partner>();
        CreateMap<TravelAgentDtoB, TravelAgent>();
        CreateMap<HousingDtoB, Housing>();
        CreateMap<RoomDtoB, Room>();
        CreateMap<BookingDtoB, Booking>();
        CreateMap<ApplicationObjectContactDtoB, ApplicationObjectContact>();
        CreateMap<CafeContactDtoB, CafeContact>();
        CreateMap<ApplicationBaseEntityDtoB, Feed>();
        CreateMap<ApplicationBaseEntityDtoB, ObjectComfort>();
        CreateMap<ApplicationBaseEntityDtoB, SubobjectComfort>();
        CreateMap<ObjectTypeDtoB, ObjectType>();
        CreateMap<NearbyDtoB, Nearby>();
        CreateMap<ApplicationBaseEntityDtoB, ObjectFeed>();
        CreateMap<CityDtoB, City>();
        CreateMap<PartnerReviewDtoB, PartnerReview>();
        CreateMap<ToiletDtoB, Toilet>();
        CreateMap<BathroomDtoB, Bathroom>();
        CreateMap<CountrySubjectDtoB, CountrySubject>();
        CreateMap<HousingTypeDtoB, HousingType>();
        CreateMap<RoomTypeDtoB, RoomType>();
        CreateMap<SubobjectComfortDtoB, SubobjectComfort>();
        CreateMap<SubobjectFeedDtoB, SubobjectFeed>();
        CreateMap<TouristDtoB, Tourist>();

        CreateMap<TitledImageDtoB, ObjectImageLink>();
        CreateMap<TitledImageDtoB, ReviewImageLink>();
        CreateMap<TitledImageDtoB, SubobjectImageLink>();
        CreateMap<TitledImageDtoB, TourImageLink>();
        CreateMap<TitledImageDtoB, AttractionImageLink>();
        CreateMap<TitledImageDtoB, CafeImageLink>();

        CreateMap<ObjectComfortDtoB, ObjectComfort>();
        CreateMap<ObjectFeedDtoB, ObjectFeed>();
        CreateMap<TourDtoB, Tour>();
        CreateMap<TourTypeDtoB, TourType>();
        CreateMap<TourReviewDtoB, TourReview>();
        CreateMap<SubobjectWeekRateDtoB, SubobjectWeekRate>();
        CreateMap<WeekRateDtoB, WeekRate>();
        CreateMap<TravelAgentTimeLimitedPaidServiceDtoB, TravelAgentTimeLimitedPaidService>();
        CreateMap<CafeTimeLimitedPaidServiceDtoB, CafeTimeLimitedPaidService>();
        CreateMap<AttractionDtoB, Attraction>();
        CreateMap<CafeDtoB, Cafe>();
    }
}
