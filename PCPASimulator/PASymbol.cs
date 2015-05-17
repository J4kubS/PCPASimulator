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

namespace PCPASimulator
{
	/// <summary>
	/// Represents a input or stack symbol of a pushdown automaton.
	/// </summary>
	public class PASymbol : IEquatable<PASymbol>
	{
		static PASymbol()
		{
			Epsilon = new PASymbol("ε");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PASymbol"/> class.
		/// </summary>
		/// <param name="name">The name of the state.</param>
		/// <param name="isQuery">If set to <c>true</c>, the symbol is a query symbol.</param>
		public PASymbol(string name, bool isQuery = false)
		{
			IsQuery = isQuery;
			Name = name;
		}

		/// <summary>
		/// Gets the ε symbol.
		/// </summary>
		public static PASymbol Epsilon { get; private set; }

		/// <summary>
		/// Gets the name of the symbol.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance represents a query symbol.
		/// </summary>
		public bool IsQuery { get; private set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PASymbol"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PASymbol"/>.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PASymbol"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="PCPASimulator.PASymbol"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PASymbol"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as PASymbol);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="PCPASimulator.PASymbol"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ IsQuery.GetHashCode();
		}

		#region IEquatable

		/// <summary>
		/// Determines whether the specified <see cref="PCPASimulator.PASymbol"/> is equal to the current <see cref="PCPASimulator.PASymbol"/>.
		/// </summary>
		/// <param name="other">The <see cref="PCPASimulator.PASymbol"/> to compare with the current <see cref="PCPASimulator.PASymbol"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="PCPASimulator.PASymbol"/> is equal to the current <see cref="PCPASimulator.PASymbol"/>;
		/// otherwise, <c>false</c>.</returns>
		public bool Equals(PASymbol other)
		{
			if (Object.ReferenceEquals(other, null))
				return false;

			if (Object.ReferenceEquals(other, this))
				return true;

			if (GetType() != other.GetType())
				return false;

			return (Name == other.Name) && (IsQuery == other.IsQuery);
		}

		#endregion
	}
}