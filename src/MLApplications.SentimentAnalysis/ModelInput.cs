using Microsoft.ML.Data;

namespace MLApplications.SentimentAnalysis
{
    public class ModelInput
    {
        [ColumnName("Comment"), LoadColumn(0)]
        public string Comment { get; set; }


        [ColumnName("Label"), LoadColumn(1)]
        public string Label { get; set; }


    }
}
