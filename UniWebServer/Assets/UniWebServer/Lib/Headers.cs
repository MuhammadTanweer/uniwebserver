using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UniWebServer
{
    public class Headers
    {
        public void Add (string name, string value)
        {
            GetAll (name).Add (value);
        }

        public string Get (string name)
        {
            List<string> header = GetAll (name);
            if (header.Count == 0) {
                return "";
            }
            return header [0];
        }

        public bool Contains (string name)
        {
            List<string> header = GetAll (name);
            if (header.Count == 0) {
                return false;
            }
            return true;
        }

        public List<string> GetAll (string name)
        {
            //name = name.ToLower();
            foreach (string key in headers.Keys) {
                if (name.ToLower () == key.ToLower ()) {
                    return headers [key];
                }
            }
            List<string> newHeader = new List<string> ();
            headers.Add (name, newHeader);
            return newHeader;
        }

        public void Set (string name, object value)
        {
            List<string> header = GetAll (name);
            header.Clear ();
            header.Add (value.ToString ());
        }

        public void Pop (string name)
        {
            if (headers.ContainsKey (name)) {
                headers.Remove (name);
            }
        }

        public string[] Keys {
            get {
                return headers.Keys.ToArray ();
            }
        }

        public void Clear ()
        {
            headers.Clear ();
        }

        public override string ToString ()
        {
            var sb = new StringBuilder ();
            foreach (string name in headers.Keys) {
                foreach (string value in headers[name]) {
                    sb.AppendFormat ("{0}: {1}\r\n", name, value);
                }
            }
            return sb.ToString ();
        }

        public void Read (string headerText)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes (headerText);
            Read (bytes);
        }

        public void Read (byte[] bytes)
        {
            var ms = new MemoryStream (bytes);
            Read (ms);
        }

        public void Read (Stream stream)
        {
            var reader = new StreamReader (stream);
            Read (reader);
        }

        public void AddHeaderLine (string line)
        {
            var parts = line.Split (new char[] {':'}, 2);
            if (parts.Length == 2)
                Add (parts [0].Trim (), parts [1].Trim ());
        }

        public void Read (StreamReader reader)
        {
            while (true) {
                var line = reader.ReadLine ();
                if (line == null || line.Trim() == string.Empty)
                    break;
                AddHeaderLine (line);
            }
        }

        Dictionary<string, List<string>> headers = new Dictionary<string, List<string>> ();
	
    }
}
