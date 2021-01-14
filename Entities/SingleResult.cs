using System;

namespace Entities
{
	public class SingleResult
	{
		public Language Language { get; set; }
		public SearchEngine Engine { get; set; }
		public double ResultsCount { get; set; }
	}
}
