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
    public class PersonProgramServiceDAL : IPersonProgramRepositoryDAL
    {
        private string url = "https://localhost:7223/";

        private HttpClient _client;

        public PersonProgramServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }

        public void Create(PersonProgramDAL p)
        {
            string jsonBody = JsonConvert.SerializeObject(p);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/PersonProgram", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }

        public void Delete(PersonProgramDAL p)
        {
            using (HttpResponseMessage message = _client.DeleteAsync("api/PersonProgram/" + p.Id_person + "/" + p.Id_program).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();
            }
        }


        public IEnumerable<PersonProgramDAL> GetAll()
        {
            using (HttpResponseMessage message = _client.GetAsync("api/PersonProgram").Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<PersonProgramDAL>>(json);
            }
        }

        public IEnumerable<PersonProgramDAL> GetByIdPerson(int id)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/PersonProgram/" + id).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<PersonProgramDAL>>(json);
            }
        }

        public PersonProgramDAL GetById(int id_person, int id_program)
        {
            using (HttpResponseMessage message = _client.GetAsync("api/PersonProgram/" + id_person + "/" + id_program).Result)
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                string json = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<PersonProgramDAL>(json);
            }
        }


        public async Task CallApiWithJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/PersonProgram");

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
