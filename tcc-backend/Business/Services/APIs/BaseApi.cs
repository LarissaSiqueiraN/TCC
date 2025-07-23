using Business.Intefaces;
using Business.Services.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.APIs
{
    public class BaseApi : BaseService
    {
        public BaseApi(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
        }

        protected async Task<string> SendPostApi(string url,
                                                string jsonData,
                                                HttpClient httpClient = null)
        {
            try
            {
                string resultado = null;
                var clientePost = new HttpClient();
                clientePost.DefaultRequestHeaders.Add("Accept", "*/*");

                if (httpClient != null) clientePost = httpClient;

                StringContent httpContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseApi = await clientePost.PostAsync(url, httpContent);

                if (responseApi.IsSuccessStatusCode)
                {
                    Task<string> jsonResponse = responseApi.Content.ReadAsStringAsync();
                    resultado = jsonResponse.Result;
                }

                var retorno = await responseApi.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(resultado))
                {
                    Notificar("Nenhum retorno foi obtido da API");
                    return null;
                }

                return resultado;
            }
            catch (Exception)
            {
                Notificar("Ocorreu um erro durante a comunicação com a api");
                return null;
            }
        }

        protected async Task<string> SendGetApi(string url,
                                                HttpClient httpClient = null)
        {
            try
            {
                string resultado = null;
                var clienteGet = new HttpClient();
                if (httpClient != null) clienteGet = httpClient;

                HttpResponseMessage responseApi = await clienteGet.GetAsync(url);

                Task<string> response = responseApi.Content.ReadAsStringAsync();

                if (response.IsCompletedSuccessfully)
                {
                    return response.Result;
                }

                return resultado;
            }
            catch (Exception)
            {
                Notificar("Falha na comunicação com o servidor solicitado");
                return null;
            }
        }

    }
}
