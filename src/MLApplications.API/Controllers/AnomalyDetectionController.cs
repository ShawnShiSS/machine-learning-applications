using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using MLApplications.API.Models.AnomalyDetection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MLApplications.API.Controllers
{
    /// <summary>
    ///     Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnomalyDetectionController : ControllerBase
    {

        // Data file path and dataset size
        private string _dataPath = Path.Combine(Environment.CurrentDirectory, "Datasets", "intraday_5min_TSLA.csv");
        // assign the Number of records in dataset file to constant variable
        private const int _docsize = 79;

        /// <summary>
        ///     Controller ctor
        /// </summary>
        public AnomalyDetectionController() { }

        // GET: api/AnomalyDetection/SpikeDetection?stock=TSLA
        /// <summary>
        ///     Return spike detection results
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        [HttpGet("SpikeDetection")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IEnumerable<MLApplications.API.Models.AnomalyDetection.ModelOutput>> GetSpikeDetectionPredictions(string stock)
        {
            List<MLApplications.API.Models.AnomalyDetection.ModelOutput> response = new List<Models.AnomalyDetection.ModelOutput>();

            // Create MLContext to be shared across the model creation workflow objects
            MLContext mlContext = new MLContext();

            // STEP 1: Common data loading configuration
            // For real-time prediction, call financial data API provider instead of loading data from text file
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(path: _dataPath, hasHeader: true, separatorChar: ',');

            // Spike detects pattern temporary changes
            return DetectSpike(mlContext, _docsize, dataView);
        }

        private IEnumerable<ModelOutput> DetectSpike(MLContext mlContext, int docSize, IDataView dataView)
        {
            // STEP 2: Set the training algorithm
            var iidSpikeEstimator = mlContext.Transforms.DetectIidSpike(outputColumnName: nameof(ModelOutput.Prediction), inputColumnName: nameof(ModelInput.Price), confidence: 95, pvalueHistoryLength: docSize / 4);
             
            // STEP 3: Create the transform
            // Create the spike detection transform
            ITransformer iidSpikeTransform = iidSpikeEstimator.Fit(CreateEmptyDataView(mlContext));

            //Apply data transformation to create predictions.
            IDataView transformedData = iidSpikeTransform.Transform(dataView);

            var predictions = mlContext.Data.CreateEnumerable<ModelOutput>(transformedData, reuseRowObject: false);

            Console.WriteLine("Alert\tStock Price\tP-Value");

            foreach (var p in predictions)
            {
                var results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}";

                if (p.Prediction[0] == 1)
                {
                    results += " <-- Spike detected";
                }

                Console.WriteLine(results);
            }
            Console.WriteLine("");

            return predictions;
        }


        private IDataView CreateEmptyDataView(MLContext mlContext)
        {
            // Create empty DataView. We just need the schema to call Fit() for the time series transforms
            IEnumerable<ModelInput> enumerableData = new List<ModelInput>();
            return mlContext.Data.LoadFromEnumerable(enumerableData);
        }
    }
}

