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

        private const Action DEFAULT_ACTION = Action.SHOW;
        private const Option DEFAULT_OPTION = Option.RECURSIVELY;

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
            Console.WriteLine();
        }

        private static void executeCommand(DirectoryInfo dir, Regex regex, Action action, Option option)
        {
            if(action == Action.HELP)
                showHelp();
            else
            {
                var files = dir.GetFiles();

                foreach (var file in files)
                {
                    if (regex.IsMatch(file.Name))
                    {
                        Console.Write("Match: " + file.FullName + (action == Action.SHOW ? "" : " ... "));

                        if (action == Action.DELETE)
                        {
                            try { File.Delete(file.FullName); Console.WriteLine("DELETED"); }
                            catch (IOException)
                            { Console.WriteLine("ERROR: FILE IN USE"); }
                            catch (UnauthorizedAccessException)
                            { Console.WriteLine("ERROR: UNAUTHORIZED"); }
                            catch (Exception)
                            { Console.WriteLine("ERROR: UNKNOWN"); }
                        }
                        else
                            Console.WriteLine();
                    }
                }

                if (option == Option.RECURSIVELY)
                {
                    foreach (var subDir in dir.GetDirectories())
                        executeCommand(subDir, regex, action, option);
                }
            }
        }

        static void Main(string[] args)
        {
            Regex regex = null;
            Action action;
            Option option;
            bool error = false;

            if (args.Length > 1 && args.Length < 4)
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
                            executeCommand(new DirectoryInfo(args[0]), regex, DEFAULT_ACTION, DEFAULT_OPTION);
                        else
                        {
                            if ((action = parseAction(args[2])) != Action.UNKNOWN)
                                if (args.Length == 3)
                                    executeCommand(new DirectoryInfo(args[0]), regex, action, DEFAULT_OPTION);
                                else
                                    if ((option = parseOption(args[3])) != Option.UNKNOWN)
                                        executeCommand(new DirectoryInfo(args[0]), regex, action, option);
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
                Console.WriteLine("Finished.");
        }
    }
}
