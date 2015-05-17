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
	/// Represents a state of a pushdown automaton.
	/// </summary>
	public class PAState : IEquatable<PAState>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PAState"/> class.
		/// </summary>
		/// <param name="name">The name of the state.</param>
		/// <param name="isFinal">If set to <c>true</c>, the state is a final state.</param>
		public PAState(string name, bool isFinal = false)
		{
			IsFinal = isFinal;
			Name = name;
		}

		/// <summary>
		/// Gets the name of the state.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance represents a final state.
		/// </summary>
		public bool IsFinal { get; private set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PAState"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PAState"/>.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PAState"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="PCPASimulator.PAState"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PAState"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as PAState);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="PCPASimulator.PAState"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ IsFinal.GetHashCode();
		}

		#region IEquatable

		/// <summary>
		/// Determines whether the specified <see cref="PCPASimulator.PAState"/> is equal to the current <see cref="PCPASimulator.PAState"/>.
		/// </summary>
		/// <param name="other">The <see cref="PCPASimulator.PAState"/> to compare with the current <see cref="PCPASimulator.PAState"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="PCPASimulator.PAState"/> is equal to the current <see cref="PCPASimulator.PAState"/>;
		/// otherwise, <c>false</c>.</returns>
		public bool Equals(PAState other)
		{
			if (Object.ReferenceEquals(other, null))
				return false;

			if (Object.ReferenceEquals(other, this))
				return true;

			if (GetType() != other.GetType())
				return false;

			return (Name == other.Name) && (IsFinal == other.IsFinal);
		}

		#endregion
	}
}