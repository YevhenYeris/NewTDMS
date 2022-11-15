using Microsoft.AspNetCore.Routing;
using NewTDBMS.API.Controllers;
using NewTDBMS.API.Hateoas.Models;
using NewTDBMS.API.Hateoas.Services.Abstractions;

namespace NewTDBMS.API.Hateoas.Services;

public class ColumnLinkGetter : LinkGetterBase
{
    public ColumnLinkGetter(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
        : base(linkGenerator, httpContextAccessor)
    {
    }

    public IEnumerable<Link> GetLinks(string dbName, string tableName, string columnName)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName, tableName, columnName }),
                "self",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName, tableName, columnName }),
                "delete column",
                "DELETE"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Post",  values: new { dbName, tableName }),
                "create column",
                "POST")
        };
        return links;
    }
}
