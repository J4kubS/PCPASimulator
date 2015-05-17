using System;
using System.Collections.Generic;
using System.Linq;

namespace PCPASimulator
{
	/// <summary>
	/// Represents a transition of a pushdown automaton
	/// </summary>
	public class PATransition : IEquatable<PATransition>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PATransition"/> class.
		/// </summary>
		/// <param name="oldState">The old state.</param>
		/// <param name="inputSymbol">The input symbol.</param>
		/// <param name="topmostSymbol">The topmost symbol.</param>
		/// <param name="newState">The new state.</param>
		/// <param name="topmostSymbolReplacement">The topmost symbol replacement.</param>
		public PATransition(PAState oldState,
		                    PASymbol inputSymbol,
		                    PASymbol topmostSymbol,
		                    PAState newState,
		                    IEnumerable<PASymbol> topmostSymbolReplacement)
		{
			if (oldState == null)
				throw new ArgumentNullException("oldState");

			if (inputSymbol == null)
				throw new ArgumentNullException("inputSymbol");

			if (topmostSymbol == null)
				throw new ArgumentNullException("topmostSymbol");

			if (newState == null)
				throw new ArgumentNullException("newState");

			if (topmostSymbolReplacement == null)
				throw new ArgumentNullException("topmostSymbolReplacement");

			TopmostSymbolReplacement = topmostSymbolReplacement.ToList();
			TopmostSymbol = topmostSymbol;
			InputSymbol = inputSymbol;
			OldState = oldState;
			NewState = newState;
		}

		/// <summary>
		/// Gets the old state.
		/// </summary>
		public PAState OldState { get; private set; }

		/// <summary>
		/// Gets the input symbol.
		/// </summary>
		public PASymbol InputSymbol { get; private set; }

		/// <summary>
		/// Gets the topmost symbol.
		/// </summary>
		public PASymbol TopmostSymbol { get; private set; }

		/// <summary>
		/// Gets the new state.
		/// </summary>
		public PAState NewState { get; private set; }

		/// <summary>
		/// Gets the topmost symbol replacement.
		/// </summary>
		public IEnumerable<PASymbol> TopmostSymbolReplacement { get; private set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PATransition"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PATransition"/>.</returns>
		public override string ToString()
		{
			var replacementString = TopmostSymbolReplacement.Any() ? string.Join(null, TopmostSymbolReplacement) : PASymbol.Epsilon.ToString();

			return string.Format("({0}, {1}) ∈ δ({2}, {3}, {4})",
				NewState,
				replacementString,
				OldState,
				InputSymbol,
				TopmostSymbol);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PATransition"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="PCPASimulator.PATransition"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PATransition"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as PATransition);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="PCPASimulator.PATransition"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			var replacementHash = 19;

			foreach (var replacement in TopmostSymbolReplacement)
				replacementHash *= replacement.GetHashCode();

			return OldState.GetHashCode() ^ InputSymbol.GetHashCode() ^ TopmostSymbol.GetHashCode() ^ NewState.GetHashCode() ^ replacementHash;
		}

		#region IEquatable

		/// <summary>
		/// Determines whether the specified <see cref="PCPASimulator.PATransition"/> is equal to the current <see cref="PCPASimulator.PATransition"/>.
		/// </summary>
		/// <param name="other">The <see cref="PCPASimulator.PATransition"/> to compare with the current <see cref="PCPASimulator.PATransition"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="PCPASimulator.PATransition"/> is equal to the current
		/// <see cref="PCPASimulator.PATransition"/>; otherwise, <c>false</c>.</returns>
		public bool Equals(PATransition other)
		{
			if (Object.ReferenceEquals(other, null))
				return false;

			if (Object.ReferenceEquals(other, this))
				return true;

			if (GetType() != other.GetType())
				return false;

			var replacementsAreEqual = TopmostSymbolReplacement.SequenceEqual(other.TopmostSymbolReplacement);
			var topmostSymbolsAreEqual = TopmostSymbol.Equals(other.TopmostSymbol);
			var inputSymbolsAreEqual = InputSymbol.Equals(other.InputSymbol);
			var newStatesAreEqual = NewState.Equals(other.NewState);
			var oldStatesAreEqual = OldState.Equals(other.OldState);

			return replacementsAreEqual && topmostSymbolsAreEqual && inputSymbolsAreEqual && newStatesAreEqual && oldStatesAreEqual;
		}

		#endregion
	}
}