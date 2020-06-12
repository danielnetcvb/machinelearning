using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MachineLearning.Infrastructure
{
    public static class UwpStreamExtension
    {
        public static async Task<InMemoryRandomAccessStream> CreateMemoryRandom(this Stream stream)
        {
            IBuffer buffer = null;
            IInputStream inputstream = stream.AsInputStream();

            using (var dataReader = new DataReader(inputstream))
            {
                await dataReader.LoadAsync((uint)stream.Length);
                buffer = dataReader.DetachBuffer();
            }

            var randomAccessStream = new InMemoryRandomAccessStream();

            await randomAccessStream.WriteAsync(buffer);

            return randomAccessStream;
        }

    }
}
