using GIBS.Module.DesignRequest.Models;
using System.Collections.Generic;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IFileToRequestRepository
    {
        IEnumerable<FileToRequest> GetFileToRequests(int designRequestId);
        FileToRequest GetFileToRequest(int fileToRequestId);
        FileToRequest AddFileToRequest(FileToRequest fileToRequest);
        FileToRequest UpdateFileToRequest(FileToRequest fileToRequest);
        void DeleteFileToRequest(int fileToRequestId);
    }
}