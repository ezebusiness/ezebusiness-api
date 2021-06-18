using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using cafe_api_orders.Interfaces;
using System.IO;
using Newtonsoft.Json;
using cafe_common.Models;

namespace cafe_api_orders.Functions.Books
{
    public class DeleteBook
    {
        private readonly ILogger<DeleteBook> _logger;
        private readonly IBookService _bookService;

        public DeleteBook(
            ILogger<DeleteBook> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [FunctionName("DeleteBook")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Book/{id}")] HttpRequest req,
            string id)
        {
            IActionResult result;

            try
            {
                await _bookService.Delete(id);

                result = new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }

        [FunctionName("RemoveAuthors")]
        public async Task<IActionResult> RemoveAuthors(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Book/{id}/Authors")] HttpRequest req,
        string id)
        {
            IActionResult result;

            try
            {
                var input = await new StreamReader(req.Body).ReadToEndAsync();
                var authorsRequest = JsonConvert.DeserializeObject<string[]>(input);

                await _bookService.RemoveFrom(id, q => q.Authors, new Authors() { Ref = "1", Name = "Pogi" });

                result = new StatusCodeResult(StatusCodes.Status202Accepted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
