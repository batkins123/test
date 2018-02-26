namespace GSAWindowsParser
{
    using MiscUtil.IO;
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    class Program
    {
        // Application directory
        static string applicationDirectory = @"C:\Users\batki\Documents\Source Code\GSAWindows";

        // Application libraries
        static string[] applicationLibraries =
        {
            @"OpenSource\Source\applicat",
            @"OpenSource\Source\account",
            @"OpenSource\Source\acctg",
            @"OpenSource\Source\address",
            @"OpenSource\Source\admin",
            @"OpenSource\Source\anc_uo1",
            @"OpenSource\Source\anc_uo2",
            @"OpenSource\Source\anc_win",
            @"OpenSource\Source\app_obj",
            @"OpenSource\Source\audit",
            @"OpenSource\Source\bencat",
            @"OpenSource\Source\bnftadj",
            @"OpenSource\Source\bnftelig",
            @"OpenSource\Source\cladmin1",
            @"OpenSource\Source\cladmin2",
            @"OpenSource\Source\cladmin3",
            @"OpenSource\Source\clienadm",
            @"OpenSource\Source\clientinquiry",
            @"OpenSource\Source\cli_offg",
            @"OpenSource\Source\clminfo1",
            @"OpenSource\Source\clminfo2",
            @"OpenSource\Source\clminfo3",
            @"OpenSource\Source\clminfo4",
            @"OpenSource\Source\clminfo5",
            @"OpenSource\Source\contact1",
            @"OpenSource\Source\contact2",
            @"OpenSource\Source\copy",
            @"OpenSource\Source\co_design",
            @"OpenSource\Source\cwr_common",
            @"OpenSource\Source\dbobj",
            @"OpenSource\Source\dentalkfi",
            @"OpenSource\Source\drugpkg",
            @"OpenSource\Source\enableengine",
            @"OpenSource\Source\enrollment",
            @"OpenSource\Source\enrollment1",
            @"OpenSource\Source\feeguid",
            @"OpenSource\Source\frm_wrk0",
            @"OpenSource\Source\frm_wrk1",
            @"OpenSource\Source\gsostruc",
            @"OpenSource\Source\hcsa",
            @"OpenSource\Source\kfibalance",
            @"OpenSource\Source\ocr_kfi1",
            @"OpenSource\Source\ocr_kfi2",
            @"OpenSource\Source\ocr_kfi3",
            @"OpenSource\Source\ocr_kfi4",
            @"OpenSource\Source\lr_comp",
            @"OpenSource\Source\mrktg0",
            @"OpenSource\Source\mrktg1",
            @"OpenSource\Source\notepad",
            @"OpenSource\Source\partacum",
            @"OpenSource\Source\partcndt",
            @"OpenSource\Source\pfc_obj",
            @"OpenSource\Source\pkg_co",
            @"OpenSource\Source\ppoprice",
            @"OpenSource\Source\pricing",
            @"OpenSource\Source\provider",
            @"OpenSource\Source\provprc",
            @"OpenSource\Source\prvdrppo",
            @"OpenSource\Source\prvdrtyp",
            @"OpenSource\Source\pte0",
            @"OpenSource\Source\pte1",
            @"OpenSource\Source\pte2",
            @"OpenSource\Source\pte3",
            @"OpenSource\Source\pte4",
            @"OpenSource\Source\pte5",
            @"OpenSource\Source\pte6",
            @"OpenSource\Source\pte7",
            @"OpenSource\Source\ratecopy",
            @"OpenSource\Source\rfpren",
            @"OpenSource\Source\rule",
            @"OpenSource\Source\ruleoffg",
            @"OpenSource\Source\subnqry1",
            @"OpenSource\Source\subnqry2",
            @"OpenSource\Source\subxfer1",
            @"OpenSource\Source\subxfer2",
            @"OpenSource\Source\support1",
            @"OpenSource\Source\support2",
            @"OpenSource\Source\taboutl",
            @"OpenSource\Source\tuition",
            @"OpenSource\Source\util_obj",
            @"OpenSource\Source\vision",
            @"OpenSource\Source\proxy",
            @"OpenSource\Source\telephony",
            @"SecureSource\Source\adminsec",
            @"SecureSource\Source\admnobj",
            @"SecureSource\Source\web_security"
        };

        static string[] applicationLibrariesShort =
        {
            @"OpenSource\Source\applicat",
            @"OpenSource\Source\account",
            @"OpenSource\Source\acctg",
            @"OpenSource\Source\address",
            @"OpenSource\Source\admin",
            @"OpenSource\Source\anc_uo1",
            @"OpenSource\Source\anc_uo2",
            @"OpenSource\Source\anc_win",
            @"OpenSource\Source\app_obj"
        };

        // List of application elements discovered by the process
        static List<Element> applicationElements = new List<Element>();

        // For process state
        static bool forwardDeclaration;
        static bool forwardPrototypeDeclaration;
        static bool globalTypeDeclaration;
        static bool globalVariableDeclaration;
        static bool localTypeDeclaration;
        static bool localVariableDeclaration;

        // For input cleanup
        static string commentMultipleLine = @"/\*(.*?)\*/";
        static string commentSingleLine = @"//(.*?)\r?\n";
        static string regularString = @"""((\\[^\n]|[^""\n])*)""";
        static string verbatimString = @"@(""[^""]*"")+";
        static string regularExpression1 = commentMultipleLine + @"|" + commentSingleLine + @"|" + regularString + @"|" + verbatimString;
        static string regularExpression2 = @"[\t]{1,}";
        static string regularExpression3 = @"[=]";
        static string regularExpression4 = @"[,]";
        static string regularExpression5 = @"[ ]{2,}";
        static string regularExpression6 = @"\s+(?=[^[\]]*\])";

        /// <summary>
        /// Main thread
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Process libraries
            applicationLibraries.ToList().ForEach(x => { ProcessLibrary(applicationDirectory + @"\" + x); });
            //applicationLibrariesShort.ToList().ForEach(x => { ProcessLibrary(applicationDirectory + @"\" + x); });
            // Output elements
            applicationElements.ToList().ForEach(x => { Debug.WriteLine("{0}\t{1}\t{2}\t{3}", x.Category, x.Name, x.Type, x.Within); });
            // Keep window open until key is pressed
            Console.ReadKey();
        }

        /// <summary>
        /// Process library
        /// </summary>
        /// <param name="library"></param>
        static void ProcessLibrary(string library)
        {
            foreach (string file in Directory.EnumerateFiles(library))
            {
                // Skip datawindow source
                if (Path.GetExtension(file) == ".srd")
                    continue;
                using (StreamReader reader = new StreamReader(file))
                {
                    // Read file into string
                    string fileContent = reader.ReadToEnd();
                    // Remove comments
                    fileContent = Regex.Replace(fileContent, regularExpression1, x => { if (x.Value.StartsWith("/*") || x.Value.StartsWith("//")) { return x.Value.StartsWith("//") ? Environment.NewLine : ""; } return x.Value; }, RegexOptions.Singleline);
                    // Flag for detecting/skipping non-PowerScript files
                    bool powerScript = false;
                    string globalType = "";
                    forwardDeclaration = false;
                    forwardPrototypeDeclaration = false;
                    globalTypeDeclaration = false;
                    globalVariableDeclaration = false;
                    localTypeDeclaration = false;
                    localVariableDeclaration = false;
                    string extendedLine = @"";
                    bool nextLine = false;
                    // Process file content one line at a time
                    foreach (string line in new LineReader(() => new StringReader(fileContent)))
                    {
                        // Replace consecutive tabs in line with 1 space
                        string adjustedLine = Regex.Replace(line, regularExpression2, " ", RegexOptions.None);
                        // Replace '=' in line with " = "
                        adjustedLine = Regex.Replace(adjustedLine, regularExpression3, " = ", RegexOptions.None);
                        // Replace ',' in line with " , "
                        adjustedLine = Regex.Replace(adjustedLine, regularExpression4, " , ", RegexOptions.None);
                        // Replace consecutive spaces in line with 1 space
                        adjustedLine = Regex.Replace(adjustedLine, regularExpression5, " ", RegexOptions.None);
                        // Remove spaces embedded in square brackets
                        adjustedLine = Regex.Replace(adjustedLine, regularExpression6, "", RegexOptions.None);
                        //
                        adjustedLine = adjustedLine.Trim();
                        //
                        adjustedLine = adjustedLine.ToLower();
                        if (adjustedLine.EndsWith(@"&") || true == nextLine)
                        {
                            extendedLine += adjustedLine;
                            if (!adjustedLine.EndsWith(@"&") && true == nextLine)
                            {
                                nextLine = false;
                                adjustedLine = extendedLine;
                                extendedLine = @"";
                            }
                            else
                            {
                                nextLine = true;
                                continue;
                            }
                        }
                        //
                        adjustedLine = adjustedLine.Replace('&', ' ');
                        // Replace consecutive spaces in line with 1 space
                        adjustedLine = Regex.Replace(adjustedLine, regularExpression5, " ", RegexOptions.None);
                        // Split line on space after trimming whitespace at ends and converting to lowercase
                        string[] substring = Regex.Split(adjustedLine.ToLower(), @" ");
                        // Skip empty line
                        if (@"" == substring[0])
                            continue;
                        // We have found a PowerScript file
                        // Move on to next line
                        if (substring[0].StartsWith(@"ha$pbexportheader$"))
                        {
                            powerScript = true;
                            continue;
                        }
                        // We have found a non-PowerScript file
                        // Move on to next file
                        if (false == powerScript && !substring[0].StartsWith(@"ha$pbexportheader$"))
                            break;
                        // Flag for triggering move on to next file
                        bool moveOnToNextFile = false;
                        // Process line
                        switch(substring[0])
                        {
                            case @"forward":
                                forwardDeclaration = true;
                                if (1 < substring.Count() && @"prototypes" == substring[1])
                                    forwardPrototypeDeclaration = true;
                                continue;
                            case @"global":
                                if (1 < substring.Count())
                                    switch (substring[1])
                                    {
                                        case @"type":
                                            globalTypeDeclaration = true;
                                            if (true == forwardDeclaration)
                                            {
                                                applicationElements.Add(new Element(substring[2], substring[4], "class", "global"));
                                                CodeCompileUnit compileUnit = new CodeCompileUnit();
                                                CodeNamespace ns= new CodeNamespace("GSAWindows");
                                                compileUnit.Namespaces.Add(ns);
                                                ns.Imports.Add(new CodeNamespaceImport("System"));
                                                ns.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));
                                                CodeTypeDeclaration c = new CodeTypeDeclaration(substring[2]);
                                                if (@"window" == substring[4])
                                                    c.BaseTypes.Add(@"Form");
                                                else if (@"menu" == substring[4])
                                                    c.BaseTypes.Add(@"MainMenu");
                                                else if (@"userobject" == substring[4])
                                                    c.BaseTypes.Add(@"UserControl");
                                                else if (@"throwable" == substring[4])
                                                    c.BaseTypes.Add(@"Exception");
                                                else if (@"picture" == substring[4])
                                                    c.BaseTypes.Add(@"PictureBox");
                                                else if (@"multilineedit" == substring[4] || @"singlelineedit" == substring[4])
                                                    c.BaseTypes.Add(@"TextBox");
                                                else if (@"datawindow" == substring[4])
                                                    c.BaseTypes.Add(@"DataWindow");
                                                else if (@"datastore" == substring[4])
                                                    c.BaseTypes.Add(@"DataStore");
                                                else if (@"commandbutton" == substring[4])
                                                    c.BaseTypes.Add(@"Button");
                                                else if (@"statictext" == substring[4])
                                                    c.BaseTypes.Add(@"Label");
                                                else if (@"transaction" == substring[4])
                                                    c.BaseTypes.Add(@"Transaction");
                                                else if (@"connection" == substring[4])
                                                    c.BaseTypes.Add(@"Connection");
                                                else if (@"error" == substring[4])
                                                    c.BaseTypes.Add(@"Error");
                                                else if (@"tab" == substring[4])
                                                    c.BaseTypes.Add(@"TabPage");
                                                else if (@"treeview" == substring[4])
                                                    c.BaseTypes.Add(@"TreeView");
                                                else if (@"nonvisualobject" == substring[4])
                                                { }
                                                else if (@"application" == substring[4])
                                                { }
                                                else
                                                    c.BaseTypes.Add(substring[4]);
                                                ns.Types.Add(c);
                                                //if (@"menu" == substring[4] || substring[2].StartsWith(@"m_"))
                                                //{
                                                //    CodeConstructor m = new CodeConstructor();
                                                //    m.Attributes = MemberAttributes.Public;
                                                //    m.Parameters.Add(new CodeParameterDeclarationExpression(@"MenuItem[]", @"items"));
                                                //    m.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("items"));
                                                //    c.Members.Add(m);
                                                //}
                                                Generator.GenerateCSharpCode(compileUnit, @"WFApp\" + substring[2]);
                                            }
                                            globalType = substring[2];
                                            continue;
                                        case @"variables":
                                            globalVariableDeclaration = true;
                                            continue;
                                    }
                                if (true == forwardDeclaration)
                                    applicationElements.Add(new Element(substring[2], substring[1], "variable", "global"));
                                else if (substring[1] == substring[2])
                                    continue;
                                else
                                    moveOnToNextFile = true;
                                break;
                            case @"end":
                                if (1 < substring.Count())
                                    switch (substring[1])
                                    {
                                        case @"forward":
                                            forwardDeclaration = false;
                                            continue;
                                        case @"prototypes":
                                            forwardPrototypeDeclaration = false;
                                            continue;
                                        case @"type":
                                            globalTypeDeclaration = localTypeDeclaration = false;
                                            continue;
                                        case @"variables":
                                            globalVariableDeclaration = localVariableDeclaration = false;
                                            continue;
                                    }
                                break;
                            case @"type":
                                if (true == forwardDeclaration)
                                {
                                    localTypeDeclaration = true;
                                    applicationElements.Add(new Element(substring[1], substring[3], "class", globalType));
                                }
                                else if (substring[1] == "prototypes")
                                    forwardPrototypeDeclaration = true;
                                else if (substring[1] == "variables")
                                    localVariableDeclaration = true;
                                continue;
                            case @"public":
                                if (false == forwardPrototypeDeclaration)
                                    moveOnToNextFile = true;
                                else if (@"function" == substring[1])
                                    applicationElements.Add(new Element(substring[3], substring[2], "function", globalType));
                                else if (@"subroutine" == substring[1])
                                    applicationElements.Add(new Element(substring[2], "n/a", "subroutine", globalType));
                                break;
                            case @"private":
                                if (false == forwardPrototypeDeclaration)
                                    moveOnToNextFile = true;
                                else if (@"function" == substring[1])
                                    applicationElements.Add(new Element(substring[3], substring[2], "function", globalType));
                                else if (@"subroutine" == substring[1])
                                    applicationElements.Add(new Element(substring[2], "n/a", "subroutine", globalType));
                                break;
                            case @"protected":
                                if (false == forwardPrototypeDeclaration)
                                    moveOnToNextFile = true;
                                else if (@"function" == substring[1])
                                    applicationElements.Add(new Element(substring[3], substring[2], "function", globalType));
                                else if (@"subroutine" == substring[1])
                                    applicationElements.Add(new Element(substring[2], "n/a", "subroutine", globalType));
                                break;
                            case @"event":
                                if (true == globalTypeDeclaration)
                                {
                                    if (@"(" == substring[2])
                                        applicationElements.Add(new Element(substring[1], "", "event", globalType));
                                    else
                                        applicationElements.Add(new Element(substring[1], substring[2], "event", globalType));
                                }
                                else
                                    moveOnToNextFile = true;
                                break;
                            case @"on":
                                if (false == globalTypeDeclaration)
                                    moveOnToNextFile = true;
                                break;
                        }
                        if ((true == globalVariableDeclaration || true == localVariableDeclaration) && !(substring[0].StartsWith("public") || substring[0].StartsWith("private") || substring[0].StartsWith("protected")))
                        {
                            bool done = false;
                            int offset = 0;
                            if (@"constant" == substring[0])
                                offset = 1;
                            for (int i = offset + 1; i < substring.Count(); i++)
                            {
                                if (@"," == substring[i])
                                    continue;
                                if (@"=" == substring[i])
                                    done = true;
                                if (true == done)
                                    continue;
                                if (true == globalVariableDeclaration)
                                    applicationElements.Add(new Element(substring[i].Trim(), substring[offset], "variable", "global"));
                                else
                                    applicationElements.Add(new Element(substring[i].Trim(), substring[offset], "variable", globalType));
                                continue;
                            }
                        }
                        if (true == moveOnToNextFile)
                            break;
                    }
                }
            }
        }
    }
}
