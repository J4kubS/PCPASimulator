using System;

namespace PCPASimulator
{
	/// <summary>
	/// Represents errors that occur during the validation of a XML configuration file
	/// for a parallel communicating pushdown automata system.
	/// </summary>
	public class PCPAValidationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAValidationException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public PCPAValidationException(string message)
			: this(message, 0, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAValidationException"/> class.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="lineNumber">Line number indicating where the error occured.</param>
		/// <param name="linePosition">Line position indicating where the error occured.</param>
		public PCPAValidationException(string format, int lineNumber, int linePosition)
			: base(string.Format(format, lineNumber, linePosition))
		{
			LinePosition = linePosition;
			LineNumber = lineNumber;
		}

		/// <summary>
		/// Gets the line number indicating where the error occured.
		/// </summary>
		public int LineNumber{ get; set; }

		/// <summary>
		/// Gets the line position indicating where the error occured.
		/// </summary>
		public int LinePosition { get; set; }
	}
}