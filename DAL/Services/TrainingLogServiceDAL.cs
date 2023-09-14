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
    public class TrainingLogServiceDAL : ITrainingLogRepositoryDAL
    {
        private string url = "https://localhost:7223/";

        private HttpClient _client;

        public TrainingLogServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }

        public void Create(TrainingLogDAL t)
        {
            string jsonBody = JsonConvert.SerializeObject(t);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/TrainingLog", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }

        public void Delete(TrainingLogDAL t)
        {
            using (HttpResponseMessage message = _client.DeleteAsync("api/TrainingLog/" + t.Id).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public IEnumerable<TrainingLogDAL> GetAll()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/TrainingLog").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<TrainingLogDAL>>(json);
            }
        }

        public TrainingLogDAL GetById(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/TrainingLog/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<TrainingLogDAL>(json);
            }
        }

        public IEnumerable<TrainingLogDAL> GetByIdPerson(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/TrainingLog/person/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<TrainingLogDAL>>(json);
            }
        }

        public void Update(TrainingLogDAL t)
        {
            string jsonBody = JsonConvert.SerializeObject(t);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpResponseMessage message = _client.PutAsync("api/TrainingLog/" + t.Id, content).Result)
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
