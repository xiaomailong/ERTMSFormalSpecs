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
using System.Collections.Generic;
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
            catch (System.Exception)
            {
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
        public override void visit(Generated.BaseModelElement obj, bool visitSubNodes)
        {
            IExpressionable expressionnable = obj as IExpressionable;
            if (expressionnable != null)
            {
                // In case of rebuild, cleans the previously constructed tree
                if (CurrentCompile.Rebuild)
                {
                    expressionnable.CleanCompilation();
                }

                // Ensures that the expressionable is compiled
                expressionnable.Compile();
            }

            Types.ITypedElement typedElement = obj as Types.ITypedElement;
            if (typedElement != null)
            {
                // Ensures that the type of the corresponding element is cached
                Types.Type type = typedElement.Type;
            }

            base.visit(obj, visitSubNodes);
        }

        public override void visit(Generated.Namable obj, bool visitSubNodes)
        {
            Namable namable = (Namable)obj;

            namable.ClearFullName();

            base.visit(obj, visitSubNodes);
        }
        #endregion

        #region Refactoring
        /// <summary>
        /// Cleans the caches of the full names
        /// </summary>
        private class FullNameCleaner : Generated.Visitor
        {
            public override void visit(Generated.Namable obj, bool visitSubNodes)
            {
                Namable namable = (Namable)obj;

                namable.ClearFullName();

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        /// Refactors an element which can hold an expression
        /// </summary>
        /// <param name="element">The element that has been modified, and for which refactoring is done</param>
        /// <param name="user">The user, which can hold an expression</param>
        private static void RefactorIExpressionable(ModelElement element, IExpressionable user)
        {
            if (user != null)
            {
                try
                {
                    string name = element.ReferenceName(user as ModelElement);

                    Refactor.RefactorTree refactorer = new Refactor.RefactorTree(element, name);
                    refactorer.PerformUpdate(user);
                }
                catch (System.Exception e)
                {
                    ((ModelElement)user).AddError("Cannot refactor this element, reason = " + e.Message);
                }
            }
        }

        /// <summary>
        /// Refactors an element which has a type
        /// </summary>
        /// <param name="element">The element that has been modified</param>
        /// <param name="user">The user which references this type</param>
        private static void RefactorTypedElement(ModelElement element, Types.ITypedElement user)
        {
            if (user != null)
            {
                try
                {
                    if ((user.Type == element))
                    {
                        user.TypeName = element.ReferenceName(user as ModelElement);
                    }
                    else if (element is Types.NameSpace)
                    {
                        Functions.Function function = user as Functions.Function;
                        if (function != null)
                        {
                            user.TypeName = function.ReturnType.ReferenceName(element);
                        }
                        else
                        {
                            user.TypeName = user.Type.ReferenceName(user as ModelElement);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    ((ModelElement)user).AddError("Cannot refactor this element, reason = " + e.Message);
                }
            }
        }

        /// <summary>
        /// Replaces all occurences of namespaces in the system
        /// </summary>
        private class NameSpaceRefactorer : Generated.Visitor
        {
            /// <summary>
            /// The namespace that has been modified, and for which the process is launched
            /// </summary>
            private Types.NameSpace NameSpace { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="dictionary"></param>
            public NameSpaceRefactorer(Types.NameSpace nameSpace)
            {
                NameSpace = nameSpace;
            }

            public override void visit(Generated.BaseModelElement obj, bool visitSubNodes)
            {
                RefactorIExpressionable(NameSpace, obj as IExpressionable);
                RefactorTypedElement(NameSpace, obj as Types.ITypedElement);

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        /// Modifies the system according to the new element definition
        /// </summary>
        /// <param name="element">The element that has been modified, and for which refactoring is done</param>
        public void Refactor(ModelElement element)
        {
            if (element != null)
            {
                // Cleans fullname cache
                FullNameCleaner cleaner = new FullNameCleaner();
                foreach (DataDictionary.Dictionary dictionary in EFSSystem.INSTANCE.Dictionaries)
                {
                    cleaner.visit(dictionary);
                }

                Types.NameSpace nameSpace = element as Types.NameSpace;
                if (nameSpace != null)
                {
                    NameSpaceRefactorer refactorer = new NameSpaceRefactorer(nameSpace);
                    foreach (DataDictionary.Dictionary dictionary in EFSSystem.INSTANCE.Dictionaries)
                    {
                        refactorer.visit(dictionary);
                    }
                }
                else
                {
                    // the system keeps track where the element is used
                    List<Usage> usages = element.EFSSystem.FindReferences(element);
                    foreach (Usage usage in usages)
                    {
                        RefactorIExpressionable(element, usage.User as IExpressionable);
                        RefactorTypedElement(element, usage.User as Types.ITypedElement);
                    }
                }
            }
        }

        private class ReNameSpacerVisitor : Generated.Visitor
        {
            /// <summary>
            /// The new location of the element
            /// </summary>
            private Types.NameSpace NewNameSpace { get; set; }

            public override void visit(Generated.BaseModelElement obj, bool visitSubNodes)
            {
                IExpressionable expressionable = obj as IExpressionable;
                if (expressionable != null)
                {
                    Interpreter.Refactor.ReNameSpaceTree refactorer = new Refactor.ReNameSpaceTree(NewNameSpace);
                    refactorer.PerformUpdate(expressionable);
                }

                base.visit(obj, visitSubNodes);
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="newNameSpace"></param>
            public ReNameSpacerVisitor(Types.NameSpace newNameSpace)
            {
                NewNameSpace = newNameSpace;
            }
        }

        /// <summary>
        /// Performs a refactoring of the model then ensure that the namespaces in its inner elements are correct
        /// </summary>
        /// <param name="model"></param>
        public void RefactorAndReNameSpace(ModelElement model)
        {
            if (model != null)
            {
                Refactor(model);

                Types.NameSpace newNameSpace = Utils.EnclosingFinder<Types.NameSpace>.find(model, true);
                ReNameSpacerVisitor renamer = new ReNameSpacerVisitor(newNameSpace);
                renamer.visit(model);
            }
        }
        #endregion
    }
}
