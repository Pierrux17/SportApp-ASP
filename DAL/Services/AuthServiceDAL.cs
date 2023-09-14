using DAL.Entities;
using DAL.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class AuthServiceDAL : IAuthRepositoryDAL
    {
        private readonly string url = "https://localhost:7223/";

        private readonly HttpClient _client;

        public string JwtToken { get; set; }

        public AuthServiceDAL(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(url);
        }
        public PersonDAL Login(LoginDAL l)
        {
            string jsonBody = JsonConvert.SerializeObject(l);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpResponseMessage message = _client.PostAsync("api/Person/login", content).Result)
            {
                if (!message.IsSuccessStatusCode)
                    throw new HttpRequestException();

                string json = message.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<PersonDAL>(json);
            }
        }

        public void Register(PersonDAL p)
        {
            string jsonBody = JsonConvert.SerializeObject(p);

            using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage message = _client.PostAsync("api/Auth/register", content).Result)
                {
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                }
            }
        }


        public string GetJwtToken(LoginDAL l)
        {
            var jsonBody = JsonConvert.SerializeObject(l);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = _client.PostAsync("api/Auth/login", content).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erreur lors de l'obtention du token JWT.");
            }

            var json = response.Content.ReadAsStringAsync().Result;
            var tokenObject = JsonConvert.DeserializeObject<TokenResponse>(json);
            JwtToken = tokenObject.token; // Assurez-vous que le nom de la propriété correspond au nom réel du token dans la réponse JSON
            return JwtToken;
        }


        public async Task CallApiWithJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/");

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
