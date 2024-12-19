using AutoMapper;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.WorkerGroup;
using SkudWebApplication.Requests.Location;
using ControllerDomain.Entities;
using SkudWebApplication.Requests.Controller;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Requests.User;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Requests.Worker;

namespace SkudWebApplication
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<DB.Card, VM.Card>()
                .ForMember(x => x.CardNumber, x => x.MapFrom(y => y.CardNumb))
                .ForMember(x => x.CardNumber16, x => x.MapFrom(y => y.CardNumb16))
                .IncludeMembers(x => x.Worker);
            CreateMap<DB.Worker, VM.Card>(MemberList.None)
                .ForMember(x => x.WorkerId, x => x.MapFrom(y => y.Id));
            CreateMap<DB.Controller, VM.Controller>()
                .ForMember(x => x.LocationId, x => x.MapFrom(y => y.ControllerLocationId))
                .ForMember(x => x.LocationName, x => x.MapFrom(y => (y.ControllerLocation == null) ? string.Empty : y.ControllerLocation.Name));
            CreateMap<DB.Event, VM.Event>()
                .ForMember(x => x.CardNumber16, x => x.MapFrom(y => y.Card))
                .ForMember(x => x.LastName, x => x.MapFrom(y => y.Worker!.LastName))
                .ForMember(x => x.FirstName, x => x.MapFrom(y => y.Worker!.FirstName))
                .ForMember(x => x.FatherName, x => x.MapFrom(y => y.Worker!.FatherName))
                .ForMember(x => x.LocationName, x => x.MapFrom(y => y.ControllerLocation!.Name))
                .ForMember(x => x.EventTypeName, x => x.MapFrom(y => y.EventType!.Name));
            CreateMap<DB.ControllerLocation, VM.Location>()
                .ForMember(x => x.ControllerId, x => x.MapFrom(y => (y.Controller == null) ? 0 : y.Controller.Id))
                .ForMember(x => x.ControllerSn, x => x.MapFrom(y => (y.Controller == null) ? string.Empty : y.Controller.Sn))
                .ForMember(x => x.ControllerIp, x => x.MapFrom(y => (y.Controller == null) ? string.Empty : y.Controller.IpAddress));
            CreateMap<DB.User, VM.User>();
            CreateMap<DB.Worker, VM.Worker>()
                .ForMember(x => x.Cards, x => x.MapFrom(y => y.Cards))
                .ForMember(x => x.Group, x => x.MapFrom(y => (y.Group == null) ? string.Empty : y.Group.Name));
            CreateMap<DB.Card, VM.WorkerCard>()
                .ForMember(x => x.Number, x => x.MapFrom(y => y.CardNumb))
                .ForMember(x => x.Number16, x => x.MapFrom(y => y.CardNumb16));
            CreateMap<DB.AccessMethod, VM.AccessMethod>();
            CreateMap<DB.WorkerGroup, VM.WorkerGroup>();
            CreateMap<DB.AccessGroup, VM.AccessGroup>();

            CreateMap<DB.WorkerGroup, WorkerGroupRequest>().ReverseMap();
            CreateMap<DB.WorkerGroup, AddWorkerGroupRequest>().ReverseMap();
            CreateMap<DB.WorkerGroup, EditWorkerGroupRequest>().ReverseMap();
            CreateMap<DB.GroupAccess, AccessRequest>()
                .ForMember(x => x.LocationName, x => x.MapFrom(y => y.ControllerLocation!.Name))
                .ForMember(x => x.Both, x => x.MapFrom(y => y.Enterance && y.Exit));
            CreateMap<AccessRequest, DB.GroupAccess>();

            CreateMap<DB.ControllerLocation, LocationRequest>().ReverseMap();
            CreateMap<DB.ControllerLocation, AddLocationRequest>().ReverseMap();
            CreateMap<DB.ControllerLocation, EditLocationRequest>();
            CreateMap<EditLocationRequest, DB.ControllerLocation>().ReverseMap();
            CreateMap<DB.Controller, LocationController>().ForMember(x => x.Ip, x => x.MapFrom(y => y.IpAddress)).ReverseMap();

            CreateMap<DB.Controller, ControllerRequest>().ReverseMap();
            CreateMap<DB.Controller, EditControllerRequest>().ReverseMap();
            CreateMap<DB.Controller, DeleteControllerRequest>().ReverseMap();

            CreateMap<DB.AccessGroup, AccessGroupRequest>().ReverseMap();
            CreateMap<DB.AccessGroup, AddAccessGroupRequest>().ReverseMap();
            CreateMap<DB.AccessGroup, EditAccessGroupRequest>().ReverseMap();
            CreateMap<DB.AccessGroupAccess, AccessRequest>()
                .ForMember(x => x.LocationName, x => x.MapFrom(y => y.ControllerLocation!.Name))
                .ForMember(x => x.Both, x => x.MapFrom(y => y.Enterance && y.Exit));
            CreateMap<AccessRequest, DB.AccessGroupAccess>();

            CreateMap<DB.User, UserRequest>().ReverseMap();
            CreateMap<DB.User, AddUserRequest>().ReverseMap();
            CreateMap<DB.User, EditUserRequest>().ReverseMap();
            CreateMap<DB.User, DeleteUserRequest>().ReverseMap();

            CreateMap<DB.Card, CardRequest>().ReverseMap();
            CreateMap<DB.Card, AddCardRequest>().ReverseMap();
            CreateMap<DB.Card, EditCardRequest>().ReverseMap();
            CreateMap<DB.Card, DeleteCardRequest>().ReverseMap();
            CreateMap<DB.Worker, CardWorker>().ReverseMap();

            CreateMap<DB.Worker, WorkerRequest>().ReverseMap();
            CreateMap<DB.Worker, AddWorkerRequest>()
                .ForMember(x => x.DateBlock, x => x.MapFrom(y => (y.DateBlock != null) ? y.DateBlock.Value.ToLocalTime() : y.DateBlock));
            CreateMap<AddWorkerRequest, DB.Worker>()
                .ForMember(x => x.DateBlock, x => x.MapFrom(y => (y.DateBlock != null) ? y.DateBlock.Value.ToUniversalTime() : y.DateBlock));
            CreateMap<DB.Worker, EditWorkerRequest>()
                .ForMember(x => x.DateBlock, x => x.MapFrom(y => (y.DateBlock != null) ? y.DateBlock.Value.ToLocalTime() : y.DateBlock));
            CreateMap<EditWorkerRequest, DB.Worker>()
                .ForMember(x => x.DateBlock, x => x.MapFrom(y => (y.DateBlock != null) ? y.DateBlock.Value.ToUniversalTime() : y.DateBlock))
                .ForMember(x => x.Cards, x => x.Ignore());

            CreateMap<DB.Worker, DeleteWorkerRequest>().ReverseMap();
            CreateMap<DB.Access, AccessRequest>()
                .ForMember(x => x.LocationName, x => x.MapFrom(y => y.ControllerLocation!.Name))
                .ForMember(x => x.Both, x => x.MapFrom(y => y.Enterance && y.Exit));
            CreateMap<DB.WorkerAccessGroup, AccessGroupWorker>()
                .ForMember(x => x.AccessGroupName, x => x.MapFrom(y => y.AccessGroup!.Name));
            CreateMap<AccessGroupWorker, DB.WorkerGroupAccess>();
            CreateMap<DB.WorkerGroupAccess, AccessGroupWorker>()
                .ForMember(x => x.AccessGroupName, x => x.MapFrom(y => y.AccessGroup!.Name));
            CreateMap<AccessGroupWorker, DB.WorkerAccessGroup>();
            CreateMap<AccessRequest, DB.Access>();
            CreateMap<DB.Card, WorkerCard>().ReverseMap();
            CreateMap<DB.WorkerGroup, Requests.Worker.WorkerGroup>().ReverseMap();
            CreateMap<DB.AccessMethod, WorkerAccessMethod>().ReverseMap();

            CreateMap<DB.ControllerLocation, Requests.Controller.ControllerLocation>().ReverseMap();

            CreateMap<EditWorkerRequest, RefreshAccessesWorkerRequest>();
            CreateMap<AddWorkerRequest, RefreshAccessesWorkerRequest>();
           // CreateMap<EditWorkerRequest, RefreshAccessGroupWorkerRequest>();
           // CreateMap<AddWorkerRequest, RefreshAccessGroupWorkerRequest>();
            
            CreateMap<DB.Access, DB.QuickAccess>()
                .ForMember(x => x.Sn, x => x.MapFrom(y => y.ControllerLocation.Controller.Sn))
                .ForMember(x => x.Granted, x => x.MapFrom((x, y) => ((x.Enterance && y.Reader == 1) || (x.Exit && y.Reader == 2)) ? 1 : 0))
                .ForMember(x => x.DateBlock, x => x.Ignore());
            CreateMap<DB.GroupAccess, DB.QuickAccess>()
                .ForMember(x => x.Sn, x => x.MapFrom(y => y.ControllerLocation.Controller.Sn))
                .ForMember(x => x.Granted, x => x.MapFrom((x, y) => ((x.Enterance && y.Reader == 1) || (x.Exit && y.Reader == 2)) ? 1 : 0))
                .ForMember(x => x.DateBlock, x => x.Ignore());
            CreateMap<DB.AccessGroupAccess, DB.QuickAccess>()
                .ForMember(x => x.Sn, x => x.MapFrom(y => y.ControllerLocation.Controller.Sn))
                .ForMember(x => x.Granted, x => x.MapFrom((x, y) => ((x.Enterance && y.Reader == 1) || (x.Exit && y.Reader == 2)) ? 1 : 0))
                .ForMember(x => x.DateBlock, x => x.Ignore());

            CreateMap<EditWorkerGroupRequest, RefreshAccessesWorkerRequest>();
        }
    }
}
