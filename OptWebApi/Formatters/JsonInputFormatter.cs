using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

                try
                {
                    var model = _serializer.Deserialize(jw, context.ModelType);

                    return Task.FromResult(InputFormatterResult.Success(model));
                }
                catch (JsonReaderException e)
                {
                    Console.WriteLine(e);

                    context.HttpContext.Response.StatusCode = 500;
                    context.HttpContext.Response.WriteAsync(e.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    context.HttpContext.Response.StatusCode = 500;

                    throw;
                }
                
                
                return Task.FromResult(InputFormatterResult.Failure());
            }
        }
    }
}
