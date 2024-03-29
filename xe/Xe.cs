/* Copyright (c) Citrix Systems, Inc. 
 * All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

// project created on 23/02/2007 at 11:13
using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using CommandLib;

public class Global{
    public static void error(string x){
        Console.WriteLine("Error: " + x);
    }
    public static void debug(string msg, thinCLIProtocol tCLIprotocol){
        if (tCLIprotocol.conf.debug)
            Console.WriteLine("Debug: " + msg);
    }
    public static void usage(){
        Console.WriteLine("XSConsole XenServer Console (xe) based on XCP or XenCenter code");
        Console.WriteLine("Ported by Fandi Gunawan https://github.com/fandigunawan/XSConsole");
        Console.WriteLine("Runtime: {0} on {1}", Environment.Version, Environment.OSVersion);
        Console.WriteLine();
        Console.WriteLine("Usage: xe [parameter] <command> <argument>");
        Console.WriteLine();
        Console.WriteLine("Parameter:");
        Console.WriteLine("  -s <server> mandatory");
        Console.WriteLine("  -u <username> mandatory");
        Console.WriteLine("  -pw <password> mandatory ");
        Console.WriteLine("  -p <port> optional, default 443");
        Console.WriteLine("use system variable instead:");
        Console.WriteLine("  XE_HOST : Hostname");
        Console.WriteLine("  XE_USERNAME : Username");
        Console.WriteLine("  XE_PASSWORD : pasword");
        Console.WriteLine("  XE_PORT : Port");
        Console.WriteLine();
        Console.WriteLine("Additional parameter");
        Console.WriteLine("  -version : show version");
        Console.WriteLine("  -nossl : do not use SSL");
        Console.WriteLine("  -debug : display debug log");
        Console.WriteLine("  -display-config : display command configuration");
		Console.WriteLine("  -h : display this help");
        Console.WriteLine("For XAPI help, use xe -s <server> -u <user> -pw <password> [-p <port>] help");
    }
}



namespace thincli
{
    class MainClass
    {

    
        public static void Main(string[] args)
        {
            thinCLIProtocol tCLIprotocol = new thinCLIProtocol(new delegateGlobalError(Global.error), new delegateGlobalUsage(Global.usage),
                                       new delegateGlobalDebug(Global.debug), new delegateConsoleWrite(delegate(string s) { Console.Write(s); }),
                                       new delegateConsoleWriteLine(delegate(string s) { Console.WriteLine(s); }),
                                       new delegateConsoleReadLine(delegate { return Console.ReadLine(); }),
                                       new delegateExit(delegate(int i) { Environment.Exit(i); }),
                                       new delegateProgress(delegate(int i) {}),
                                       new Config());

            String body = "";
            Char[] eqsep = {'='};
            
            bool isDisplayConfig = false;

            string temp = Environment.GetEnvironmentVariable("XE_HOST");
            if(temp != null)
            {
                tCLIprotocol.conf.hostname = temp;
            }

            temp = Environment.GetEnvironmentVariable("XE_USERNAME");
            if(temp != null)
            {
                tCLIprotocol.conf.username = temp;
            }

            temp = Environment.GetEnvironmentVariable("XE_PASSWORD");
            if(temp != null)
            {
                tCLIprotocol.conf.password = temp;
            }

            temp = Environment.GetEnvironmentVariable("XE_PORT");
            if(temp != null)
            {
                tCLIprotocol.conf.port = Int32.Parse(temp);
            }

            for(int i=0; i<args.Length; i++) {
                String s = args[i];
                try
                {
                    if (s.StartsWith("server")){
                        tCLIprotocol.conf.hostname = (s.Split(eqsep))[1];
                    } else if (s.Equals("-s")) {
                        tCLIprotocol.conf.hostname = args[++i]; 
                    } else if (s.Equals("-u")){
                        tCLIprotocol.conf.username = args[++i]; 
                    } else if (s.Equals("-pw")){
                        tCLIprotocol.conf.password = args[++i]; 
                     } else if (s.Equals("-p")){
                        tCLIprotocol.conf.port = Int32.Parse(args[++i]); 
                    } else if (s.Equals("--nossl")){
                        tCLIprotocol.conf.nossl = true;
                    } else if (s.Equals("-debug")){
                        tCLIprotocol.conf.debug = true;
                    } else if(s.Equals("-display-config")) {
                        isDisplayConfig = true;
                    } else if(s.Equals("-h")) {
                        Global.usage();
						Environment.Exit(0);
                    } else if (s.Equals("-version")){
                        Console.WriteLine("ThinCLI protocol: " + tCLIprotocol.major + "." + tCLIprotocol.minor);
                        Environment.Exit(0);
                    } else body += s + "\n";
                }
                catch {
                    Global.error("Failed to parse command-line arguments");
                    Global.usage();
                    Environment.Exit(1);
                }
            }
            if (tCLIprotocol.conf.hostname.Equals(""))
            {
                Global.error("No hostname was specified.");
                Global.usage();
                Environment.Exit(1);
            }

            if(isDisplayConfig)
            {
                Console.WriteLine("Host : " + tCLIprotocol.conf.hostname);
                Console.WriteLine("Username : " + tCLIprotocol.conf.username);
                Console.WriteLine("Password : "  + "".PadRight(tCLIprotocol.conf.password.Length, '*'));
            }

            Messages.performCommand(body, tCLIprotocol);
        }
    }
}
