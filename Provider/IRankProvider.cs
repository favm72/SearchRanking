using Entities;
using System.Threading.Tasks;

namespace Provider
{
	public interface IRankProvider
	{
		Task GetResultCount(SingleResult singleResult);
	}
}