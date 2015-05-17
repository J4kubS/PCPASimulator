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
	/// Represents a pushdown automaton that is a component of a
	/// systems of parallel communicating pushdown automata.
	/// </summary>
	public class PushdownAutomaton
	{
		private readonly List<PATransition> _transitions;
		private readonly AcceptingMode _acceptingMode;
		private readonly PASymbol _initialStackSymbol;
		private readonly PAState _initialState;
		private readonly bool _isReturning;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PushdownAutomaton"/> class.
		/// </summary>
		/// <param name="transitions">The automaton's transitions.</param>
		/// <param name="initialState">The automaton's initial state.</param>
		/// <param name="initialStackSymbol">The initial symbol of automaton's stack.</param>
		/// <param name="acceptingMode">The accepting mode of the automaton.</param>
		/// <param name="isReturning">If set to <c>true</c>, the automaton is returning.</param>
		public PushdownAutomaton(IEnumerable<PATransition> transitions,
		                         PAState initialState,
		                         PASymbol initialStackSymbol,
		                         AcceptingMode acceptingMode,
		                         bool isReturning)
		{
			if (transitions == null)
				throw new ArgumentNullException("transitions");

			if (initialState == null)
				throw new ArgumentNullException("initialState");

			if (initialStackSymbol == null)
				throw new ArgumentNullException("initialStackSymbol");

			_transitions = new List<PATransition>(transitions);
			_initialStackSymbol = initialStackSymbol;
			_acceptingMode = acceptingMode;
			_initialState = initialState;
			_isReturning = isReturning;
		}

		/// <summary>
		/// Accepting mode.
		/// </summary>
		public enum AcceptingMode
		{
			/// <summary>
			/// Acceptance by final state.
			/// </summary>
			FinalState,
			/// <summary>
			/// Acceptance by empty stack.
			/// </summary>
			EmptyStack,
			/// <summary>
			/// Acceptance by final state and empty stack.
			/// </summary>
			Both
		}

		/// <summary>
		/// Gets the initial configuration for the automaton using the specified list of input symbols.
		/// </summary>
		/// <returns>The initial configuration.</returns>
		/// <param name="input">A list of input symbols.</param>
		public PAConfiguration GetInitialConfiguration(IList<PASymbol> input)
		{
			if (input == null)
				throw new ArgumentNullException("input");

			return new PAConfiguration(_initialState,
				new List<PASymbol>(input),
				new List<PASymbol>{ _initialStackSymbol });
		}

		/// <summary>
		/// Performs a single step from the specified <paramref name="configuration"/>.
		/// Returns a <see cref="IEnumerable&lt;T&gt;"/> of all resulting configurations.
		/// </summary>
		/// <param name="configuration">A <see cref="IEnumerable&lt;T&gt;"/> of configurations.</param>
		public IEnumerable<PAConfiguration> Step(PAConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			var configurations = new List<PAConfiguration>();
			var transitions = (from t in _transitions
			                   where t.OldState.Equals(configuration.State)
			                       && t.TopmostSymbol.Equals(configuration.TopmostSymbol)
			                       && (t.InputSymbol.Equals(PASymbol.Epsilon) || t.InputSymbol.Equals(configuration.InputSymbol))
			                   select t).ToList();

			// Implicit transition
			if (transitions.Count == 0 && IsAccepting(configuration))
			{
				var implicitTransition = new PATransition(configuration.State,
					                         configuration.InputSymbol,
					                         configuration.TopmostSymbol,
					                         configuration.State,
					                         new []{ configuration.TopmostSymbol });

				_transitions.Add(implicitTransition);
				transitions.Add(implicitTransition);
			}

			foreach (var transition in transitions)
			{
				var stack = configuration.Stack.Skip(1).ToList();
				var input = new List<PASymbol>();

				if (!transition.InputSymbol.Equals(PASymbol.Epsilon))
					input.AddRange(configuration.Input.Skip(1));
				else
					input.AddRange(configuration.Input);

				stack.InsertRange(0, transition.TopmostSymbolReplacement);
				configurations.Add(new PAConfiguration(transition.NewState, input, stack, transition));
			}

			return configurations;
		}

		/// <summary>
		/// Determines whether the specified configuration is a accepting one for this instance.
		/// </summary>
		/// <returns><c>true</c> if the specified configuration is a accepting one; otherwise, <c>false</c>.</returns>
		/// <param name="configuration">Configuration.</param>
		public bool IsAccepting(PAConfiguration configuration)
		{
			if (configuration.Input.Count > 0)
				return false;

			var isInFinalState = configuration.State.IsFinal;
			var isStackEmpty = configuration.Stack.Count == 0;

			if (_acceptingMode == AcceptingMode.FinalState)
				return isInFinalState;
			else if (_acceptingMode == AcceptingMode.EmptyStack)
					return isStackEmpty;
				else
					return isInFinalState && isStackEmpty;
		}

		/// <summary>
		/// Sends the stack.
		/// </summary>
		/// <remarks>
		/// If the instance is returning, the stack is also reset to <see cref="_initialStackSymbol"/>.
		/// </remarks>
		/// <returns>The stack.</returns>
		/// <param name="configuration">Current configuration.</param>
		public IList<PASymbol> SendStack(PAConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			var stack = new List<PASymbol>(configuration.Stack);

			if (_isReturning)
			{
				configuration.Stack.Clear();
				configuration.Stack.Add(_initialStackSymbol);
			}

			return stack;
		}

		/// <summary>
		/// Receives the stack.
		/// </summary>
		/// <param name="configuration">Current configuration.</param>
		/// <param name="stack">Receiving stack.</param>
		public void ReceiveStack(PAConfiguration configuration,
		                         IList<PASymbol> stack)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			if (stack == null)
				throw new ArgumentNullException("stack");

			if (configuration.Stack.Count > 0)
				configuration.Stack.RemoveAt(0);

			foreach (var symbol in stack.Reverse())
				configuration.Stack.Insert(0, symbol);
		}
	}
}