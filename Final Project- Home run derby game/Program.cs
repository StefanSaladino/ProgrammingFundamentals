using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;

namespace Final_Project__Home_run_derby_game;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}
