using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace OptWebApi.Formatters
{
    public class JsonOutputFormatter: TextOutputFormatter
    {
        private readonly NewtonPool _arrayPool;
        private readonly JsonSerializer _serializer;

        public JsonOutputFormatter(ArrayPool<char> arrayPool): base()
        {
            _arrayPool = new NewtonPool(arrayPool);
            
            SupportedMediaTypes.Add("application/json");
            SupportedEncodings.Add(Encoding.UTF8);

            _serializer = new JsonSerializer();
        }
        
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, 
                                                    Encoding selectedEncoding)
        {
            using (var sw = new StreamWriter(context.HttpContext.Response.Body, selectedEncoding))
            using (var jw = new JsonTextWriter(sw))
            {
                jw.ArrayPool = _arrayPool;
                
                _serializer.Serialize(jw, context.Object);
            }
                
            return Task.CompletedTask;
        }
    }
}
