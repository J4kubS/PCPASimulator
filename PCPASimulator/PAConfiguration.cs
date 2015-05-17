using System;
using System.Collections.Generic;
using System.Linq;

namespace PCPASimulator
{
	/// <summary>
	/// Represents a configuration of a pushdown automaton.
	/// </summary>
	public class PAConfiguration : IEquatable<PAConfiguration>, ICloneable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PAConfiguration"/> class.
		/// </summary>
		/// <param name="state">The current state.</param>
		/// <param name="input">The unread part of the input.</param>
		/// <param name="stack">The current stack contents.</param>
		/// <param name="reachedWith">The transition by which this configuration was reached.</param>
		public PAConfiguration(PAState state,
		                       IList<PASymbol> input,
		                       IList<PASymbol> stack,
		                       PATransition reachedWith = null)
		{
			if (state == null)
				throw new ArgumentNullException("state");

			if (input == null)
				throw new ArgumentNullException("input");

			if (stack == null)
				throw new ArgumentNullException("stack");

			Input = new List<PASymbol>(input);
			Stack = new List<PASymbol>(stack);
			ReachedWith = reachedWith;
			State = state;
		}

		/// <summary>
		/// Gets the current state.
		/// </summary>
		public PAState State { get; private set; }

		/// <summary>
		/// Gets the first symbol of the input.
		/// </summary>
		public PASymbol InputSymbol
		{
			get { return Input.DefaultIfEmpty(PASymbol.Epsilon).First(); } 
		}

		/// <summary>
		/// Gets the topmost symbol of the stack.
		/// </summary>
		public PASymbol TopmostSymbol
		{
			get { return Stack.DefaultIfEmpty(PASymbol.Epsilon).First(); }
		}

		/// <summary>
		/// Gets the unread input.
		/// </summary>
		public IList<PASymbol> Input { get; private set; }

		/// <summary>
		/// Gets the current stack contents.
		/// </summary>
		/// <value>The stack.</value>
		public IList<PASymbol> Stack { get; private set; }

		/// <summary>
		/// Gets the transition that was used to reach this configuration.
		/// </summary>
		/// <value>The reached with.</value>
		public PATransition ReachedWith { get; private set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PAConfiguration"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="PCPASimulator.PAConfiguration"/>.</returns>
		public override string ToString()
		{
			var inputString = Input.Any() ? string.Join(null, Input) : PASymbol.Epsilon.ToString();
			var stackString = Stack.Any() ? string.Join(null, Stack) : PASymbol.Epsilon.ToString();

			return string.Format("({0}, {1}, {2})", State, inputString, stackString);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="PCPASimulator.PAConfiguration"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="PCPASimulator.PAConfiguration"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="PCPASimulator.PAConfiguration"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as PAConfiguration);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="PCPASimulator.PAConfiguration"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			var inputHash = 19;
			var stackHash = 19;

			foreach (var symbol in Input)
				inputHash = inputHash * 31 + symbol.GetHashCode();

			foreach (var symbol in Stack)
				stackHash = stackHash * 31 + symbol.GetHashCode();

			return State.GetHashCode() ^ inputHash ^ stackHash;
		}

		#region IEquatable

		/// <summary>
		/// Determines whether the specified <see cref="PCPASimulator.PAConfiguration"/> is equal to the current <see cref="PCPASimulator.PAConfiguration"/>.
		/// </summary>
		/// <param name="other">The <see cref="PCPASimulator.PAConfiguration"/> to compare with the current <see cref="PCPASimulator.PAConfiguration"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="PCPASimulator.PAConfiguration"/> is equal to the current
		/// <see cref="PCPASimulator.PAConfiguration"/>; otherwise, <c>false</c>.</returns>
		public bool Equals(PAConfiguration other)
		{
			if (Object.ReferenceEquals(other, null))
				return false;

			if (Object.ReferenceEquals(other, this))
				return true;

			if (GetType() != other.GetType())
				return false;
				
			var statesAreEqual = State.Equals(other.State);
			var inputsAreEqual = Input.SequenceEqual(other.Input);
			var stacksAreEqual = Stack.SequenceEqual(other.Stack);

			return statesAreEqual && inputsAreEqual && stacksAreEqual;
		}

		#endregion

		#region ICloneable

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public object Clone()
		{
			var newInput = new List<PASymbol>(Input);
			var newStack = new List<PASymbol>(Stack);

			return new PAConfiguration(State, newInput, newStack, ReachedWith);
		}

		#endregion
	}
}