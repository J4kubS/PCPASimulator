namespace PCPASimulator
{
	/// <summary>
	/// Program.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		public static void Main(string[] args)
		{
			if (args.Length > 0)
				UI.Instance.Help();
			else
				UI.Instance.Show();
		}
	}
}