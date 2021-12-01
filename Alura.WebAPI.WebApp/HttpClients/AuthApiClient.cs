using Alura.ListaLeitura.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;
        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> PostLoginAsync(LoginModel loginModel)
        {
            var resposta = await _httpClient.PostAsJsonAsync("login",loginModel);
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsStringAsync();
        }
    }
}
