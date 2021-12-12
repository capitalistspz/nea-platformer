using System;

namespace server.helpers
{
    public static class CmdMsg
    {
        public static string PrintAndInput(string printMessage)
        {
            Console.WriteLine(printMessage);
            return Console.ReadLine();
        }
    }
}