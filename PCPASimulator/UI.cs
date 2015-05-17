using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace PCPASimulator
{
	/// <summary>
	/// Represents a program's user interface.
	/// </summary>
	public sealed class UI
	{
		private static UI _instance;

		private readonly List<Tuple<MenuOptions, string>> _systemMenuOptions;
		private readonly List<Tuple<MenuOptions, string>> _mainMenuOptions;
		private PCPASystem _pcpaSystem;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.UI"/> class.
		/// </summary>
		private UI()
		{
			_systemMenuOptions = new List<Tuple<MenuOptions, string>> {
				new Tuple<MenuOptions, string>(MenuOptions.ParseInput, Resources.OptionParseInput),
				new Tuple<MenuOptions, string>(MenuOptions.Return, Resources.OptionReturn)
			};
			_mainMenuOptions = new List<Tuple<MenuOptions, string>> {
				new Tuple<MenuOptions, string>(MenuOptions.NewSystem, Resources.OptionNewSystem),
				new Tuple<MenuOptions, string>(MenuOptions.Quit, Resources.OptionQuit)
			};
		}

		/// <summary>
		/// Menu options.
		/// </summary>
		private enum MenuOptions
		{
			NewSystem,
			ParseInput,
			Return,
			Quit
		}

		/// <summary>
		/// Gets the instance of UI.
		/// </summary>
		public static UI Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UI();

				return _instance;
			}
		}

		/// <summary>
		/// Displays the help.
		/// </summary>
		public void Help()
		{
			Console.WriteLine(Resources.Help);
		}

		/// <summary>
		/// Displays the user interface.
		/// </summary>
		public void Show()
		{
			MainMenu();
		}

		/// <summary>
		/// Writes the coloured text representation of the specified object
		/// to the standard output stream using the specified format information.
		/// </summary>
		/// <param name="arg">An object to write.</param>
		private static void WriteColoured(object arg)
		{
			WriteColoured("{0}", arg);
		}

		/// <summary>
		/// Writes the coloured text representation of the specified array of objects
		/// to the standard output stream using the specified format information.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
		private static void WriteColoured(string format, params object[] args)
		{
			Console.ForegroundColor = Resources.Color;
			Console.Write(format, args);
			Console.ResetColor();
		}

		/// <summary>
		/// Writes the coloured text representation of the specified object,
		/// followed by the current line terminator, to the standard output stream
		/// using the specified format information.
		/// </summary>
		/// <param name="arg">An object to write.</param>
		private static void WriteColouredLine(object arg)
		{
			WriteColoured("{0}" + Environment.NewLine, arg);
		}

		/// <summary>
		/// Writes the coloured text representation of the specified array of objects,
		/// followed by the current line terminator, to the standard output stream
		/// using the specified format information.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
		private static void WriteColouredLine(string format, params object[] args)
		{
			WriteColoured(format + Environment.NewLine, args);
		}

		/// <summary>
		/// Displays the error message.
		/// </summary>
		private static void Error(string error)
		{
			Console.Error.WriteLine(error);
			Console.ReadKey();
		}

		/// <summary>
		/// Clears the console and displays the program's title.
		/// </summary>
		private static void Title()
		{
			var bottomLine = new string(Resources.TitleBoxB, Resources.Title.Length + 2);
			var topLine = new string(Resources.TitleBoxT, Resources.Title.Length + 2);

			Console.Clear();
			WriteColouredLine("{0}{1}{2}", Resources.TitleBoxTL, topLine, Resources.TitleBoxTR);
			WriteColouredLine("{0} {1} {2}", Resources.TitleBoxL, Resources.Title, Resources.TitleBoxR);
			WriteColouredLine("{0}{1}{2}", Resources.TitleBoxBL, bottomLine, Resources.TitleBoxBR);
			Console.WriteLine();
		}

		/// <summary>
		/// Displays the <see cref="Title"/> followed by a menu specified by the <paramref name="options"/>.
		/// </summary>
		private static MenuOptions Menu(List<Tuple<MenuOptions, string>> options)
		{
			var padding = Resources.MenuSelector.Length + 1;
			var index = 0;
			Console.CursorVisible = false;

			while (true)
			{
				Title();
				Console.WriteLine(Resources.MenuHeadline);
				Console.WriteLine();

				for (var i = 0; i < options.Count; i++)
				{
					if (i == index)
						WriteColoured(Resources.MenuSelector);

					Console.CursorLeft = padding;
					Console.WriteLine(options[i].Item2);
				}

				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.UpArrow:
					case ConsoleKey.LeftArrow:
						if (index - 1 < 0)
							index = options.Count - 1;
						else
							index--;
						break;
					case ConsoleKey.DownArrow:
					case ConsoleKey.RightArrow:
						if (index >= options.Count - 1)
							index = 0;
						else
							index++;
						break;
					case ConsoleKey.Enter:
						Console.CursorVisible = true;
						return options[index].Item1;
				}
			}
		}

		/// <summary>
		/// Displays the <see cref="Title"/> followed by a prompt for the user's input.
		/// </summary>
		private static string Prompt(string prompt)
		{
			Title();
			Console.WriteLine(Resources.PromptHeadline);
			Console.WriteLine();
			Console.Write(prompt);

			return Console.ReadLine();
		}

		/// <summary>
		/// Displays the program's main <see cref="Menu"/>.
		/// </summary>
		private void MainMenu()
		{
			while (Menu(_mainMenuOptions) == MenuOptions.NewSystem)
			{
				while (!TryLoadSystem(Prompt(Resources.MenuPromptConfigurationFile)))
				{
				}

				SystemMenu();
			}
		}

		/// <summary>
		/// Displays the system's <see cref="Menu"/>.
		/// </summary>
		private void SystemMenu()
		{
			if (_pcpaSystem == null)
				throw new InvalidOperationException("No system loaded");

			while (Menu(_systemMenuOptions) == MenuOptions.ParseInput)
			{
				try
				{
					var result = _pcpaSystem.Parse(Prompt(Resources.MenuPromptInput));

					if (result.Type == PCPAConfiguration.ConfigurationType.InputAccepted)
						Console.WriteLine(Resources.InputAccepted);
					else
						Console.WriteLine(Resources.InputRejected);

					Console.WriteLine();
					Result(result);
					Console.ReadKey();
				}
				catch (Exception e)
				{
					if (e is LexerException || e is ParserException)
					{
						Error(e.Message);
						continue;
					}

					throw;
				}
			}
		}

		/// <summary>
		/// Displays the result of the system's work.
		/// </summary>
		private static void Result(PCPAConfiguration result)
		{
			if (result == null)
				throw new ArgumentNullException("result");

			var configurations = new List<PCPAConfiguration>();
			var current = result;

			while (current != null)
			{
				configurations.Add(current);
				current = current.Previous;
			}

			var longestConfiguration = configurations.Max(s => s.Max(c => c.ToString().Length));
			var padding = configurations.Count.ToString().Length;
			configurations.Reverse();

			for (var i = 0; i < result.Count; i++)
			{
				WriteColoured(Resources.ResultLinePrefixFormat, "".PadLeft(padding));
				WriteColoured(Resources.ResultAutomatonNFormat, i);

				for (int j = 0; j < configurations.Count; j++)
				{
					var configuration = configurations[j][i];

					WriteColoured(Resources.ResultLinePrefixFormat, (j + 1).ToString().PadLeft(padding));
					Console.Write(configuration.ToString().PadRight(longestConfiguration));

					if (configuration.ReachedWith != null)
						Console.WriteLine(Resources.ResultReachedWithFormat, configuration.ReachedWith);
					else if (configurations[j].Previous != null)
					{
						if (configurations[j].Previous[i].TopmostSymbol.IsQuery)
							Console.WriteLine(Resources.ResultAfterRequestedCommunication);
						else
							Console.WriteLine(Resources.ResultAfterCommunication);
					}
					else
						Console.WriteLine();
				}

				Console.WriteLine();
			}
		}

		/// <summary>
		/// Tries to load a new system.
		/// </summary>
		/// <remarks>
		/// If unsuccessful, the <see cref="_pcpaSystem"/> is set to <c>null</c>.
		/// </remarks>
		/// <returns><c>true</c>, if the system was successfully loaded, <c>false</c> otherwise.</returns>
		/// <param name="path">Path to the system's configuration file.</param>
		private bool TryLoadSystem(string path)
		{
			_pcpaSystem = null;

			if (path == null)
				return false;
			
			if (!File.Exists(path))
			{
				Error(Resources.FileNotFound);
				return false;
			}

			try
			{
				_pcpaSystem = PCPASystem.FromXML(path);
			}
			catch (Exception e)
			{
				if (e is XmlSchemaValidationException || e is PCPAValidationException)
				{
					Error(e.Message);
					return false;
				}

				throw;
			}

			return true;
		}
	}
}