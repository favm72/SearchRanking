using Entities;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Provider
{
	public class AgilityPackProvider : IRankProvider
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
			
				HtmlDocument htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(responseString);

				HtmlNode container = htmlDocument.DocumentNode.SelectSingleNode(singleResult.Engine.XPATH);
				
				if (container != null)
				{
					string content = container.InnerText;
					content = Regex.Replace(content, @"[^\d]", "");
					singleResult.ResultsCount = double.Parse(content);
				}
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
