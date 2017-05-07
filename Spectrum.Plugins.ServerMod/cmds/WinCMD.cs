using System;
using System.Collections.Generic;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class WinCMD : cmd
    {
        public override string name { get { return "win"; } }
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
            Utilities.sendMessage("[b][" + ColorEx.ColorToHexNGUI(new ColorHSB(color, 1.0f, 1.0f, 1f).ToColor()) + "]" + winList[r.Next(winList.Count)] + "[-][/b]");
        }

        public static List<string> winList = new List<string>
        {
            "ALL RIGHT!",
            "ALRIGHT!",
            "AMAZING!",
            "ASTOUNDING!",
            "AWESOME!",
            "AWESOMESAUCE!",
            "BANGIN'!",
            "BETTER THAN SLICED BREAD!",
            "CHA-CHING!",
            "COOL!",
            "COWABUNGA!",
            "CRAZY!",
            "CUNNING!",
            "DOUBLE RAINBOW!",
            "DUDE!",
            "DYN-O-MITE!",
            "EXCELLENT!",
            "EXTREME!",
            "FABULOUS!",
            "FANTABULOUS!",
            "GOIN' THE DISTANCE!",
            "GOING THE DISTANCE!",
            "GOOD SHOW!",
            "GREAT JOB!",
            "GROOVY!",
            "HARDCORE!",
            "HOLY TOLEDO!",
            "HOT DOG!",
            "INSANE!",
            "IT'S BUSINESS TIME!",
            "JAMMIN'!",
            "KA-BAM SON!",
            "LEGENDARY!",
            "LIKE A BOSS!",
            "NICE!",
            "NIFTY!",
            "NO HANDLEBARS!",
            "NO WAY!",
            "OH SNAP!",
            "OUTTA THE PARK!",
            "PEAS AND CARROTS!",
            "PIZZA!",
            "PROFESSIONAL!",
            "RIDICULOUS!",
            "RIGHT ON!",
            "ROCK N' ROLL!",
            "SCORE!",
            "SIDEWHEELIE!",
            "SPIFFY!",
            "STUNNING!",
            "SUPER!",
            "SUPERFLY!",
            "SUPREME!",
            "TOO GOOD!",
            "TOTALLY AWESOME!",
            "TOTALLY TUBULAR!",
            "TRICKSTER!",
            "TWIST AND SHOUT!",
            "TWISTER CITY!",
            "ULTRA COMBO!",
            "ULTRACOMBO!",
            "UNREAL!",
            "WELL DONE!",
            "WHOA!",
            "WICKED!",
            "WINNING!",
            "WONDERFUL!",
            "YEAH!",
        };
    }
}
