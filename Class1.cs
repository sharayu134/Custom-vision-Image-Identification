using predictsign;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CSPredictionSample
{
    static class Program
    {
        static void Main()
        {

            string imageFilePath = @"D:\as.jpg";

            string sdn = MakePredictionRequest(imageFilePath).Result;


            var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(sdn);


            var predict = oMycustomclassname.predictions;

            foreach (var pre in predict) {

                var pro = pre.probability;

                 double i = pro * 100;

                if (i > 30) {
                    Console.WriteLine("Cheque Contain sign  "+i+"%");
                }

            }





            //Console.WriteLine(sdn);
            Thread.Sleep(100000);


        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async Task<string> MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", "226f12ea329149718db1847640d07a04");

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/15fd403b-de4a-43e9-80c4-acf5e1192133/image";

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                string cons;

                return (cons = await response.Content.ReadAsStringAsync());


            }
        }
    }
}