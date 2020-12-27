using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
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
        //private readonly ConsumeModel _consumeModel;
        // Use a pool of prediction engines instead of one singleton instance
        PredictionEnginePool<MLApplications.SentimentAnalysis.ModelInput, MLApplications.SentimentAnalysis.ModelOutput> _predictionEnginePool;

        /// <summary>
        ///     Controller ctor
        /// </summary>
        /// <param name="consumeModel"></param>
        public SentimentAnalysisController(PredictionEnginePool<MLApplications.SentimentAnalysis.ModelInput, MLApplications.SentimentAnalysis.ModelOutput> predictionEnginePool)
        {
            this._predictionEnginePool = predictionEnginePool ?? throw new ArgumentNullException(nameof(predictionEnginePool));
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
            var predictionResult = _predictionEnginePool.Predict(modelName: "SentimentAnalysisModel", example: sampleData);

            return predictionResult;
        }

    }
}

