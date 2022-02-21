using System;

namespace client2
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new ClientGame();
            game.Run();
        }
    }
}