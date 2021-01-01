using Microsoft.ML.Data;

namespace MLApplications.AnomalyDetection
{
    public class ModelOutput
    {
        /// <summary>
        ///     Prediction output, which is a vector to hold alert, score, p-value values.
        ///     For anomaly detection, the prediction consists of 
        ///         - an alert to indicate whether there is an anomaly
        ///         - a raw score, which is the stock price
        ///         - a p-value, the closer the p-value is to 0, the more likely an anomaly has occurred.
        /// </summary>
        [VectorType(3)]
        public double[] Prediction { get; set; }
    }
}
