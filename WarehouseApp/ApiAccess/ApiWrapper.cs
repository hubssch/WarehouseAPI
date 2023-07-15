using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WarehouseAPI.Models.Dto;

namespace WarehouseApp.ApiAccess
{
	public class ApiWrapper
	{
		private static HttpClient client = null;
		private static string token = String.Empty;
		public static string Token { set { token = value; } }

		private static void SetHttpClient()
		{
			if (client is null)
			{
				client = new HttpClient();
				client.BaseAddress = new Uri("https://localhost:7167");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
		}

		public static async Task<List<ContractorDto>> GetContactors()
		{
			List<ContractorDto> conts = null;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/contractor"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					conts = await response.Content.ReadAsAsync<List<ContractorDto>>();
				}
			}
			return conts;
		}

		public static async Task<List<DocumentDto>> GetDocuments()
		{
			List<DocumentDto> documents = null;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/doc"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					documents = await response.Content.ReadAsAsync<List<DocumentDto>>();
				}
			}
			return documents;
		}

		public static async Task<DocumentAllDataDto> GetDocument(int id)
		{
			DocumentAllDataDto document = null;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/doc/all/{id}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					document = await response.Content.ReadAsAsync<DocumentAllDataDto>();
				}
			}
			return document;
		}

		public static async Task<List<ArticleDto>> GetArticles()
		{
			List<ArticleDto> articles = null;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/article"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					articles = await response.Content.ReadAsAsync<List<ArticleDto>>();
				}
			}
			return articles;
		}

		public static async Task<LoggedUserRecordDto> Login(string userMail, string password)
		{
			LoggedUserRecordDto auth_data = new LoggedUserRecordDto { IsLogged = false, Message = "Login failed", UserItem = null, Token = String.Empty };
			SetHttpClient();
			HttpResponseMessage response = await client.PostAsJsonAsync("api/auth/login", new LoginDto { Email = userMail, Password = password} );
			if (response.IsSuccessStatusCode)
			{
				auth_data = await response.Content.ReadAsAsync<LoggedUserRecordDto>();
			}
			return auth_data;
		}

		public static async Task<bool> CreateDocument(DocumentAllDataDto docdata)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/doc/all"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var json = JsonConvert.SerializeObject(docdata);
				requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> CreateContract(ContractorDto contract)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/contractor"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var json = JsonConvert.SerializeObject(contract);
				requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> CreateArticle(ArticleDto article)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/article"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var json = JsonConvert.SerializeObject(article);
				requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> RemoveDocument(int id)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"api/doc/{id}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> RemoveContractor(int id)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"api/contractor/{id}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> RemoveArticle(int id)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"api/article/{id}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> UpdateDocument(DocumentAllDataDto doc)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/doc/all/{doc.DocID}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var json = JsonConvert.SerializeObject(doc);
				requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> UpdateContractor(ContractorDto contract)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/contractor/{contract.ContractID}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var json = JsonConvert.SerializeObject(contract);
				requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> UpdateArticle(ArticleDto article)
		{
			bool result = false;
			SetHttpClient();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/article/{article.ArticleID}"))
			{
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var json = JsonConvert.SerializeObject(article);
				requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					result = true;
				}
			}
			return result;
		}
	}
}
