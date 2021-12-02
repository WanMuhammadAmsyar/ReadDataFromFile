using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ReadDataFromFiles
{
    class Program
    {
        //email: cwlok@motorolasolutions.com
        static void Main(string[] args)
        {
            string _filePath = "", _textData="";
            int _lineNumber = 0;
            Regex _checkString = new Regex("[^A-Za-z0-9]");
            Dictionary<string, string> _softwareInfo = new Dictionary<string, string>();
            Dictionary<string, string> _otherVersion = new Dictionary<string, string>();

            Console.WriteLine("Please Insert File Path: ");
            _filePath = Console.ReadLine().Trim('"');
            _textData = File.ReadAllText(_filePath);
            StringReader _reader = new StringReader(_textData);

            for(string _line = _reader.ReadLine(); _line != null; _line = _reader.ReadLine())
            {
                if (!_line.Contains("*") && _checkString.IsMatch(_line))
                {
                    if (_line.StartsWith("RELEASE"))
                    {
                        _softwareInfo.Add(_line.Split(':')[0].ToLower(), _line.Split(':')[1].ToLower().Trim(' '));
                    }
                    else if (_line.StartsWith("Artifactory"))
                    {
                        _softwareInfo.Add(_line.Split(':')[0].ToLower().Trim(' ').Replace(' ', '_'), "");
                    }
                    else if (_line.Contains("https://artifactory.com"))
                    {
                        _softwareInfo["artifactory_url"] = _line;
                    }
                    else if (_line.Contains("OTHERS VERISON"))
                    {
                        _softwareInfo.Add(_line.Trim(' ').ToLower().Replace(' ', '_'), "");
                    }else if (_line.Contains("setting_app"))
                    {
                        _otherVersion.Add(_line.Split(':')[0], _line.Split(':')[1]);
                    }else if (_line.Contains("dial_app"))
                    {
                        _otherVersion.Add(_line.Split(':')[0], _line.Split(':')[1]);
                        _softwareInfo["other_version"] = JsonConvert.SerializeObject(_otherVersion);
                    }
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(_softwareInfo));


            Console.ReadKey();
        }
    }
}
