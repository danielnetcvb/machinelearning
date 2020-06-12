using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MachineLearning.Prediction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MachineLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {

        private readonly Predictor _predictor;

        public PredictionController(Predictor prediction)
        {
            _predictor = prediction;
        }

        [HttpPost]
        public async Task<IDictionary<string,float>> DoPrediction([FromForm] IFormFile file)
        {

            using (Stream image = file.OpenReadStream())
            {
                var dictionary = await _predictor.GetPredictionAsync(image);
                return dictionary;
            }
           
        }
    }
}