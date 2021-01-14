using Entities;
using Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
	public class LangRank
	{
		IRankProvider rankProvider;
		DataStore dataStore;
		List<SingleResult> results;
		List<Task> requests;

		public List<string> mainHeaders;
		public List<List<string>> mainRows;

		public List<string> summaryHeaders;
		public List<List<string>> summaryRows;

		public LangRank()
		{
			dataStore = new DataStore();
			//rankProvider = new AgilityPackProvider();
			rankProvider = new RegexSearchProvider();
			requests = new List<Task>();
			results = new List<SingleResult>();
		}

		async Task GetData()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			await Task.WhenAll(dataStore.FillEnginesAsync(), dataStore.FillLanguagesAsync());
			stopwatch.Stop();
			Console.WriteLine($"'FAKE FETCH DATA' : Time elpased {stopwatch.ElapsedMilliseconds} ms");
		}

		public async Task MakeRequests()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			foreach (var lang in dataStore.Languages)
			{
				foreach (var engine in dataStore.SearchEngines)
				{
					SingleResult singleResult = new SingleResult();
					singleResult.Engine = engine;
					singleResult.Language = lang;
					results.Add(singleResult);
					requests.Add(rankProvider.GetResultCount(singleResult));
				}
			}
			await Task.WhenAll(requests.ToArray());
			stopwatch.Stop();
			Console.WriteLine($"'REQUESTS' : Time elpased {stopwatch.ElapsedMilliseconds} ms");
		}

		public void CalculateTotals()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			foreach (var lang in dataStore.Languages)
			{
				lang.TotalCount = (from x in results
								   where x.Language.Name == lang.Name
								   select x.ResultsCount).Sum();
			}
			stopwatch.Stop();
			Console.WriteLine($"'COMPUTE TOTALS X LANGUAGE' : Time elpased {stopwatch.ElapsedMilliseconds} ms");
		}

		public void FillMainTable()
		{
			mainHeaders = new List<string>();
			mainRows = new List<List<string>>();
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			//HEADER
			mainHeaders.Add("LANGUAGE");
			foreach (var engine in dataStore.SearchEngines)
			{
				mainHeaders.Add(engine.Name);
			}
			mainHeaders.Add("TOTAL");

			//BODY
			List<string> row;
			foreach (var lang in dataStore.Languages.OrderByDescending(x => x.TotalCount).ToList())
			{
				row = new List<string>();
				row.Add(lang.Name);
				foreach (var engine in dataStore.SearchEngines)
				{
					var singleResult = (from x in results
										where x.Language.Name == lang.Name
										&& x.Engine.Name == engine.Name
										select x).FirstOrDefault();
					
					row.Add(singleResult.ResultsCount.ToString());
				}
				row.Add(lang.TotalCount.ToString());
				mainRows.Add(row);
			}
			stopwatch.Stop();
			Console.WriteLine($"'COMPUTE MAIN TABLE' : Time elpased {stopwatch.ElapsedMilliseconds} ms");
		}

		public async Task Execute()
		{
			await GetData();
			await MakeRequests();
			CalculateTotals();
			FillMainTable();
			FillSummaryTable();
		}

		public void FillSummaryTable()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			summaryHeaders = new List<string>();
			summaryRows = new List<List<string>>();

			summaryHeaders.Add("ENGINE");
			summaryHeaders.Add("WINNER");

			List<string> row;
			foreach (var engine in dataStore.SearchEngines)
			{
				row = new List<string>();
				row.Add(engine.Name);

				var name = (from x in results
							where x.Engine.Name == engine.Name
							orderby x.ResultsCount descending
							select x).FirstOrDefault().Language.Name;

				row.Add(name);
				summaryRows.Add(row);
			}
			
			var winner = (from x in dataStore.Languages
						  orderby x.TotalCount descending
						  select x).FirstOrDefault().Name;

			row = new List<string>();
			row.Add("Total Winner");
			row.Add(winner);
			summaryRows.Add(row);

			stopwatch.Stop();
			Console.WriteLine($"'COMPUTE SUMMARY TABLE' : Time elpased {stopwatch.ElapsedMilliseconds} ms");
		}
	}
}
