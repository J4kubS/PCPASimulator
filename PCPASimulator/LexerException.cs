using System;

namespace PCPASimulator
{
	/// <summary>
	/// Represents errors that occur during lexical analysis.
	/// </summary>
	public class LexerException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.LexerException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public LexerException(string message)
			: this(message, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.LexerException"/> class.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="linePosition">Line position indicating where the error occured.</param>
		public LexerException(string format, int linePosition)
			: base(string.Format(format, linePosition))
		{
			LinePosition = linePosition;
		}

		/// <summary>
		/// Gets the line position indicating where the error occured.
		/// </summary>
		public int LinePosition { get; private set; }
	}
}