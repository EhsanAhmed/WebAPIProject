using Microsoft.AspNetCore.WebUtilities;
using System;
using WebAPIProject.Interfaces;
using WebAPIProject.Models;

namespace WebAPIProject.Repository
{
    public class UriRepository : IUriService
    {
        private readonly string _baseUri;
        public UriRepository(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
