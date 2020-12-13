using Microsoft.AspNetCore.Mvc;
using MLApplications.SentimentAnalysis;
using System;
using System.Threading.Tasks;

namespace MLApplications.API.Controllers
{
    /// <summary>
    ///     Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SentimentAnalysisController : ControllerBase
    {
        private readonly ConsumeModel _consumeModel;

        /// <summary>
        ///     Controller ctor
        /// </summary>
        /// <param name="consumeModel"></param>
        public SentimentAnalysisController(ConsumeModel consumeModel)
        {
            this._consumeModel = consumeModel ?? throw new ArgumentNullException(nameof(consumeModel));
        }

        // GET: api/SentimentAnalysis?comment=I love machine learning!
        /// <summary>
        ///     Return sentiment of a comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ModelOutput> Get(string comment)
        {
            ModelInput sampleData = new ModelInput()
            {
                Comment = @$"{comment}",
            };

            // Make a single prediction
            var predictionResult = _consumeModel.Predict(sampleData);

            return predictionResult;
        }

    }
}

