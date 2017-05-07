using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class RipCMD : cmd
    {
        public override string name { get { return "rip"; } }
        public override PermType perm { get { return PermType.ALL; } }
        public override bool canUseAsClient { get { return true; } }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!win: Win the game !");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            Random r = new Random();
            float color = (float)r.NextDouble() * 360;
            Utilities.sendMessage("[b][" + ColorEx.ColorToHexNGUI(new ColorHSB(color, 0.5f, 0.5f, 1f).ToColor()) + "]" + ripList[r.Next(ripList.Count)] + "[-][/b]");
        }

        public static List<string> ripList = new List<string>
        {
            "ACCESS VIOLATION!",
            "AW, THAT'S TOO BAD!",
            "BOGUS!",
            "BUCKETS OF TEARS!",
            "BUMMER!",
            "CAR BOOM!",
            "CASUAL!",
            "CRIKEY!",
            "CRITICAL ERROR!",
            "DENIED!",
            "DESTROYED!",
            "DIVISION BY ZERO!",
            "DRIVING MISS DAISY!",
            "ERROR 404: SUCCESS NOT FOUND!",
            "FAREWELL!",
            "FRAGGED!",
            "FRAGMENTED!",
            "GAME OVER!",
            "HOLY TOLEDO!",
            "INERTIA SUCKS!",
            "IT KEEPS HAPPENING!",
            "KNOCKOUT!",
            "LAMESAUCE!",
            "MAYDAY!",
            "MONDO DISASTER!",
            "NICE TRY!",
            "NITRAGIC!",
            "NITRONICRASHED!",
            "NO CIGAR!",
            "NUCLEAR!",
            "NULL TERMINATED!",
            "REJECTED!",
            "ROADKILL!",
            "SAD DAY!",
            "SAYONARA!",
            "SHORT CIRCUIT!",
            "SHUT DOWN!",
            "SO SAD!",
            "STEP ON IT!",
            "STUDENT DRIVER!",
            "TAKE DOWN!",
            "THAT'S GOTTA HURT!",
            "THAT'S WHACK!",
            "TOO BAD!",
            "UNCOOL!",
            "UNFORTUNATE!",
            "VAPORIZED!",
            "WEAKSAUCE!",
            "WRECKED!",
            "WRONG TURN!",
            "YOU'D BETTER UPGRADE THAT GRAPHICS CARD--YOUR COMPUTER IS TOO SLOW!",
            "YOU'RE OUT OF CONTROL!",
        };
    }
}
