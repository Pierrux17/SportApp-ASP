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
    public class TrainingExerciceServiceDAL : ITrainingExerciceRepositoryDAL
    {
        private string url = "https://localhost:7223/";

        private HttpClient _client;

        public TrainingExerciceServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }
        public void Create(TrainingExerciceDAL t)
        {
            string jsonBody = JsonConvert.SerializeObject(t);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/TrainingExercice", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }

        public void Delete(TrainingExerciceDAL t)
        {
            using (HttpResponseMessage message = _client.DeleteAsync("api/TrainingExercice/" + t.Id_training + "/" + t.Id_exercice).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public IEnumerable<TrainingExerciceDAL> GetAll()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/TrainingExercice").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<TrainingExerciceDAL>>(json);
            }
        }

        public IEnumerable<TrainingExerciceDAL> GetByIdTraining(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/TrainingExercice/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<TrainingExerciceDAL>>(json);
            }
        }

        public TrainingExerciceDAL GetById(int id_training, int id_exercice)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/TrainingExercice/" + id_training + "/" + id_exercice).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<TrainingExerciceDAL>(json);
            }
        }

        public void Update(TrainingExerciceDAL t)
        {
            string jsonBody = JsonConvert.SerializeObject(t);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpResponseMessage message = _client.PutAsync("api/TrainingExercice/" + t.Id_training + "/" + t.Id_exercice, content).Result)
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

            var response = await _client.GetAsync("/api/TrainingExercice");

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
