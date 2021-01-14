using Entities;
using NUnit.Framework;
using Provider;
using System.Threading.Tasks;

namespace Testing
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public async Task ProviderTest()
		{
			SingleResult singleResult = new SingleResult();
			singleResult.Engine = new SearchEngine()
			{
				XPATH = "//div[contains(@class,'compPagination')]//span/text()",
				Pattern = "<div[ ]+class=\"([\\w:\\-_ ]*compPagination[\\w:\\-_ ]*)\"##<span",
				Name = "Yahoo",
				QueryString = "https://espanol.search.yahoo.com/search?p=[parameter]"
			};
			singleResult.Language = new Language()
			{
				Name = "C Sharp",
				QueryParameter = "C#"
			};
			RegexSearchProvider regexSearch = new RegexSearchProvider();
			await regexSearch.GetResultCount(singleResult);
			Assert.Pass();
		}
	}
}