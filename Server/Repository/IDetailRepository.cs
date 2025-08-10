using System.Collections.Generic;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IDetailRepository
    {
        IEnumerable<Detail> GetDetails(int moduleId);
        Detail GetDetail(int detailId);
        Detail AddDetail(Detail detail);
        Detail UpdateDetail(Detail detail);
        void DeleteDetail(int detailId);
    }
}