using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace OptWebApi.Formatters
{
    public class JsonInputFormatter: TextInputFormatter
    {
        private readonly NewtonPool _arrayPool;
        private readonly JsonSerializer _serializer;

        public JsonInputFormatter(ArrayPool<char> arrayPool): base()
        {
            _arrayPool = new NewtonPool(arrayPool);
            
            SupportedMediaTypes.Add("application/json");
            SupportedEncodings.Add(Encoding.UTF8);

            _serializer = new JsonSerializer();
        }
        
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            using (var sw = new StreamReader(context.HttpContext.Request.Body, encoding))
            using (var jw = new JsonTextReader(sw))
            {
                jw.ArrayPool = _arrayPool;
                
                var model = _serializer.Deserialize(jw, context.ModelType);
                
                return Task.FromResult(InputFormatterResult.Success(model));
            }
        }
    }
}
