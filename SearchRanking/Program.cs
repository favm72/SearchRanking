using Logic;
using System;
using System.Threading.Tasks;

namespace SearchRanking
{
	class Program
	{
		static void Main(string[] args)
		{
			MainAsync(args).GetAwaiter().GetResult();
		}

		static async Task MainAsync(string[] args)
		{
			var langRank = new LangRank();
			await langRank.Execute();

			TableHelper table;

			table = new TableHelper(15, langRank.mainHeaders, langRank.mainRows);
			table.RenderTable();

			table = new TableHelper(15, langRank.summaryHeaders, langRank.summaryRows);
			table.RenderTable();

			Console.ReadLine();
		}
	}
}
