// ------------------------------------------------------------------------------
// -- Copyright ERTMS Solutions
// -- Licensed under the EUPL V.1.1
// -- http://joinup.ec.europa.eu/software/page/eupl/licence-eupl
// --
// -- This file is part of ERTMSFormalSpec software and documentation
// --
// --  ERTMSFormalSpec is free software: you can redistribute it and/or modify
// --  it under the terms of the EUPL General Public License, v.1.1
// --
// -- ERTMSFormalSpec is distributed in the hope that it will be useful,
// -- but WITHOUT ANY WARRANTY; without even the implied warranty of
// -- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// --
// ------------------------------------------------------------------------------
using System.Threading;
namespace DataDictionary.Interpreter
{
    /// <summary>
    /// Compiles all expressions and statements located in the model & tests
    /// </summary>
    public class Compiler : Generated.Visitor
    {
        private class InitDeclaredElements : Generated.Visitor
        {
            /// <summary>
            /// Cleans up the declaraed elements dictionaries
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="visitSubNodes"></param>
            public override void visit(XmlBooster.IXmlBBase obj, bool visitSubNodes)
            {
                Utils.ISubDeclarator subDeclarator = obj as Utils.ISubDeclarator;
                if (subDeclarator != null)
                {
                    subDeclarator.InitDeclaredElements();
                }

                Utils.IFinder finder = obj as Utils.IFinder;
                if (finder != null)
                {
                    finder.ClearCache();
                }

                base.visit(obj, visitSubNodes);
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="system"></param>
            public InitDeclaredElements(EFSSystem system)
            {
                system.InitDeclaredElements();

                foreach (Dictionary dictionary in system.Dictionaries)
                {
                    visit(dictionary, true);
                }
            }
        }

        /// <summary>
        /// The EFS system that need to be compiled
        /// </summary>
        public EFSSystem System { get; set; }

        /// <summary>
        /// Indicates that the compilation should be performed
        /// </summary>
        public bool DoCompile { get; set; }

        /// <summary>
        /// The compilation options needed for the next compile
        /// </summary>
        private class CompilationOptions
        {
            /// <summary>
            /// Indicates that everything should be recompiled
            /// </summary>
            public bool Rebuild { get; set; }

            /// <summary>
            /// Indicates that compilation should be silent (or not)
            /// </summary>
            public bool SilentCompile { get; set; }

            /// <summary>
            /// Indicates that the compilation has been performed
            /// </summary>
            public bool CompilationDone { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="rebuild"></param>
            /// <param name="silent"></param>
            public CompilationOptions(bool rebuild, bool silent)
            {
                Rebuild = rebuild;
                SilentCompile = silent;
            }
        }

        /// <summary>
        /// The next compile session options
        /// </summary>
        private CompilationOptions NextCompile { get; set; }

        /// <summary>
        /// The current compilation session options
        /// </summary>
        private CompilationOptions CurrentCompile { get; set; }

        /// <summary>
        /// The compiler thread
        /// </summary>
        private Thread CompilerThread { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rebuild"></param>
        public Compiler(EFSSystem system)
        {
            System = system;

            DoCompile = true;
            CompilerThread = new Thread(CompileContinuously);
            CompilerThread.Start();
        }

        /// <summary>
        /// Perform continuous compilation
        /// </summary>
        /// <param name="obj"></param>
        private void CompileContinuously(object obj)
        {
            while (DoCompile)
            {
                CurrentCompile = NextCompile;
                if (CurrentCompile != null)
                {
                    NextCompile = null;
                    PerformCompile(CurrentCompile);
                    CurrentCompile.CompilationDone = true;
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Compiles or recompiles everything
        /// </summary>
        private void PerformCompile(CompilationOptions options)
        {
            try
            {
                ModelElement.BeSilent = options.SilentCompile;

                // Initialises the declared eleemnts
                InitDeclaredElements initDeclaredElements = new InitDeclaredElements(System);

                // Compiles each expression and each statement encountered in the nodes
                foreach (DataDictionary.Dictionary dictionary in System.Dictionaries)
                {
                    visit(dictionary, true);
                }
            }
            finally
            {
                ModelElement.BeSilent = false;
            }
        }

        /// <summary>
        /// Setups the NextCompile option
        /// </summary>
        /// <param name="rebuild"></param>
        /// <param name="silent"></param>
        /// <returns></returns>
        private CompilationOptions SetupCompilationOptions(bool rebuild, bool silent)
        {
            CompilationOptions options = NextCompile;

            if (options != null)
            {
                options.Rebuild = options.Rebuild || rebuild;
                options.SilentCompile = options.SilentCompile || silent;
            }
            else
            {
                options = new CompilationOptions(rebuild, silent);
                NextCompile = options;
            }

            return options;
        }

        /// <summary>
        /// Performs a synchronous compilation
        /// </summary>
        /// <param name="retbuild"></param>
        /// <param name="silent"></param>
        public void Compile_Synchronous(bool rebuild, bool silent = false)
        {
            if (DoCompile)
            {
                // Background compilation process is running
                CompilationOptions options = SetupCompilationOptions(rebuild, silent);
                while (!options.CompilationDone)
                {
                    Thread.Sleep(100);
                }
            }
            else
            {
                CurrentCompile = new CompilationOptions(rebuild, silent);
                PerformCompile(CurrentCompile);
            }
        }

        /// <summary>
        /// Performs an asynchronous compilation
        /// </summary>
        /// <param name="rebuild"></param>
        /// <param name="silent"></param>
        public void Compile_Asynchronous(bool rebuild, bool silent = false)
        {
            SetupCompilationOptions(rebuild, silent);
        }

        #region Compilation
        public override void visit(Generated.Action obj, bool visitSubNodes)
        {
            Rules.Action action = (Rules.Action)obj;

            if (CurrentCompile.Rebuild)
            {
                action.Statement = null;
            }

            // Side effect : compiles or recompiles the statement
            DataDictionary.Interpreter.Statement.Statement statement = action.Statement;

            base.visit(obj, visitSubNodes);
        }

        public override void visit(Generated.PreCondition obj, bool visitSubNodes)
        {
            Rules.PreCondition preCondition = (Rules.PreCondition)obj;

            if (CurrentCompile.Rebuild)
            {
                preCondition.ExpressionTree = null;
            }

            // Side effect : compiles or recompiles the expression
            DataDictionary.Interpreter.Expression expression = preCondition.ExpressionTree;

            base.visit(obj, visitSubNodes);
        }

        public override void visit(Generated.Expectation obj, bool visitSubNodes)
        {
            Tests.Expectation expectation = (Tests.Expectation)obj;

            if (CurrentCompile.Rebuild)
            {
                expectation.ExpressionTree = null;
                expectation.ConditionTree = null;
            }

            // Side effect : compiles or recompiles the expressions
            DataDictionary.Interpreter.Expression expression = expectation.ExpressionTree;
            DataDictionary.Interpreter.Expression condition = expectation.ConditionTree;

            base.visit(obj, visitSubNodes);
        }

        public override void visit(Generated.Frame obj, bool visitSubNodes)
        {
            Tests.Frame frame = (Tests.Frame)obj;

            if (CurrentCompile.Rebuild)
            {
                frame.CycleDuration = null;
            }

            // Side effect : compiles or recompiles the expression
            DataDictionary.Interpreter.Expression expression = frame.CycleDuration;


            base.visit(obj, visitSubNodes);
        }

        public override void visit(Generated.Namable obj, bool visitSubNodes)
        {
            Namable namable = (Namable)obj;

            namable.ClearFullName();

            base.visit(obj, visitSubNodes);
        }
        #endregion
    }
}
