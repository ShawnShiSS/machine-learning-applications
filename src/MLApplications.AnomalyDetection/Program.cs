using System;
using System.IO;
using Microsoft.ML;
using System.Collections.Generic;

namespace MLApplications.AnomalyDetection
{
    class Program
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Datasets", "intraday_5min_aapl.csv");
        // assign the Number of records in dataset file to constant variable
        const int _docsize = 100;

        static void Main(string[] args)
        {
            // Create MLContext to be shared across the model creation workflow objects
            MLContext mlContext = new MLContext();

            //STEP 1: Common data loading configuration
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(path: _dataPath, hasHeader: true, separatorChar: ',');

            // Spike detects pattern temporary changes
            DetectSpike(mlContext, _docsize, dataView);

            // Changepoint detects pattern persistent changes
            //DetectChangepoint(mlContext, _docsize, dataView);
        }
        static void DetectSpike(MLContext mlContext, int docSize, IDataView dataView)
        {
            Console.WriteLine("Detect temporary changes in pattern");

            // STEP 2: Set the training algorithm
            var iidSpikeEstimator = mlContext.Transforms.DetectIidSpike(outputColumnName: nameof(ModelOutput.Prediction), inputColumnName: nameof(ModelInput.Price), confidence: 95, pvalueHistoryLength: docSize / 4);

            // STEP 3: Create the transform
            // Create the spike detection transform
            Console.WriteLine("=============== Training the model ===============");
            ITransformer iidSpikeTransform = iidSpikeEstimator.Fit(CreateEmptyDataView(mlContext));

            Console.WriteLine("=============== End of training process ===============");
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
        }

        static void DetectChangepoint(MLContext mlContext, int docSize, IDataView productSales)
        {
            Console.WriteLine("Detect Persistent changes in pattern");

            //STEP 2: Set the training algorithm
            // <SnippetAddChangePointTrainer>
            var iidChangePointEstimator = mlContext.Transforms.DetectIidChangePoint(outputColumnName: nameof(ModelOutput.Prediction), inputColumnName: nameof(ModelInput.Price), confidence: 95, changeHistoryLength: docSize / 4);
            // </SnippetAddChangePointTrainer>

            //STEP 3: Create the transform
            Console.WriteLine("=============== Training the model Using Change Point Detection Algorithm===============");
            // <SnippetTrainModel2>
            var iidChangePointTransform = iidChangePointEstimator.Fit(CreateEmptyDataView(mlContext));
            // </SnippetTrainModel2>
            Console.WriteLine("=============== End of training process ===============");

            //Apply data transformation to create predictions.
            // <SnippetTransformData2>
            IDataView transformedData = iidChangePointTransform.Transform(productSales);
            // </SnippetTransformData2>

            // <SnippetCreateEnumerable2>
            var predictions = mlContext.Data.CreateEnumerable<ModelOutput>(transformedData, reuseRowObject: false);
            // </SnippetCreateEnumerable2>

            // <SnippetDisplayHeader2>
            Console.WriteLine("Alert\tScore\tP-Value\tMartingale value");
            // </SnippetDisplayHeader2>

            // <SnippetDisplayResults2>
            foreach (var p in predictions)
            {
                var results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}\t{p.Prediction[3]:F2}";

                if (p.Prediction[0] == 1)
                {
                    results += " <-- alert is on, predicted changepoint";
                }
                Console.WriteLine(results);
            }
            Console.WriteLine("");
            // </SnippetDisplayResults2>
        }

        // <SnippetCreateEmptyDataView>
        static IDataView CreateEmptyDataView(MLContext mlContext)
        {
            // Create empty DataView. We just need the schema to call Fit() for the time series transforms
            IEnumerable<ModelInput> enumerableData = new List<ModelInput>();
            return mlContext.Data.LoadFromEnumerable(enumerableData);
        }
        // </SnippetCreateEmptyDataView>
    }
}
