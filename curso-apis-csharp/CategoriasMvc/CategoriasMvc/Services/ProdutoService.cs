using CategoriasMvc.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CategoriasMvc.Services;

public class ProdutoService : IProdutoService
{

    private const string apiEndpoint = "/produtos/";// "/api/1/produtos/";

    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _clientFactory;

    private ProdutoViewModel produtoVM;
    private IEnumerable<ProdutoViewModel> produtosVM;

    public ProdutoService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private static void PutTokenInHeaderAuthotization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<IEnumerable<ProdutoViewModel>> GetProdutos(string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode) // 200 ate 299 = OK (true)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                produtosVM = await JsonSerializer.DeserializeAsync<IEnumerable<ProdutoViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return produtosVM;
    }

    public async Task<ProdutoViewModel> GetProdutoPorId(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode) // 200 ate 299 = OK (true)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                produtoVM = await JsonSerializer.DeserializeAsync<ProdutoViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return produtoVM;
    }

    public async Task<ProdutoViewModel> CriarProduto(ProdutoViewModel produtoVM, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthotization(token, client);

        var produto = JsonSerializer.Serialize(produtosVM);
        StringContent content = new StringContent(produto, Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode) // 200 ate 299 = OK (true)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                produtoVM = await JsonSerializer.DeserializeAsync<ProdutoViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return produtoVM;
    }

    public async Task<bool> AtualizarProduto(int id, ProdutoViewModel produtoVM, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.PutAsJsonAsync(apiEndpoint + id, produtosVM))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
   

    public async Task<bool> DeletaProduto(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

}
