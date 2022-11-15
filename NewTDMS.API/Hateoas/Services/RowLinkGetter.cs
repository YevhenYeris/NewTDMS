using Microsoft.AspNetCore.Routing;
using NewTDBMS.API.Controllers;
using NewTDBMS.API.Hateoas.Models;
using NewTDBMS.API.Hateoas.Services.Abstractions;

namespace NewTDBMS.API.Hateoas.Services;

public class RowLinkGetter : LinkGetterBase
{
    public RowLinkGetter(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
        : base(linkGenerator, httpContextAccessor)
    {
    }

    public IEnumerable<Link> GetLinks(string dbName, string tableName, int rowId)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Get", values: new { dbName, tableName }),
                "self",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Post",  values: new { dbName, tableName }),
                "create row",
                "POST"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Post",  values: new { dbName, tableName }),
                "update row",
                "PUT"),
            new Link(_linkGenerator.GetUriByAction(_httpContext, "Post",  values: new { dbName, tableName, rowId }),
                "delete row",
                "DELETE")
        };
        return links;
    }
}
