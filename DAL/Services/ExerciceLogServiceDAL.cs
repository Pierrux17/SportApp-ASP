using DAL.Entities;
using DAL.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ExerciceLogServiceDAL : IExerciceLogRepositoryDAL
    {
        private string url = "https://localhost:7223/";

        private HttpClient _client;

        public ExerciceLogServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }

        public void Create(ExerciceLogDAL e)
        {
            string jsonBody = JsonConvert.SerializeObject(e);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/ExerciceLog", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }

        public void Delete(ExerciceLogDAL e)
        {
            using (HttpResponseMessage message = _client.DeleteAsync("api/ExerciceLog/" + e.Id).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public IEnumerable<ExerciceLogDAL> GetAll()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/ExerciceLog").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<ExerciceLogDAL>>(json);
            }
        }

        public ExerciceLogDAL GetById(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/ExerciceLog/getbyid/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<ExerciceLogDAL>(json);
            }
        }

        public IEnumerable<ExerciceLogDAL> GetByIdTrainingLog(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/ExerciceLog/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<ExerciceLogDAL>>(json);
            }
        }

        public void Update(ExerciceLogDAL e)
        {
            string jsonBody = JsonConvert.SerializeObject(e);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpResponseMessage message = _client.PutAsync("api/ExerciceLog/" + e.Id, content).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public async Task CallApiWithJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/TrainingLog");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<string>(json);
            }
            else
            {
                // La requête a échoué, vous pouvez traiter l'erreur ici
                // Par exemple, si la réponse est 401 Unauthorized, cela signifie que le token est invalide ou expiré
                // Vous pouvez également gérer d'autres codes d'erreur ici
            }
        }
    }
}
