using System;

namespace Rander
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static Game gm;

        [STAThread]
        static void Main()
        {
            using (var game = new Game())
            {
                gm = game;
                game.Run();
            }
        }
    }
}
