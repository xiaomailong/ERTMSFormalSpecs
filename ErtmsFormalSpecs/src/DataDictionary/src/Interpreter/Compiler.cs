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
using DataDictionary.Generated;
using Utils;
using Function = DataDictionary.Functions.Function;
using NameSpace = DataDictionary.Types.NameSpace;
using StructureElement = DataDictionary.Types.StructureElement;
using Variable = DataDictionary.Variables.Variable;

namespace DataDictionary.Interpreter
{
    /// <summary>
    /// Compiles all expressions and statements located in the model & tests
    /// </summary>
    public class Compiler : Generated.Visitor
    {
        /// <summary>
        /// Performs a clean before compiling
        /// </summary>
        private class CleanBeforeCompilation : Generated.Visitor
        {
            /// <summary>
            /// The compilation options
            /// </summary>
            private CompilationOptions Options { get; set; }

            /// <summary>
            /// Cleans up the declared elements dictionaries
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
            /// Clears the cache dependancy when a full rebuilt is asked
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="visitSubNodes"></param>
            public override void visit(BaseModelElement obj, bool visitSubNodes)
            {
                if (Options.Rebuild)
                {
                    obj.CacheDependancy = null;
                }

                base.visit(obj, visitSubNodes);
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="options"></param>
            /// <param name="system"></param>
            public CleanBeforeCompilation(CompilationOptions options, EFSSystem system)
            {
                Options = options;
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
        public EFSSystem EFSSystem { get; set; }

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
            EFSSystem = system;

            DoCompile = true;
            CompilerThread = new Thread(CompileContinuously);
            CompilerThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
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
        /// Creates the function dependancies
        /// </summary>
        private class CreateDependancy : Generated.Visitor
        {
            /// <summary>
            /// Visits the expressions used in functions to create the dependancy graph for that functions
            /// </summary>
            private class ReferenceVisitor : Visitor
            {
                /// <summary>
                /// The function for which the dependancy graph is being built
                /// </summary>
                private Function DependantFunction { get; set; }

                /// <summary>
                /// Indicates that a change in the dependancy graph has been performed
                /// </summary>
                public bool DependancyChange { get; set; }

                /// <summary>
                /// Constructor
                /// </summary>
                public ReferenceVisitor()
                {
                }

                /// <summary>
                /// Updates the dependancy graph according to this expression tree
                /// </summary>
                /// <param name="dependantFunction"/>
                /// <param name="tree"></param>
                public void UpdateReferences(Function dependantFunction, InterpreterTreeNode tree)
                {
                    DependantFunction = dependantFunction;

                    visitInterpreterTreeNode(tree);
                }

                protected override void VisitDesignator(Designator designator)
                {
                    base.VisitDesignator(designator);

                    Utils.ModelElement current = designator.Ref as Utils.ModelElement;
                    while (current != null && !(current is NameSpace) && !(current is Parameter))
                    {
                        bool change = false;

                        Variable variable = current as Variable;
                        if (variable != null)
                        {
                            change = variable.AddDependancy(DependantFunction);
                            DependancyChange = DependancyChange || change;
                        }
                        else
                        {
                            StructureElement structureElement = current as StructureElement;
                            if (structureElement != null)
                            {
                                change = structureElement.AddDependancy(DependantFunction);
                                DependancyChange = DependancyChange || change;

                                change = structureElement.Structure.AddDependancy(DependantFunction);
                                DependancyChange = DependancyChange || change;
                            }
                            else
                            {
                                Function function = current as Function;
                                if (function != null)
                                {
                                    change = function.AddDependancy(DependantFunction);
                                    DependancyChange = DependancyChange || change;
                                }
                            }
                        }

                        current = current.Enclosing as Utils.ModelElement;
                    }
                }
            }
            

            /// <summary>
            /// The reference visitor
            /// </summary>
            private ReferenceVisitor TheReferenceVisitor { get; set; }

            /// <summary>
            /// Indicates that a change in the dependancy graph has been performed
            /// </summary>
            public bool DependancyChange
            {
                get { return TheReferenceVisitor.DependancyChange; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="system"></param>
            public CreateDependancy(EFSSystem system)
            {
                TheReferenceVisitor = new ReferenceVisitor();

                foreach (DataDictionary.Dictionary dictionary in system.Dictionaries)
                {
                    visit(dictionary, true);
                }
            }

            public override void visit(Generated.BaseModelElement obj, bool visitSubNodes)
            {
                IExpressionable expressionnable = obj as IExpressionable;
                if (expressionnable != null)
                {
                    Functions.Function enclosingFunction = EnclosingFinder<Functions.Function>.find(obj, true);
                    if (enclosingFunction != null)
                    {
                        // The value of the function depends on this. 
                        TheReferenceVisitor.UpdateReferences(enclosingFunction, expressionnable.Tree);
                    }
                }

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        /// PropagatesDependancy the dependancy relationship between elements
        /// </summary>
        private class FlattenDependancy : Generated.Visitor
        {
            /// <summary>
            /// The elements that have already been browsed
            /// </summary>
            private HashSet<Utils.ModelElement> BrowsedElements { get; set; } 

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="system"></param>
            public FlattenDependancy(EFSSystem system)
            {
                foreach (DataDictionary.Dictionary dictionary in system.Dictionaries)
                {
                    foreach (NameSpace nameSpace in dictionary.NameSpaces)
                    {
                        visit(nameSpace, true);
                    }
                }
            }

            /// <summary>
            /// PropagatesDependancy the elementToAdd to the dependancies of the elementToBrowse
            /// </summary>
            /// <param name="elementToBrowse"></param>
            /// <param name="elementToAdd"></param>
            private void PropagatesDependancy(Utils.ModelElement elementToBrowse, Utils.ModelElement elementToAdd)
            {
                if (!BrowsedElements.Contains(elementToBrowse))
                {
                    BrowsedElements.Add(elementToBrowse);

                    elementToAdd.AddDependancy(elementToBrowse);
                    if (elementToBrowse.CacheDependancy != null)
                    {
                        foreach (Utils.ModelElement depending in elementToBrowse.CacheDependancy)
                        {
                            object current = depending;
                            while (current != null)
                            {
                                Utils.ModelElement currentElement = current as Utils.ModelElement;
                                if (currentElement != null)
                                {
                                    PropagatesDependancy(currentElement, elementToAdd);
                                }

                                IEnclosed enclosed = current as IEnclosed;
                                if (enclosed != null)
                                {
                                    current = enclosed.Enclosing;
                                }
                                else
                                {
                                    current = null;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// PropagatesDependancy all elements
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="visitSubNodes"></param>
            public override void visit(BaseModelElement obj, bool visitSubNodes)
            {
                if (obj.CacheDependancy != null)
                {
                    BrowsedElements = new HashSet<Utils.ModelElement>();
                    BrowsedElements.Add(obj);

                    HashSet<Utils.ModelElement> dependingElements = obj.CacheDependancy;
                    obj.CacheDependancy = null;
                    foreach (Utils.ModelElement depending in dependingElements)
                    {
                        object current = depending;
                        while (current != null)
                        {
                            Utils.ModelElement currentElement = current as Utils.ModelElement;
                            if (currentElement != null)
                            {
                                PropagatesDependancy(currentElement, obj);
                            }

                            IEnclosed enclosed = current as IEnclosed;
                            if (enclosed != null)
                            {
                                current = enclosed.Enclosing;
                            }
                            else
                            {
                                current = null;
                            }
                        }
                    }
                }

                base.visit(obj, visitSubNodes);
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

                // Initialises the declared elements
                CleanBeforeCompilation cleanBeforeCompilation = new CleanBeforeCompilation(options, EFSSystem);

                // Compiles each expression and each statement encountered in the nodes
                foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
                {
                    visit(dictionary, true);
                }

                if (options.Rebuild)
                {
                    CreateDependancy createDependancy = new CreateDependancy(EFSSystem);
                    if (createDependancy.DependancyChange)
                    {
                        FlattenDependancy flattenDependancy = new FlattenDependancy(EFSSystem);
                    }
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
        /// <param name="rebuild"></param>
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
                    Functions.Function userFunction = user as Functions.Function;
                    if ((user.Type == element))
                    {
                        string newName = element.ReferenceName(user as ModelElement);
                        user.TypeName = newName;
                    }
                    else if (userFunction != null && userFunction.ReturnType == element)
                    {
                        userFunction.ReturnType = element as Types.Type;
                    }
                    else if (element is Types.NameSpace)
                    {
                        string newName;
                        Functions.Function function = user as Functions.Function;
                        if (function != null)
                        {
                            newName = function.ReturnType.ReferenceName(element);
                            user.TypeName = newName;
                        }
                        else
                        {
                            newName = user.Type.ReferenceName(user as ModelElement);
                            user.TypeName = newName;
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

        private class RelocateVisitor : Generated.Visitor
        {
            /// <summary>
            /// The new location of the element
            /// </summary>
            private ModelElement BaseLocation { get; set; }

            public override void visit(Generated.BaseModelElement obj, bool visitSubNodes)
            {
                IExpressionable expressionable = obj as IExpressionable;
                if (expressionable != null)
                {
                    Interpreter.Refactor.RelocateTree refactorer = new Refactor.RelocateTree(BaseLocation);
                    refactorer.PerformUpdate(expressionable);
                }

                base.visit(obj, visitSubNodes);
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="newNameSpace"></param>
            public RelocateVisitor(ModelElement modelElement)
            {
                ModelElement current = modelElement;

                while (current != null && !(current is Types.Type) && !(current is Types.NameSpace))
                {
                    current = current.Enclosing as ModelElement;
                }

                BaseLocation = current;
            }
        }

        /// <summary>
        /// Performs a refactoring of the model then ensure that the namespaces in its inner elements are correct
        /// </summary>
        /// <param name="model"></param>
        public void RefactorAndRelocate(ModelElement model)
        {
            if (model != null)
            {
                Refactor(model);

                RelocateVisitor relocator = new RelocateVisitor(model);
                relocator.visit(model);
            }
        }
        #endregion
    }
}
