using System.Collections.Generic;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface INoteToRequestRepository
    {
        IEnumerable<NoteToRequest> GetNoteToRequests(int designRequestId);
        NoteToRequest GetNoteToRequest(int noteId);
        NoteToRequest AddNoteToRequest(NoteToRequest noteToRequest);
        NoteToRequest UpdateNoteToRequest(NoteToRequest noteToRequest);
        void DeleteNoteToRequest(int noteId);
    }
}