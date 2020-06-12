using MachineLearning.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage.Streams;


namespace MachineLearning.Prediction
{
    public class Predictor
    {
        private readonly Model _model;


        protected Predictor(Model model)
        {
            _model = model;
        }


        public static async Task<Predictor> CreateAsync(string onnxFilename)
        {
            Model onnxModel = await Model.CreateFromOnnxFilenameAsync(onnxFilename);
            return new Predictor(onnxModel);
        }


        public async Task<IDictionary<string, float>> GetPredictionAsync(Stream image)
        {
            SoftwareBitmap softwareBitmap;
            
            using (IRandomAccessStream stream = await image.CreateMemoryRandom())
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }

            VideoFrame frame = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);

            return await EvaluateVideoFrameAsync(frame);

        }



        private async Task<IDictionary<string, float>> EvaluateVideoFrameAsync(VideoFrame frame)
        {
            var results = await _model.EvaluateAsync(frame);

            var loss = results.Loss
                .OrderByDescending(x => x.Value)
                .ToDictionary(item => item.Key, item => item.Value);

            return loss;
        }


    }
}
