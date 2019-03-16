using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiffProject.WebAPI.Controllers
{

    [Route("v1/")]

    public class DiferenceController : Controller
    {
        private readonly IProcessDataService _processDataService;
        private readonly IProcessResultRepository _processResultRepositoryRepository;
        public DiferenceController(IProcessDataService processDataService, IProcessResultRepository processResultRepositoryRepository)
        {
            _processDataService = processDataService;
            _processResultRepositoryRepository = processResultRepositoryRepository;

        }

        /// <summary>
        /// Returns the result of the processing of two files
        /// </summary>
        /// <param name="id">File Id</param>
        /// <returns>The Current ProcessResult</returns>
        /// <response code="204">The file Id don´t exists</response>
        /// <response code="200">The Current ProcessResult</response>    
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProcessResult), 200)]
        [ProducesResponseType(204)]
        public ActionResult Get(string id)
        {
            return Ok(_processResultRepositoryRepository.GetResultByContentId(id));
        }

        /// <summary>
        /// Send File to the processing Queue
        /// </summary>
        /// <param name="id">File Id</param>
        /// <param name="direction">"left" or "right" direction of the file</param>        
        /// <returns>Ok if the file was properly processed</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("{id}/{direction:regex(^(right|left)$)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(string id, string direction)
        {
            Stream stream = new MemoryStream();
            await Request.Body.CopyToAsync(stream);
            var body = new StreamReader(stream);
            //The modelbinder has already read the stream and need to reset the stream index
            body.BaseStream.Seek(0, SeekOrigin.Begin);
            var content = await body.ReadToEndAsync();
            var result = await _processDataService.ProcessDataAsync(content, id, direction);
            if (result)
                return Ok();
            return BadRequest("There is already a document with this id in this direction.");
        }


    }
}