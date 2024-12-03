using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace SimpleAzureFunction
{
    public class Function1
    {
        private static string? _message;

        [Function("PostEmail")]
        public async Task<HttpResponseData> PostEmail(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Please provide an email!");
                return badResponse;
            }

            _message = $"Hello {requestBody}, this message is from AzureFunction";

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(_message);
            return response;
        }

        [Function("GetMessage")]
        public HttpResponseData GetMessage(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            if (string.IsNullOrEmpty(_message))
            {
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                notFoundResponse.WriteString("No message available. Please send a POST request first.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            response.WriteString(_message);
            return response;
        }
    }
}
