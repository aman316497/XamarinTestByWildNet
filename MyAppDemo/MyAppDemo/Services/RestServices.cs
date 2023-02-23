using MyAppDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyAppDemo.Services
{
    public class RestServices
    {
        async public Task<List<QuestionModel>> GetQuestionsAsync()
        {
            try
            {
                HttpClient http = new HttpClient();
                http.BaseAddress = new Uri("https://test-vml.free.beeceptor.com/questions");
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, http.BaseAddress);
                var response = await http.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    ResponseModel model = JsonConvert.DeserializeObject<ResponseModel>(jsonResponse);
                    return model.questions;
                }
                else
                    throw new Exception();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
