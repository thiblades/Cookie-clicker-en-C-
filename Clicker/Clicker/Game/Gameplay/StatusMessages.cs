using System;
namespace Clicker.Game {
    public class StatusMessages {
        private struct StatusMessage {
            internal string Message;
            internal ulong Threshold;

            public StatusMessage(ulong score, string msg){
                Message = msg;
                Threshold = score;
            }
        }

        private static StatusMessage[] Messages = {
            new StatusMessage(          100, "Je commence à me sentir bizarre..."),
            new StatusMessage(         1000, "Comment je m'appelle déjà?"),
            new StatusMessage(         5000, "Je crois que je vais vomir..."),
            new StatusMessage(        10000, "OK ils sont où les toilettes?"),
            new StatusMessage(       100000, "Je bois depuis tout à l'heure, mais j'ai encore soif. WTF?"),
            new StatusMessage(       500000, "Hmmm je vais me poser là, marre d'être debout"),
            new StatusMessage(      1000000, "Sang/Alcool = environ 25 000"),
            new StatusMessage(      5000000, "Je peux voir les champs magnétiques à l'oeil nu"),
            new StatusMessage(     10000000, "Tout ce que je bois, ça va où exactement? OH NOOOOONN"),
            new StatusMessage(     50000000, "Je sais pas qui organise cette soirée, mais sa logistique est au top"),
            new StatusMessage(    100000000, "kd24mgoih42gmoihqrmgqihegeùpgre"),
            new StatusMessage(   1000000000, "Je transcende l'espace-temps"),
            new StatusMessage(  10000000000, "WOW! JE COMMENCE À COMPRENDRE L'AUTOMATIQUE!"),
            new StatusMessage( 100000000000, "L̨̺̯a̦ ̤͈̘t̸͓̠̱͙̳̣r̢̦̙a̴͎̻̪̬̲n̠͍̭̞̘ͅs̖̥̗̩f̦o̧̦͈̬̫̙r̝m̵͈̬̼͕̞͕a̯̩̜͉t̶i̵͎̣͓̱̞̭̠o̻̞͓͈̫̝͝n̡̠ ͖͎̩̟͉̤d҉̱̣̦̩̳͈e͎̮̱̱̼ ̱̘̻ͅḼ̭̜̫͚̦̟a͏͙̯p̪̻̣̦l̷̖̝͙ͅa̩͖̣͝c̲̺̫͉e͏̝̝̖̤͍ ̙p̴̠e̠̻͚̟̖͈̘͝r̟͚͎̥̭m̡͇͚̦̼͍͔ͅe͏̤̳̦̖ͅt̮̭̮̜ ̖͖̖̪̻͘a̟̖̳̱̖͞l̠̮̞͇͈͕̬ó͎̼̙̬̝ͅr̺̱̕ͅs͎ ̶̩ͅd̹̲͜e̥͖̖̠͖͇ ̲͕̯͍͉̪̕p̩̫̫̬a҉͕͓s̜̠̮s̭̪̫̤̪e̵͎̗̟̱r̷̪̖ ̖͈͢d̨͚̬̱ḙ̦͍̻͍ ̸l̴͉̘’̬̥̯é͖̞̬q̼͇͉u̻̣͕̫̮a̘̹t͏̠i̷̫̹̦͍͙̪o̥̮͕̩n͚͖̱̠ ̷̩̘̖͎̪̟ͅd͖̝̰i̬f́f̱̫̠͎̤é̘̖͉̙̦͝r͖̭͓̰͎̦͞ͅe̡̦̣͎̘͔n̛̘͚̟ͅti̗̞͍̠̱̫e̬̰͉̱̳͇̫l̲͍͝l̵̳̥̼̦͓̠̼e̟ ͙̺̯̮ț͙̘͞emp͚̫͓͚̤̙͘o̹̫͉͈̮̩̖r̼̹͖͞é̼l̢̯̰̣͓̹l̖͚̤̘͖̲̱e̴̱͔͎͇"),
            new StatusMessage(1000000000000, ". . ."),
        };

        public static string Message(ulong score){
            if( score < Messages[0].Threshold )
                return "";

            for( int i = Messages.Length - 1; i >= 0; --i ){
                ulong threshold = Messages[i].Threshold;

                if( score >= threshold )
                    return Messages[i].Message;
            }

            return "";
        }
    }
}
