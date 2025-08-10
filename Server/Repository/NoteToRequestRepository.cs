using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class NoteToRequestRepository : INoteToRequestRepository, ITransientService
    {
        private readonly DesignRequestContext _db;

        public NoteToRequestRepository(DesignRequestContext context)
        {
            _db = context;
        }

        public IEnumerable<NoteToRequest> GetNoteToRequests(int designRequestId)
        {
            return _db.NoteToRequest.Where(item => item.DesignRequestId == designRequestId);
        }

        public NoteToRequest GetNoteToRequest(int noteId)
        {
            return _db.NoteToRequest.Find(noteId);
        }

        public NoteToRequest AddNoteToRequest(NoteToRequest noteToRequest)
        {
            _db.NoteToRequest.Add(noteToRequest);
            _db.SaveChanges();
            return noteToRequest;
        }

        public NoteToRequest UpdateNoteToRequest(NoteToRequest noteToRequest)
        {
            _db.Entry(noteToRequest).State = EntityState.Modified;
            _db.SaveChanges();
            return noteToRequest;
        }

        public void DeleteNoteToRequest(int noteId)
        {
            NoteToRequest noteToRequest = _db.NoteToRequest.Find(noteId);
            _db.NoteToRequest.Remove(noteToRequest);
            _db.SaveChanges();
        }
    }
}