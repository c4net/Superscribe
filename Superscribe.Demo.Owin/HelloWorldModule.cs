namespace Superscribe.Demo.Owin
{
    using Superscribe.Owin;

    public class HelloWorldModule : SuperscribeOwinModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = PeformHelloWorld;

            this.Get["TestByName" / (Superscribe.Models.String)"Name"] = TestByName;

            this.Post["Test"] = _ =>
                {
                    var product = _.Bind<Product>();

                    ((OwinRouteData)_).StatusCode = 201;
                    return new { Message = string.Format("Received product {0}", product.Name) };
                };
        }

        private object TestByName(Engine.IModuleRouteData arg)
        {
            return "ByName";
        }

        private object PeformHelloWorld(Engine.IModuleRouteData arg)
        {
            return "Hello world";
        }
    }
}