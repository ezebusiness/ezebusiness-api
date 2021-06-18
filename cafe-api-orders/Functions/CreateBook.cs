using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using cafe_api_orders.Dtos;
using cafe_api_orders.Interfaces;

namespace cafe_api_orders.Functions.Books
{
    public class CreateBook
    {
        private readonly ILogger<CreateBook> _logger;
        private readonly IBookService _bookService;

        public CreateBook(
            ILogger<CreateBook> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [FunctionName(nameof(CreateBook))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Book")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                var reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var reqObject = JsonConvert.DeserializeObject<BookDto>(reqBody);

                await  _bookService.Create(reqObject);

                result = new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
