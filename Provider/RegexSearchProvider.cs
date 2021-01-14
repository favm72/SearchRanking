using Entities;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Provider
{
	public class RegexSearchProvider : IRankProvider
	{
		public async Task GetResultCount(SingleResult singleResult)
		{
			HttpClient client = new HttpClient();
			try
			{
				string url = singleResult.Engine.QueryString;
				url = url.Replace("[parameter]", HttpUtility.UrlEncode(singleResult.Language.QueryParameter));
				client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
				
				var responseString = await client.GetStringAsync(url);
				
				string[] tree = singleResult.Engine.Pattern.Split("##");

				string contentSearch = responseString;

				for (int i = 0; i < tree.Length; i++)
				{
					int startIndex = 0;
					var match = Regex.Match(contentSearch, tree[i]);
					if (match.Success)
					{
						if (i == tree.Length - 1)
						{
							startIndex = contentSearch.IndexOf(">", match.Index);
							int endIndex = contentSearch.IndexOf("<", startIndex);
							contentSearch = contentSearch.Substring(startIndex, endIndex - startIndex);
						}
						else
						{
							startIndex = match.Index;
							contentSearch = contentSearch.Substring(startIndex);
						}
					}
				}

				contentSearch = Regex.Replace(contentSearch, @"[^\d]", "");
				singleResult.ResultsCount = double.Parse(contentSearch);
				
			}
			catch (Exception ex)
			{
				Console.WriteLine("====== error ======");
				Console.WriteLine(ex.Message);
				Console.WriteLine("====== end error ======");
			}
		}
	}
}
