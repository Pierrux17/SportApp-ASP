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
    public class ProgramServiceDAL : IProgramRepositoryDAL
    {
        private string url = "https://localhost:7223/";

        private HttpClient _client;

        public ProgramServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }
        public void Create(ProgramDAL p)
        {
            string jsonBody = JsonConvert.SerializeObject(p);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/Program", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }

        public void Delete(ProgramDAL p)
        {
            using (HttpResponseMessage message = _client.DeleteAsync("api/Program/" + p.Id).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public IEnumerable<ProgramDAL> GetAll()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/Program").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<ProgramDAL>>(json);
            }
        }

        public ProgramDAL GetById(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/Program/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<ProgramDAL>(json);
            }
        }

        public void Update(ProgramDAL p)
        {
            string jsonBody = JsonConvert.SerializeObject(p);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpResponseMessage message = _client.PutAsync("api/Program/" + p.Id, content).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }

        public ProgramDAL GetLastProgramCreated()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/Program/GetLastProgramCreated").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<ProgramDAL>(json);
            }
        }

        public async Task CallApiWithJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/Program");

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
