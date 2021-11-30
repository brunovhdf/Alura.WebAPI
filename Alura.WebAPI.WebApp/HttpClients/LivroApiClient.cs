using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;
        public LivroApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<LivroApi> GetLivroAsync(int id)
        {
            HttpResponseMessage resposta = await _httpClient.GetAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<LivroApi>();
        }
        public async Task<byte[]> GetCapaAsync(int id)
        {
            HttpResponseMessage resposta = await _httpClient.GetAsync($"livros/{id}/capa");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsByteArrayAsync();
        }
        public async Task DeleteLivroAsync(int id)
        {
            var resposta = await _httpClient.DeleteAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();
        }
        public async Task<Lista> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            var resposta = await _httpClient.GetAsync($"ListasLeitura/{tipo}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<Lista>();
        }
        public async Task PostLivroAsync(LivroUpload livroUpload)
        {
            HttpContent content = CreateMultipoartFormDataContent(livroUpload);
            var resposta = await _httpClient.PostAsync("livros", content);
            resposta.EnsureSuccessStatusCode();
        }
        public async Task PutLivroAsync (LivroUpload livroUpload)
        {
            HttpContent content = CreateMultipoartFormDataContent(livroUpload);
            var resposta = await _httpClient.PutAsync("livros", content);
            resposta.EnsureSuccessStatusCode();
        }
        private HttpContent CreateMultipoartFormDataContent(LivroUpload livroUpload)
        {
            var content = new MultipartFormDataContent();
            if (livroUpload.Id != 0)
            {
                content.Add(new StringContent(livroUpload.Id.ToString()), AspasDuplas("id"));
            }
            content.Add(new StringContent(livroUpload.Titulo), AspasDuplas("titulo"));
            content.Add(new StringContent(livroUpload.Lista.ParaString()), AspasDuplas("lista"));

            if (!string.IsNullOrEmpty(livroUpload.Subtitulo))
            {
                content.Add(new StringContent(livroUpload.Subtitulo), AspasDuplas("subtitulo"));
            }
            if (!string.IsNullOrEmpty(livroUpload.Resumo))
            {
                content.Add(new StringContent(livroUpload.Resumo), AspasDuplas("resumo"));
            }
            if (!string.IsNullOrEmpty(livroUpload.Autor))
            {
                content.Add(new StringContent(livroUpload.Autor), AspasDuplas("autor"));
            }
            if (livroUpload.Capa != null)
            {
                var imagemContent = new ByteArrayContent(livroUpload.Capa.ConvertToBytes());
                imagemContent.Headers.Add("content-type", "image/png");
                content.Add(imagemContent, AspasDuplas("capa"), AspasDuplas("Capa.png"));
            }

            return content;
        }
        private string AspasDuplas(string valor)
        {
            return $"\"{valor}\"";
        }
    }
}
