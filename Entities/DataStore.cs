using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class DataStore
	{
		private List<Language> languages;
		private List<SearchEngine> searchEngines;

		public List<Language> Languages { get => languages; }
		public List<SearchEngine> SearchEngines { get => searchEngines; }

		public async Task FillLanguagesAsync()
		{
			languages = new List<Language>();
			await Task.Run(() =>
			{
				Languages.Add(new Language()
				{
					Name = "C Sharp",
					QueryParameter = "C#"
				});
				Languages.Add(new Language()
				{
					Name = "Javascript",
					QueryParameter = "javascript"
				});
				Languages.Add(new Language()
				{
					Name = "Kotlin",
					QueryParameter = "kotlin"
				});
				Languages.Add(new Language()
				{
					Name = "Python",
					QueryParameter = "python"
				});
				Languages.Add(new Language()
				{
					Name = "Java",
					QueryParameter = "java"
				});
				Languages.Add(new Language()
				{
					Name = "Dart",
					QueryParameter = "dart"
				});
			});
		}


		public async Task FillEnginesAsync()
		{
			searchEngines = new List<SearchEngine>();
			await Task.Run(() =>
			{
				SearchEngines.Add(new SearchEngine()
				{
					XPATH = "//div[@id='result-stats']/text()",
					Name = "Google",
					Pattern = "id=\"result-stats\"",
					QueryString = "https://www.google.com/search?q=[parameter]"
				});
				SearchEngines.Add(new SearchEngine()
				{
					XPATH = "//span[contains(@class,'sb_count')]/text()",
					Pattern = "<span[ ]+class=\"([\\w:\\-_ ]*sb_count[\\w:\\-_ ]*)\"",
					Name = "Bing",
					QueryString = "https://www.bing.com/search?q=[parameter]"
				});
				SearchEngines.Add(new SearchEngine()
				{
					XPATH = "//div[contains(@class,'fs-body3')]/text()",
					Pattern = "<div[ ]+class=\"([\\w:\\-_ ]*fs-body3[\\w:\\-_ ]*)\"",
					Name = "StackOverflow",
					QueryString = "https://stackoverflow.com/questions/tagged/[parameter]"
				});
				SearchEngines.Add(new SearchEngine()
				{
					XPATH = "//div[contains(@class,'compPagination')]//span/text()",
					Pattern = "<div[ ]+class=\"([\\w:\\-_ ]*compPagination[\\w:\\-_ ]*)\"##<span",
					Name = "Yahoo",
					QueryString = "https://espanol.search.yahoo.com/search?p=[parameter]"
				});
			});
		}
	}
}
