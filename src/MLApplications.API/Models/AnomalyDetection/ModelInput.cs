using Microsoft.ML.Data;

namespace MLApplications.API.Models.AnomalyDetection
{
    /// <summary>
    ///     Input model for Time Series Stock Price Anamoly Detection
    /// </summary>
    public class ModelInput
    {
        [LoadColumn(0)]
        public string Timestamp;

        [LoadColumn(4)]
        public float Price;
    }
}
