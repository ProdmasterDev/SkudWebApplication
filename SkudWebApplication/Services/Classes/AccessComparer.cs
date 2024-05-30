using ControllerDomain.Entities;
using SkudWebApplication.Requests;

namespace SkudWebApplication.Services.Classes
{
    public class AccessComparer : IEqualityComparer<AccessRequest>
    {
        public bool Equals(AccessRequest? x, AccessRequest? y)
        {
            if (ReferenceEquals(x, y)) 
                return true;

            if (x is null || y is null)
                return false;

            return x.ControllerLocationId == y.ControllerLocationId;
        }
        public int GetHashCode(AccessRequest? accessRequest)
        {
            if (ReferenceEquals(accessRequest, null)) return 0;
            return accessRequest == null ? 0 : accessRequest.ControllerLocationId.GetHashCode();
        }
    }
}
