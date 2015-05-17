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