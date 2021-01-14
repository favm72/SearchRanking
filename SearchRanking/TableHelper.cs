using System;
using System.Collections.Generic;
using System.Text;

namespace SearchRanking
{
	public class TableHelper
	{
		private int tableWidth;
		private List<string> headers;
		private List<List<string>> rows;
		public TableHelper(int columnWidth, List<string> headers, List<List<string>> rows)
		{
			this.headers = headers;
			this.rows = rows;
			this.tableWidth = columnWidth * headers.Count;
		}

		void PrintLine()
		{
			Console.WriteLine(new string('-', tableWidth));
		}

		void PrintRow(List<string> row)
		{
			int width = (tableWidth - row.Count) / row.Count;
			string strRow = "|";

			foreach (string column in row)
			{
				strRow += AlignRight(column, width) + "|";
			}
			Console.WriteLine(strRow);
		}

		string AlignRight(string text, int width)
		{
			text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

			if (string.IsNullOrEmpty(text))
			{
				return new string(' ', width);
			}
			else
			{
				//return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
				return text.PadLeft(width);
			}
		}

		public void RenderTable()
		{
			PrintLine();
			PrintRow(headers);
			PrintLine();
			foreach (var item in rows)
			{
				PrintRow(item);
			}
			PrintLine();
		}
	}
}
