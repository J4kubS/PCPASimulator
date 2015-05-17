using System.Collections.Generic;
using System.Linq;

namespace PCPASimulator
{
	/// <summary>
	/// Extensions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Cartesian  product.
		/// </summary>
		/// <remarks>
		/// <see href="http://blogs.msdn.com/b/ericlippert/archive/2010/06/28/computing-a-cartesian-product-with-linq.aspx"/>
		/// </remarks>
		/// <returns>The cartesian product.</returns>
		/// <param name="sequences">Sequences.</param>
		public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
		{ 
			IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

			return sequences.Aggregate(emptyProduct,
				(accumulator, sequence) => 
				from accseq in accumulator
				from item in sequence
				select accseq.Concat(new[] { item }));
		}
	}
}