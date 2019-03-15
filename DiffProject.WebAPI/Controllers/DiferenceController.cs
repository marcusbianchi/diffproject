using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiffProject.WebAPI.Controllers
{

    [Route("v1/")]

    public class DiferenceController : Controller
    {
        private readonly IProcessDataService _processDataService;
        private readonly IComparisonRepository _comparisonRepository;
        public DiferenceController(IProcessDataService processDataService, IComparisonRepository comparisonRepository)
        {
            _processDataService = processDataService;
            _comparisonRepository = comparisonRepository;

        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            return Ok(_comparisonRepository.GetResultByContentId(id));
        }

        [HttpPost("{id}/{direction:regex(^(right|left)$)}")]
        public async Task<ActionResult> Post(string id, string direction)
        {
            var body = new StreamReader(Request.Body);
            //The modelbinder has already read the stream and need to reset the stream index
            body.BaseStream.Seek(0, SeekOrigin.Begin);
            var content = body.ReadToEnd();
            var result = await _processDataService.ProcessDataAsync(content, id, direction);
            if (result)
                return Ok();
            return BadRequest("There is already a document with this id in this direction.");
        }


    }
}