using Microsoft.AspNetCore.Routing;
using NewTDBMS.API.Controllers;
using NewTDBMS.API.Hateoas.Models;
using NewTDBMS.API.Hateoas.Services.Abstractions;

namespace NewTDBMS.API.Hateoas.Services;

public class TableLinkGetter : LinkGetterBase
{
    public TableLinkGetter(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
        : base(linkGenerator, httpContextAccessor)
    {
    }

    public IEnumerable<Link> GetLinks (string dbName, string tableName)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Get", values: new { dbName, tableName }),
                "self",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName, tableName }),
                "delete table",
                "DELETE"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", values: new { dbName }),
                "create table",
                "POST"),
        };
        return links;
    }
}
