//
//  Author:
//    Jakub Šoustar xsoust02@stud.fit.vutbr.cz
//
//  Copyright (c) 2015, Jakub Šoustar
//
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the distribution.
//     * Neither the name of the Jakub Šoustar nor the names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

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