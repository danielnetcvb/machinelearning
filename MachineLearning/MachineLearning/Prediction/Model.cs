using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;


namespace MachineLearning.Prediction
{
    public class Model
    {
        private LearningModel learningModel;
        private LearningModelSession session;
        private LearningModelBinding binding;

        public static async Task<Model> CreateFromOnnxFilenameAsync(string onnxFilename)
        {
            Model onnxModel = new Model();
            IRandomAccessStreamReference modelFileStream = await StorageFile.GetFileFromPathAsync(onnxFilename);

            onnxModel.learningModel = await LearningModel.LoadFromStreamAsync(modelFileStream);
            onnxModel.session = new LearningModelSession(onnxModel.learningModel);
            onnxModel.binding = new LearningModelBinding(onnxModel.session);

            return onnxModel;
        }

        public async Task<PredictionResult> EvaluateAsync(VideoFrame image)
        {
            binding.Bind("data", image);
            var result = await session.EvaluateAsync(binding, string.Empty);


            var classeLabels = (result.Outputs["classLabel"] as TensorString).GetAsVectorView();
            var losses = result.Outputs["loss"] as IList<IDictionary<string, float>>;

            PredictionResult output = new PredictionResult(classeLabels, losses[0]);

            return output;
        }
    }
}
