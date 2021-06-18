using cafe_api_orders.Interfaces;
using cafe_common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace cafe_api_orders.Functions.Books
{
    public class UpdateBook
    {
        private readonly ILogger<UpdateBook> _logger;
        private readonly IBookService _bookService;

        public UpdateBook(
            ILogger<UpdateBook> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [FunctionName("AddAuthors")]
        public async Task<IActionResult> AddAuthors(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Book/{id}/Authors")] HttpRequest req,
            string id)
        {
            IActionResult result;

            try
            {
                var input = await new StreamReader(req.Body).ReadToEndAsync();

                var replacements = new List<Authors>() {
                    new Authors() { Ref = "2", Name = "Jhon" },
                    new Authors() { Ref = "1", Name = "Pogi" }
                };

                await _bookService.AddTo(id, q => q.Authors, replacements);
                await _bookService.AddTo(id, q => q.Authors, new Authors() { Ref = "3", Name = "Pogiii" });

                result = new StatusCodeResult(StatusCodes.Status202Accepted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }

        [FunctionName("UpdateBookPrice")]
        public async Task<IActionResult> UpdateBookField(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Book/{id}/Price/{price}")] HttpRequest req,
        string id, decimal price)
        {
            IActionResult result;

            try
            {
                var input = await new StreamReader(req.Body).ReadToEndAsync();
                var authorsRequest = JsonConvert.DeserializeObject<string[]>(input);

                await _bookService.UpdateSingleField(id, q => q.Price, price);

                result = new StatusCodeResult(StatusCodes.Status202Accepted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }

        [FunctionName("ReplaceAuthors")]
        public async Task<IActionResult> ReplaceAuthors(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Book/{id}/Authors/Replace")] HttpRequest req,
        string id)
        {
            IActionResult result;

            try
            {
                var input = await new StreamReader(req.Body).ReadToEndAsync();
                var authorsRequest = JsonConvert.DeserializeObject<IList<Authors>>(input);

                await _bookService.Replace(id, q => q.Authors, authorsRequest);

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
