﻿namespace Superscribe.Tests.Owin
{
    using System.IO;
    using System.Net;
    using System.Net.Http;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Newtonsoft.Json;

    using Superscribe.Owin;
    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    
    public class Module : SuperscribeOwinModule
    {
        public Module()
        {
            this.Get["Hello"] = o => "Hello World";
        }
    }

    public abstract class ModuleTests
    {
        protected static TestServer owinTestServer;

        protected static HttpResponseMessage responseMessage;

        protected static HttpClient client;

        protected Establish context = () =>
            {
                owinTestServer = TestServer.Create(
                builder =>
                {
                    var config = new SuperscribeOwinOptions();
                    config.MediaTypeHandlers.Add("application/json", new MediaTypeHandler
                    {
                        Write = (env, o) => env.WriteResponse(JsonConvert.SerializeObject(o)),
                        Read = (env, type) =>
                        {
                            object obj;
                            using (var reader = new StreamReader(env.GetRequestBody()))
                            {
                                obj = JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
                            };

                            return obj;
                        }
                    });

                    var engine = OwinRouteEngineFactory.Create(config);
            
                    builder.UseSuperscribeRouter(engine)
                        .UseSuperscribeHandler(engine);
                });

            client = owinTestServer.HttpClient;
            client.DefaultRequestHeaders.Add("accept", "text/html");
        };
    }

    public class When_scanning_for_modules : ModuleTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello").Await();

        private It should_wire_up_get_handlers =
            () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("Hello World");
    }

    public class When_hitting_a_route_that_is_incomplete: ModuleTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/NotDefined").Await();

        private It should_return_a_404 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }

    public class When_hitting_an_extraneous_route : ModuleTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello/More").Await();

        private It should_return_a_404 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
    
    public abstract class JsonTests : ModuleTests
    {
        private Establish context = () =>
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("accept", "application/json");
            };
    }

    public class When_getting_products : JsonTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Products").Await();

        private It should_return_a_200 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class When_getting_products_by_id : JsonTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Products/1").Await();

        private It should_return_a_200 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class When_getting_products_by_category : JsonTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Products/Fashion").Await();

        private It should_return_a_200 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class When_posting_a_product : JsonTests
    {
        private static Product product;

        private static StringContent content;

        private Establish context = () =>
            {
                product = new Product { Id = 6, Category = "Books", Description = "Dune Messiah" };
                content = new StringContent(JsonConvert.SerializeObject(product));
                content.Headers.ContentType.MediaType = "application/json";
            };

        private Because of = () => responseMessage = client.PostAsync("http://localhost/Products", content).Await();

        private It should_return_a_201 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.Created);

        private It should_return_an_acknowledgment =
           () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("{\"Message\":\"Received product id: 6, description: Dune Messiah\"}");

    }
}
