using Google.Protobuf;
using System.IO;
using pbc = global::Google.Protobuf.Collections;

namespace HookKeylogger.Base
{
    /// <summary>
    /// A buffer of Key Presses. Persisted to the a file.
    /// 
    /// Add new Key Presses using the AddKeyPress method do not use the Keys.Add() method.
    /// Weird behavior here. It is not specified in the Proto3 spec anywhere but this is what I found during testing.
    /// Suppose you have a Top level Container protocol message that has repeated messages as a field. (KeyStrokeBuffer)
    /// If you have two instances of that Top Level Message and you save both to the same file opened using append,
    /// then parse the same file starting from the begining you will end up with one of the Top Level Container
    /// messages which containes the child messages of both orgional Container Messages.
    /// The code below uses that fact to append one KeyStroke to the KeyStrokeBuffer container in the binary log. This
    /// is a hack. The proper way to store multiple messages in the same file is to first write the size and parse each
    /// individual chunck of the buffer individually. See: https://developers.google.com/protocol-buffers/docs/techniques#streaming
    /// This is easier, and is really fast, so until it blows up I'm going to leave it like this.
    /// </summary>
    public class KeyPressBuffer
    {
        private string path;
        private KeyPressSet set;
        public KeyPressBuffer(string path)
        {
            this.path = path;
        }

        public pbc::RepeatedField<KeyPress> Keys
        {
            get {
                if (set != null)
                {
                    return set.Keys;
                }
                else
                {
                    var fh = File.Open(path, FileMode.OpenOrCreate);
                    set = KeyPressSet.Parser.ParseFrom(fh);
                    fh.Close();
                    return set.Keys;
                }
            }
        }

        public void AddKeyPress(KeyPress kp)
        {
            var output = File.Open(path, FileMode.Append);
            KeyPressSet b = new KeyPressSet();
            b.Keys.Add(kp);
            b.WriteTo(output);
            output.Close();
        }
    }
}
