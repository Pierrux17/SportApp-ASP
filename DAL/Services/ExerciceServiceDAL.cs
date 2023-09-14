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
    public class ExerciceServiceDAL : IExerciceRepositoryDAL
    {
        private string url = "https://localhost:7223/";

        private HttpClient _client;

        public ExerciceServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }
        public void Create(ExerciceDAL e)
        {
            string jsonBody = JsonConvert.SerializeObject(e);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/Exercice", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }

        public void Delete(ExerciceDAL e)
        {
            using (HttpResponseMessage message = _client.DeleteAsync("api/Exercice/" + e.Id).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public IEnumerable<ExerciceDAL> GetAll()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/Exercice").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<ExerciceDAL>>(json);
            }
        }

        public ExerciceDAL GetById(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/Exercice/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<ExerciceDAL>(json);
            }
        }

        public void Update(ExerciceDAL e)
        {
            string jsonBody = JsonConvert.SerializeObject(e);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpResponseMessage message = _client.PutAsync("api/Exercice/" + e.Id, content).Result)
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

            var response = await _client.GetAsync("/api/Exercice");

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
