﻿namespace Superscribe.Testing.Controllers
{
    using System.Web.Http;

    public class PortfolioProjectsController : ApiController
    {
        public string Get(int siteId)
        {
            return "Get";
        }

        public string GetById(int siteId, int projectId)
        {
            return "GetById";
        }
    }
}