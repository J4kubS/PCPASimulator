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