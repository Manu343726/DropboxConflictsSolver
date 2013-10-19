using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

namespace DropboxConflictsSolver
{
    class Program
    {
        private static string APPNAME = "DrobpxConflictsSolver";

        private static List<string> matches = new List<string>();

        private enum Action
        {
            DELETE,
            SHOW,
            HELP,
            UNKNOWN
        };

        private enum Option
        {
            RECURSIVELY,
            ROOT_ONLY,
            UNKNOWN
        };

        private enum DebugSettings
        {
            ALLLOG_ENABLED,
            ALLLOG_DISABLED,
            UNKNOWN
        };

        private const Action        DEFAULT_ACTION         = Action.SHOW;
        private const Option        DEFAULT_OPTION         = Option.RECURSIVELY;
        private const DebugSettings DEFAULT_DEBUG_SETTINGS = DebugSettings.ALLLOG_DISABLED;

        private static Action parseAction(String arg)
        {
            switch (arg)
            {
                case "-d": return Action.DELETE;
                case "-s": return Action.SHOW;
                case "-h": return Action.HELP;
                default: return Action.UNKNOWN;
            }
        }

        private static Option parseOption(String arg)
        {
            switch (arg)
            {
                case "-r": return Option.RECURSIVELY;
                case "-n": return Option.ROOT_ONLY;
                default: return Option.UNKNOWN;
            }
        }

        private static DebugSettings parseDebugSettings(String arg)
        {
            switch (arg)
            {
                case "-dl": return DebugSettings.ALLLOG_ENABLED;
                default: return DebugSettings.UNKNOWN;
            }
        }

        private static void showHelp()
        {
            Console.WriteLine("usage: " + APPNAME + " [rootDirectory] [regex] [Action] [Option]");
            Console.WriteLine();
            Console.WriteLine("rootDirectory: Searching directory");
            Console.WriteLine("regex: Regular expression used to match files by name");
            Console.WriteLine("Action: Action to be performed");
            Console.WriteLine("        -d Deletes matching files.");
            Console.WriteLine("        -s Shows matching files (Default).");
            Console.WriteLine("        -h Shows help.");
            Console.WriteLine("Option: Performed action options.");
            Console.WriteLine("        -r Loops directories recoursively starting at rootDirectory.");
            Console.WriteLine("        -n Search at rootDirectory only (Default).");
            Console.WriteLine("Debug settings: Shows extra information during the search");
            Console.WriteLine("        -dl Logs every file transversed, not only matches.");
            Console.WriteLine();
        }

        private static void executeCommand(DirectoryInfo dir, Regex regex, Action action, Option option , DebugSettings debug_settings)
        {
            FileInfo[] files = null;
            bool access_ok = false;

            if (action == Action.HELP)
                showHelp();
            else
            {
                try
                {
                    files = dir.GetFiles();
                    access_ok = true;
                }
                catch (UnauthorizedAccessException)
                { Console.WriteLine("ERROR: Unauthorized access to path '" + dir.FullName + "'"); }
                catch (Exception)
                { Console.WriteLine("ERROR: UNKNOWN"); }


                if (!access_ok) return;

                foreach (var file in files)
                {
                    if (regex.IsMatch(file.Name))
                    {
                        Console.Write("Match: " + file.FullName + (action == Action.SHOW ? "" : " ... "));

                        matches.Add(file.FullName);

                        if (action == Action.DELETE)
                        {
                            try { File.Delete(file.FullName); Console.WriteLine("DELETED"); }
                            catch (IOException)
                            { Console.WriteLine("ERROR: FILE IN USE"); }
                            catch (UnauthorizedAccessException)
                            { Console.WriteLine("ERROR: Unauthorized access to file '" + file.Name + "'"); }
                            catch (Exception)
                            { Console.WriteLine("ERROR: UNKNOWN"); }
                        }
                        else
                            Console.WriteLine();
                    }
                    else
                        if( debug_settings == DebugSettings.ALLLOG_ENABLED )
                            Console.WriteLine("Scanning " + file.FullName + " ... ");
                }

                if (option == Option.RECURSIVELY)
                {
                    foreach (var subDir in dir.GetDirectories())
                        executeCommand(subDir, regex, action, option , debug_settings);
                }
            }
        }

        static void Main(string[] args)
        {
            Regex regex = null;
            Action action;
            Option option;
            DebugSettings debug_settings = 0;
            bool error = false;

            if (args.Length > 1 && args.Length <= 5)
            {
                if (Directory.Exists(args[0]))
                {
                    try { regex = new Regex(args[1]); }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("ERROR: '" + args[1] + "' is not a valid regular expression.");
                    }

                    if (regex != null)
                        if (args.Length == 2)
                            executeCommand(new DirectoryInfo(args[0]), regex, DEFAULT_ACTION, DEFAULT_OPTION , DEFAULT_DEBUG_SETTINGS);
                        else
                        {
                            if ((action = parseAction(args[2])) != Action.UNKNOWN)
                                if (args.Length == 3)
                                    executeCommand(new DirectoryInfo(args[0]), regex, action, DEFAULT_OPTION, DEFAULT_DEBUG_SETTINGS);
                                else
                                    if ((option = parseOption(args[3])) != Option.UNKNOWN)
                                        if (args.Length == 4)
                                            executeCommand(new DirectoryInfo(args[0]), regex, action, option, DEFAULT_DEBUG_SETTINGS);
                                        else
                                            if ((debug_settings = parseDebugSettings(args[4])) != DebugSettings.UNKNOWN)
                                                executeCommand(new DirectoryInfo(args[0]), regex, action, option, debug_settings);
                                            else
                                                error = true;
                                    else
                                        error = true;
                            else
                                error = true;
                        }
                }
                else
                    Console.WriteLine("ERROR: '" + args[0] + "' no such directory.");
            }
            else
                error = true;

            if (error)
            {
                Console.WriteLine("ERROR: Bad params. See help:");
                showHelp();
            }
            else
            {
                Console.WriteLine("Finished.");

                if (debug_settings == DebugSettings.ALLLOG_ENABLED)
                    foreach (var match in matches)
                        Console.WriteLine("Match: " + match);
            }
                
        }
    }
}
