using System.Collections.Generic;


namespace MachineLearning.Prediction
{
    public class PredictionResult
    {
        public IReadOnlyList<string> ClassLabel { get; private set; }
        public IDictionary<string, float> Loss { get; private set; }

        public PredictionResult(
            IReadOnlyList<string> classLabel,
            IDictionary<string, float> loss)
        {
            this.ClassLabel = classLabel;
            this.Loss = loss;
        }

    }
}
