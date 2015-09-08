namespace Nancy.Demo.Razor.Localization
{
    using Bootstrapper;
    using Nancy.ErrorHandling;
    using Nancy.Responses;
    using Nancy.TinyIoc;

    public class DemoBootstrapper : DefaultNancyBootstrapper
    {
        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(x => x.ResourceAssemblyProvider = typeof(CustomResourceAssemblyProvider));
            }
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            pipelines.OnError += (ctx, ex) =>
            {
                ctx.Items["ON_ERROR"] = "value from OnError";
                return null;
            };
        }
    }

    public class StatusCodeHandler : IStatusCodeHandler
    {
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return true;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            context.Response = new TextResponse(statusCode, (string)context.Items["ON_ERROR"]);
        }
    }
}