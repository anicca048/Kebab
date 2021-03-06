﻿
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
        static public readonly string Name = @"Kebab";
        // Version of the program (for releases).
        static public readonly string Version = @"1.3.0.0";

        // Links and strings for update checking mechanism.
        static public readonly string GithubWEB_RepoURL = @"https://github.com/anicca048/Kebab";
        static public readonly string GithubWEB_LatestReleaseURL = @"https://github.com/anicca048/Kebab/releases/latest";
        static public readonly string GithubAPI_LatestReleaseURL = @"https://api.github.com/repos/anicca048/Kebab/releases/latest";
        static public readonly string GithubAPI_ReleaseTagElementName = @"tag_name";
        static public readonly string GithubAPI_ReleaseTagElementValue = @"v1.3.0.0_Windows_x86_64";
        // Github API requires a http user agent to be set.
        static public readonly string GithubAPI_HTTPUserAgent = (Program.Name + "/" + Program.Version);

        // Name of file to store config variables (in json format).
        static public readonly string ConfigFileName = @"kebab_config.json";
        // Name of file with documentation information.
        static public readonly string ReadmeFileName = @"docs\README.md";

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
