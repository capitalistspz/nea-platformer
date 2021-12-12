using System;

namespace server
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ServerGame())
                game.Run();
        }
    }
}