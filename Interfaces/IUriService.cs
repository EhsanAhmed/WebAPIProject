using System;
using WebAPIProject.Models;

namespace WebAPIProject.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
