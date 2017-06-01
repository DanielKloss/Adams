using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TheClockEnd.Data
{
    public class XmlDataService
    {
        public async Task<string> MakeRequestAsync(string dataType)
        {
            HttpWebRequest request = WebRequest.Create(string.Format("https://raw.githubusercontent.com/DanielKloss/Adams/master/Data/{0}.xml", dataType)) as HttpWebRequest;
            request.ContentType = "application/xml";
            request.Method = "GET";

            string responseData = string.Empty;
            HttpWebResponse response;

            try
            {
                response = await request.GetResponseAsync() as HttpWebResponse;
            }
            catch (WebException)
            {
                return string.Empty;
            }

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return await MakeRequestAsync(dataType);
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                using (response)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        //Read data
                        responseData = await reader.ReadToEndAsync();
                    }
                }

                return responseData;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
