using EnsoulSharp.SDK;
using EnsoulSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrickSTRR.AIO
{

    class Program
    {

        static void Main(string[] args)
        {
            GameEvent.OnGameLoad += GameEvent_OnGameLoad;
           //GameEvent.OnLoad += GameEvent_OnLoad;

        }

        private static void GameEvent_OnGameLoad()
        {
            Console.WriteLine("Done");
            Init.Initialize();
        }

    }
}