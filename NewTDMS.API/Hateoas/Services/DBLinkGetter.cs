using Microsoft.AspNetCore.Routing;
using NewTDBMS.API.Controllers;
using NewTDBMS.API.Hateoas.Models;
using NewTDBMS.API.Hateoas.Services.Abstractions;

namespace NewTDBMS.API.Hateoas.Services;

public class DBLinkGetter : LinkGetterBase
{
    public DBLinkGetter(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
        : base(linkGenerator, httpContextAccessor)
    {
    }

    public IEnumerable<Link> GetLinks(string dbName)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName }),
                "delete database",
                "DELETE"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Post",  values: new { dbName }),
                "create database",
                "POST")
        };
        return links;
    }
}
