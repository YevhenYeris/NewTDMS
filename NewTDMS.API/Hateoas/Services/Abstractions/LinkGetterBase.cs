using NewTDBMS.API.Hateoas.Models;

namespace NewTDBMS.API.Hateoas.Services.Abstractions;

public abstract class LinkGetterBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly LinkGenerator _linkGenerator;
    protected readonly HttpContext _httpContext;

    public LinkGetterBase(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
        _httpContext = _httpContextAccessor.HttpContext!;
    }
}
