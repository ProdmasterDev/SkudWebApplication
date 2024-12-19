using ControllerDomain.Entities;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.Worker;

namespace SkudWebApplication.Services.Classes
{
    public class AccessGroupComparer : IEqualityComparer<AccessGroupWorker>
    {
        public bool Equals(AccessGroupWorker? x, AccessGroupWorker? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.AccessGroupId == y.AccessGroupId;
        }
        public int GetHashCode(AccessGroupWorker? accessRequest)
        {
            if (ReferenceEquals(accessRequest, null)) return 0;
            return accessRequest == null ? 0 : accessRequest.AccessGroupId.GetHashCode();
        }
    }
}
