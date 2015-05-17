using System;
using System.Collections.Generic;
using System.Linq;

namespace PCPASimulator
{
	/// <summary>
	/// Lexer capable of performing a basic lexical analysis.
	/// </summary>
	public class Lexer
	{
		private readonly Dictionary<string, PASymbol> _symbolMappings;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.Lexer"/> class.
		/// </summary>
		/// <param name="symbolMappings">Mapping from textual representation of symbols to appropriate  objects.</param>
		public Lexer(IDictionary<string, PASymbol> symbolMappings)
		{
			if (symbolMappings == null)
				throw new ArgumentNullException("symbolMappings");

			_symbolMappings = new Dictionary<string, PASymbol>();

			// To match the longest prefixes first
			foreach (var prefix in symbolMappings.Keys.OrderByDescending(p => p.Length))
				_symbolMappings[prefix] = symbolMappings[prefix];
		}

		/// <summary>
		/// Converts the text to a list of symbols using the lexer's symbol mapping.
		/// </summary>
		/// <param name="text">Symbols.</param>
		public IList<PASymbol> Tokenize(string text)
		{
			var prefixes = _symbolMappings.Keys;
			var symbols = new List<PASymbol>();
			var linePosition = 1;

			while (text.Length > 0)
			{
				var matchingPrefix = prefixes.FirstOrDefault(text.StartsWith);

				if (matchingPrefix == null)
					throw new LexerException("Lexical error. Line position {0}.", linePosition);

				text = text.Remove(0, matchingPrefix.Length);
				symbols.Add(_symbolMappings[matchingPrefix]);
				linePosition += matchingPrefix.Length;
			}

			return symbols;
		}
	}
}