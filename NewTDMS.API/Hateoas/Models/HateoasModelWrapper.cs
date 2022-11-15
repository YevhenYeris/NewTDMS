namespace NewTDBMS.API.Hateoas.Models;

public class HateoasModelWrapper<T> where T : class
{
    public T Model { get; set; }

    public IEnumerable<Link> Links { get; set; }

    public HateoasModelWrapper(T model, IEnumerable<Link> links)
    {
        Model = model;
        Links = links;
    }
}
