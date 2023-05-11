using Grpc.Core;
using GrpcTest.Clients;
using GrpcTest.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GrpcTest.Net48WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/GrpcTest")]
        public async Task<string> GrpcTest()
        {
            try
            {
                var grpcClient = MicroservicesClientComposition.CreateMicroserviceClient<GrpcTestTranslator.GrpcTestTranslatorClient>("bin\\Certificates\\ca.crt", "localhost", 5002);
                var res = await grpcClient.TranslateOneValueAsync(new TranslateOneValueRequest() { Value = "vasya", Section = "section1" });
                return res.Value;
            }
            catch (RpcException ex)
            {
                if (!string.IsNullOrWhiteSpace(ex.Status.Detail))
                    return "RpcException: " + ex.Status.Detail;
                else
                    throw;
            }
        }
    }
}
