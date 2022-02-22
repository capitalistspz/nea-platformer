﻿using System;
using Serilog;

namespace client2
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
                .File("logs/client.log", rollingInterval: RollingInterval.Hour).CreateLogger();
            using var game = new ClientGame();
            game.Run();
        }
    }
}