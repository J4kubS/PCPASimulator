using System.Collections.Generic;
using System.Linq;

namespace PCPASimulator
{
	/// <summary>
	/// Represents a configuration of a system of parallel communicating pushdown automata.
	/// </summary>
	public class PCPAConfiguration : List<PAConfiguration>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAConfiguration"/> class.
		/// </summary>
		public PCPAConfiguration()
			: this(Enumerable.Empty<PAConfiguration>())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAConfiguration"/> class.
		/// </summary>
		/// <param name="configuration">A configuration from which to copy configurations of components.</param>
		public PCPAConfiguration(IEnumerable<PAConfiguration> configuration)
			: this(configuration, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAConfiguration"/> class.
		/// </summary>
		/// <param name="configuration">A configuration from which to copy configurations of components.</param>
		/// <param name="previous">A system's previous configuration.</param>
		public PCPAConfiguration(IEnumerable<PAConfiguration> configuration,
		                         PCPAConfiguration previous)
			: this(configuration, previous, ConfigurationType.General)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAConfiguration"/> class.
		/// </summary>
		/// <param name="configuration">A configuration from which to copy configurations of components.</param>
		/// <param name="previous">A system's previous configuration.</param>
		/// <param name="type">The type of the configuration.</param>
		public PCPAConfiguration(IEnumerable<PAConfiguration> configuration,
		                         PCPAConfiguration previous,
		                         ConfigurationType type)
			: base(configuration)
		{
			Previous = previous;
			Type = type;
		}

		/// <summary>
		/// Type of the configuration.
		/// </summary>
		public enum ConfigurationType
		{
			/// <summary>
			/// General configuration.
			/// </summary>
			General,
			/// <summary>
			/// Configuration in which the input was accepted.
			/// </summary>
			InputAccepted,
			/// <summary>
			/// Configuration in which the input was rejected.
			/// </summary>
			InputRejected
		}

		/// <summary>
		/// Gets the system's previous configuration.
		/// </summary>
		public PCPAConfiguration Previous { get; private set; }

		/// <summary>
		/// Gets or sets the type of the configuration.
		/// </summary>
		/// <value>The type.</value>
		public ConfigurationType Type { get; set; }
	}
}