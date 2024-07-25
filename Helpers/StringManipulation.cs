using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking.Helpers
{
    public class StringManipulation
    {
        public String CenterString(string _value, int totalLength)
        {
            return _value.PadLeft(((totalLength - _value.Length) / 2) + _value.Length).PadRight(totalLength);
        }

        public String ThreeColumnString(Boolean _alignright, string _1ststring, int _1stcolumnlength, string _2ndstring, int _2ndcolumnlength, string _3rdstring, int _3rdcolumnlength)
        {
            string _1ststr, _2ndstr, _3rdstr;
            if (!_alignright)
                _1ststr = Suffixchar(" ", _1stcolumnlength, _1ststring);
            else
                _1ststr = Prefixchar(" ", _1stcolumnlength, _1ststring);

            if (!_alignright)
                _2ndstr = Suffixchar(" ", _2ndcolumnlength, _2ndstring);
            else
                _2ndstr = Prefixchar(" ", _2ndcolumnlength, _2ndstring);

            if (!_alignright)
                _3rdstr = Suffixchar(" ", _3rdcolumnlength, _3rdstring);
            else
                _3rdstr = Prefixchar(" ", _3rdcolumnlength, _3rdstring);

            return _1ststr + _2ndstr + _3rdstr;
        }

        public String ThreeColumnStringWithAlign(Boolean _1stalignright, string _1ststring, int _1stcolumnlength, Boolean _2ndalignright, string _2ndstring, int _2ndcolumnlength, Boolean _3rdalignright, string _3rdstring, int _3rdcolumnlength)
        {
            string _1ststr, _2ndstr, _3rdstr;
            if (!_1stalignright)
                _1ststr = Suffixchar(" ", _1stcolumnlength, _1ststring);
            else
                _1ststr = Prefixchar(" ", _1stcolumnlength, _1ststring);

            if (!_2ndalignright)
                _2ndstr = Suffixchar(" ", _2ndcolumnlength, _2ndstring);
            else
                _2ndstr = Prefixchar(" ", _2ndcolumnlength, _2ndstring);

            if (!_3rdalignright)
                _3rdstr = Suffixchar(" ", _3rdcolumnlength, _3rdstring);
            else
                _3rdstr = Prefixchar(" ", _3rdcolumnlength, _3rdstring);

            return _1ststr + _2ndstr + _3rdstr;
        }

        public void CenterLongString(StringBuilder sb, string _longstring, int chunkSize)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(_longstring))
                    return;

                string[] Arraystr = _longstring.Split("\n"[0]);
                for (int i = 0; i < Arraystr.Length; i++)
                {
                    string str = Arraystr[i];
                    for (int x = 0; x < str.Length; x += chunkSize)
                    {
                        sb.AppendLine(CenterString(str.Substring(x, Math.Min(chunkSize, str.Length - x)), 40));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}>>{1}>>{2}>>{3}", ex.Message, ex.StackTrace, ex.Source, ex.TargetSite));
            }
        }

        public void DisplayLongString(StringBuilder sb, string _longstring, int chunkSize, Boolean _isCenter)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(_longstring))
                    return;

                string[] words = _longstring.Split(' ');
                string line = "";
                foreach (string word in words)
                {
                    if ((line + word).Length > chunkSize)
                    {
                        if (_isCenter)
                            sb.AppendLine(CenterString(line, chunkSize));
                        else
                            sb.AppendLine(line);
                        line = "";
                    }

                    line += string.Format("{0} ", word);
                }
                if (line.Length > 0)
                {
                    if (_isCenter)
                        sb.AppendLine(CenterString(line, chunkSize));
                    else
                        sb.AppendLine(line);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}>>{1}>>{2}>>{3}", ex.Message, ex.StackTrace, ex.Source, ex.TargetSite));
            }
        }

        public String Prefixchar(string wchar, int lenwhole, string _thestring)
        {
            StringBuilder sb = new StringBuilder();
            int numspace = lenwhole - _thestring.Length;
            for (int i = 1; i <= numspace; i++) sb.Append(wchar);

            sb.Append(_thestring);

            return sb.ToString();
        }

        public String Suffixchar(string wchar, int lenwhole, string _thestring)
        {
            if (_thestring.Length >= lenwhole)
                return _thestring.Substring(0, lenwhole);

            StringBuilder sb = new StringBuilder();
            int numspace = lenwhole - _thestring.Length;
            sb.Append(_thestring);
            for (int i = 1; i <= numspace; i++) sb.Append(wchar);
            return sb.ToString();
        }

        public void HexString2Ascii(string toConvert, out string hexString)
        {
            hexString = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= toConvert.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(toConvert.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            hexString = sb.ToString();
        }
    }
}
