
/* 
 * Copyright (c) 2018 anicca048
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Windows.Forms;

namespace Kebab
{
    static class Program
    {
        // Name of program for repeated use.
        public const string Name = @"Kebab";
        // Version of the program (for releases).
        public const string Version = @"1.1.3.3";

        // Links and strings for update checking mechanism.
        public const string GithubWEB_LatestReleaseURL = @"https://github.com/anicca048/Kebab/releases/latest";
        public const string GithubAPI_LatestReleaseURL = @"https://api.github.com/repos/anicca048/Kebab/releases/latest";
        public const string GithubAPI_ReleaseTagElementName = @"tag_name";
        public const string GithubAPI_ReleaseTagElementValue = @"v1.1.3.3_Windows_x86_64";
        // Github API requires a http user agent to be set.
        public const string GithubAPI_HTTPUserAgent = (Program.Name + "/" + Program.Version);

        // Name of file to store config variables (in json format).
        public const string ConfigFileName = @"kebab_config.json";

        // Entry point for program.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
