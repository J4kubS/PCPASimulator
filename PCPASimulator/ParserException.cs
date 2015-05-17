using System;

namespace PCPASimulator
{
	/// <summary>
	/// Represents errors that occur during syntactical analysis.
	/// </summary>
	public class ParserException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.ParserException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public ParserException(string message)
			: base(message)
		{
		}
	}
}