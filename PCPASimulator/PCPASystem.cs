using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace PCPASimulator
{
	/// <summary>
	/// Represents a parallel communicating pushdown automata system.
	/// </summary>
	public class PCPASystem
	{
		private readonly Dictionary<PASymbol, int> _querySymbolsMapping;
		private readonly List<PushdownAutomaton> _automata;
		private readonly Lexer _lexer;
		private PCPAConfiguration _configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPASystem"/> class.
		/// </summary>
		/// <param name="automata">A list of system's components.</param>
		/// <param name="querySymbolsMapping">A mapping of query symbols to the indexes of components.</param>
		/// <param name="lexer"><see cref="PCPASimulator.Lexer"/>.</param>
		private PCPASystem(IList<PushdownAutomaton> automata,
		                   IDictionary<PASymbol, int> querySymbolsMapping,
		                   Lexer lexer)
		{
			if (automata == null)
				throw new ArgumentNullException("automata");

			if (querySymbolsMapping == null)
				throw new ArgumentNullException("querySymbolsMapping");

			if (lexer == null)
				throw new ArgumentNullException("lexer");
			
			_querySymbolsMapping = new Dictionary<PASymbol, int>(querySymbolsMapping);
			_configuration = new PCPAConfiguration();
			_automata = automata.ToList();
			_lexer = lexer;
		}

		/// <summary>
		/// Creates a instance of <see cref="PCPASimulator.PCPASystem"/> from a XML configuration file
		/// specified by the <paramref name="path"/>.
		/// </summary>
		/// <returns>The <see cref="PCPASimulator.PCPASystem"/> instance.</returns>
		/// <param name="path">Path to the configuration file.</param>
		public static PCPASystem FromXML(string path)
		{
			var querySymbolsMapping = new Dictionary<PASymbol, int>();
			var pushdownSymbols = new Dictionary<string, PASymbol>();
			var inputSymbols = new Dictionary<string, PASymbol>();
			var automata = new List<PushdownAutomaton>();
			var root = LoadXML(path).Root;

			var isReturning = Convert.ToBoolean(root.Attribute("IsReturning").Value);

			foreach (var symbol in root.Element("InputSymbols").Elements("Symbol"))
				inputSymbols[symbol.Value] = new PASymbol(symbol.Value);

			foreach (var symbol in root.Element("PushdownSymbols").Elements("Symbol"))
				pushdownSymbols[symbol.Value] = new PASymbol(symbol.Value);

			foreach (var symbol in root.Element("QuerySymbols").Elements("Symbol"))
			{
				var automaton = Convert.ToInt32(symbol.Attribute("Automaton").Value) - 1;
				var querySymbol = new PASymbol(symbol.Value, true);

				if (automaton + 1 > root.Elements("PushdownAutomaton").Count())
					throw new PCPAValidationException(
						string.Format("Query symbol is referencing a non-existing automaton. Line {0}, Position {1}.",
							((IXmlLineInfo)symbol).LineNumber,
							((IXmlLineInfo)symbol).LinePosition));

				querySymbolsMapping.Add(querySymbol, automaton);
				pushdownSymbols[symbol.Value] = querySymbol;
			}

			foreach (var automaton in root.Elements("PushdownAutomaton"))
			{
				var states = new Dictionary<string, PAState>();
				var transitions = new HashSet<PATransition>();

				var initialStackSymbolValue = automaton.Attribute("InitialStackSymbol").Value;
				var initialStateValue = automaton.Attribute("InitialState").Value;
				var acceptingMode = (PushdownAutomaton.AcceptingMode)Enum.Parse(
					                    typeof(PushdownAutomaton.AcceptingMode),
					                    automaton.Attribute("AcceptingMode").Value);

				foreach (var state in automaton.Element("States").Elements("State"))
					states[state.Value] = new PAState(state.Value);

				foreach (var state in automaton.Element("FinalStates").Elements("State"))
					states[state.Value] = new PAState(state.Value, true);

				if (!pushdownSymbols.ContainsKey(initialStackSymbolValue))
					throw new PCPAValidationException(string.Format("Initial stack symbol is undefined. Line {0}, Position {1}.",
						((IXmlLineInfo)automaton).LineNumber,
						((IXmlLineInfo)automaton).LinePosition));

				if (!states.ContainsKey(initialStateValue))
					throw new PCPAValidationException(string.Format("Initial state is undefined. Line {0}, Position {1}.",
						((IXmlLineInfo)automaton).LineNumber,
						((IXmlLineInfo)automaton).LinePosition));

				foreach (var transition in automaton.Element("Transitions").Elements("Transition"))
				{
					var topmostSymbolValue = transition.Attribute("TopmostSymbol").Value;
					var inputSymbolValue = (string)transition.Attribute("InputSymbol");
					var oldStateValue = transition.Attribute("OldState").Value;
					var newStateValue = transition.Attribute("NewState").Value;
					var topmostSymbolReplacement = new List<PASymbol>();

					if (!pushdownSymbols.ContainsKey(topmostSymbolValue))
						throw new PCPAValidationException(string.Format("Topmost symbol is undefined. Line {0}, Position {1}.",
							((IXmlLineInfo)transition).LineNumber,
							((IXmlLineInfo)transition).LinePosition));

					if (inputSymbolValue != null && !inputSymbols.ContainsKey(inputSymbolValue))
						throw new PCPAValidationException(string.Format("Input symbol is undefined. Line {0}, Position {1}.",
							((IXmlLineInfo)transition).LineNumber,
							((IXmlLineInfo)transition).LinePosition));

					if (!states.ContainsKey(oldStateValue))
						throw new PCPAValidationException(string.Format("Old state is undefined. Line {0}, Position {1}.",
							((IXmlLineInfo)transition).LineNumber,
							((IXmlLineInfo)transition).LinePosition));

					if (!states.ContainsKey(newStateValue))
						throw new PCPAValidationException(string.Format("New state is undefined. Line {0}, Position {1}.",
							((IXmlLineInfo)transition).LineNumber,
							((IXmlLineInfo)transition).LinePosition));

					foreach (var replacement in transition.Elements("Symbol"))
					{
						if (!pushdownSymbols.ContainsKey(replacement.Value))
							throw new PCPAValidationException(string.Format("Replacement symbol is undefined. Line {0}, Position {1}.",
								((IXmlLineInfo)replacement).LineNumber,
								((IXmlLineInfo)replacement).LinePosition));

						topmostSymbolReplacement.Add(pushdownSymbols[replacement.Value]);
					}

					var inputSymbol = inputSymbolValue == null ? PASymbol.Epsilon : inputSymbols[inputSymbolValue];
					var topmostSymbol = pushdownSymbols[topmostSymbolValue];
					var oldState = states[oldStateValue];
					var newState = states[newStateValue];

					transitions.Add(new PATransition(oldState, inputSymbol, topmostSymbol, newState, topmostSymbolReplacement));
				}

				var initialStackSymbol = pushdownSymbols[initialStackSymbolValue];
				var initialState = states[initialStateValue];

				automata.Add(new PushdownAutomaton(transitions, initialState, initialStackSymbol, acceptingMode, isReturning));
			}

			return new PCPASystem(automata, querySymbolsMapping, new Lexer(inputSymbols));
		}

		/// <summary>
		/// Tries to parse the <paramref name="text"/>.
		/// </summary>
		/// <remarks>
		/// Returns a system's <see cref="PCPASimulator.PCPAConfiguration"/> in which the system either accepted or rejected the <paramref name="text"/>.
		/// This configuration can be used to reconstruct system's actions.
		/// If the <paramref name="text"/> was rejected, the returned <see cref="PCPASimulator.PCPAConfiguration"/> represents a configuration
		/// in which was the largest part of the <paramref name="text"/> parsed.
		/// </remarks>
		/// <param name="text">Text.</param>
		public PCPAConfiguration Parse(string text)
		{
			var newConfigurations = new List<PCPAConfiguration>();
			var failedBranches = new List<PCPAConfiguration>();
			var branches = new Queue<PCPAConfiguration>();
			var input = _lexer.Tokenize(text);
			_configuration = new PCPAConfiguration();

			foreach (var automaton in _automata)
				_configuration.Add(automaton.GetInitialConfiguration(input));

			if (IsAccepting(_configuration))
				return _configuration;
			
			branches.Enqueue(_configuration);

			while (branches.Count > 0)
			{
				_configuration = branches.Dequeue();
				newConfigurations.Clear();

				if (_configuration.Any(c => c.TopmostSymbol.IsQuery))
					newConfigurations.Add(CommunicationStep());
				else
					newConfigurations.AddRange(AcceptingStep());

				foreach (var newConfiguration in newConfigurations)
				{
					if (IsAccepting(newConfiguration))
					{
						newConfiguration.Type = PCPAConfiguration.ConfigurationType.InputAccepted;
						return newConfiguration;
					}

					if (newConfiguration.Type == PCPAConfiguration.ConfigurationType.InputRejected)
						failedBranches.Add(newConfiguration);
					else
						branches.Enqueue(newConfiguration);
				}
			}

			return failedBranches.OrderBy(p => p.Sum(c => c.Input.Count)).First();
		}

		/// <summary>
		/// Performs a accepting step from the current configuration.
		/// </summary>
		/// <returns>A list of resulting configurations.</returns>
		private List<PCPAConfiguration> AcceptingStep()
		{
			// List of lists of all the possible configurations that each component can make a move to
			var newPAConfigurations = new List<List<PAConfiguration>>();
			var newConfigurations = new List<PCPAConfiguration>();

			// Collect all the possible configurations for each component
			for (var i = 0; i < _automata.Count; i++)
				newPAConfigurations.Add(_automata[i].Step(_configuration[i]).ToList());

			// System can't perform any transition
			if (newPAConfigurations.Any(c => c.Count == 0))
			{
				_configuration.Type = PCPAConfiguration.ConfigurationType.InputRejected;
				newConfigurations.Add(_configuration);
			}
			else
			{
				// No non-determinism occured
				if (newPAConfigurations.All(c => c.Count == 1))
					newConfigurations.Add(new PCPAConfiguration(
						newPAConfigurations.Select(c => c.First()),
						_configuration));
				// Non-determinism. Get new configurations (cartesian) and clone components' configurations
				else
					foreach (var product in newPAConfigurations.CartesianProduct())
						newConfigurations.Add(new PCPAConfiguration(
							product.Select(c => c.Clone() as PAConfiguration),
							_configuration));
			}

			return newConfigurations;
		}

		/// <summary>
		/// Performs a communication step from the current configuration.
		/// </summary>
		/// <returns>A resulting configuration.</returns>
		private PCPAConfiguration CommunicationStep()
		{
			// <requesting automaton, requested automaton> 
			var requests = new Dictionary<int, int>();
			// <requested automaton, requested stack>
			var cache = new Dictionary<int, IList<PASymbol>>();

			// Collect the requests
			for (int i = 0; i < _configuration.Count; i++)
			{
				var topmostSymbol = _configuration[i].TopmostSymbol;

				if (topmostSymbol.IsQuery)
					requests[i] = _querySymbolsMapping[topmostSymbol];
			}

			if (requests.Count == 0)
				throw new ParserException("No communication was requested.");

			if (!requests.Keys.Except(requests.Values).Any())
				throw new ParserException("Circular query appeared.");

			var paConfigurations = from c in _configuration
			                       select new PAConfiguration(c.State, new List<PASymbol>(c.Input), new List<PASymbol>(c.Stack));
			var newConfiguration = new PCPAConfiguration(paConfigurations, _configuration);

			// Only satisfiable requests
			foreach (var request in requests.Where(r => !requests.ContainsKey(r.Value)))
			{
				var requesting = request.Key;
				var requested = request.Value;

				// Store the requested stack in case there is another request for it
				if (!cache.ContainsKey(requested))
					cache.Add(requested, _automata[requested].SendStack(newConfiguration[requested]));

				_automata[requesting].ReceiveStack(newConfiguration[requesting], cache[requested]);
			}

			return newConfiguration;
		}

		/// <summary>
		/// Loads a XML configuration file specified by the <paramref name="path"/>.
		/// Also performs a validation againts the xml schema.
		/// </summary>
		/// <returns>A loaded <see cref="System.Xml.Linq.XDocument"/>.</returns>
		/// <param name="path">Path to the configuration file.</param>
		private static XDocument LoadXML(string path)
		{
			var schemas = new XmlSchemaSet();
			schemas.Add(null, XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream("PCPASimulator.PCPASchema.xsd")));

			var settings = new XmlReaderSettings {
				ValidationType = ValidationType.Schema,
				Schemas = schemas
			};
			settings.ValidationEventHandler += (o, e) =>
			{
				if (e.Severity == XmlSeverityType.Error)
					throw e.Exception;
			};

			using (var reader = XmlReader.Create(path, settings))
				return XDocument.Load(reader, LoadOptions.SetLineInfo);
		}

		/// <summary>
		/// Determines whether the current configuration accepts the input.
		/// </summary>
		/// <returns><c>true</c> if the current configuration is accepting the input; otherwise, <c>false</c>.</returns>
		/// <param name="systemConfiguration">Current configuration.</param>
		private bool IsAccepting(PCPAConfiguration systemConfiguration)
		{
			var isAccepting = true;

			for (var i = 0; i < systemConfiguration.Count; i++)
				isAccepting &= _automata[i].IsAccepting(systemConfiguration[i]);

			return isAccepting;
		}
	}
}