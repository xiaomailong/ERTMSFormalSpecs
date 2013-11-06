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
using System;
using System.Collections.Generic;
using System.IO;
using DataDictionary.Functions;
using DataDictionary.Specification;
using DataDictionary.Tests;
using DataDictionary.Tests.Translations;
using DataDictionary.Types;
using DataDictionary.Variables;
using XmlBooster;
using Utils;

namespace DataDictionary
{
    public class Util
    {
        /// <summary>
        /// The Logger
        /// </summary>
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Indicates that the files should be locked for edition when opened
        /// </summary>
        public static bool PleaseLockFiles = true;

        /// <summary>
        /// Updates the dictionary contents
        /// </summary>
        private class Updater : Generated.Visitor
        {
        }

        /// <summary>
        /// Updates the dictionary contents
        /// </summary>
        private class LoadDepends : Generated.Visitor
        {
            /// <summary>
            /// The base path used to load files
            /// </summary>
            public string BasePath { get; private set; }

            /// <summary>
            /// Indicates that the files should be locked
            /// </summary>
            public bool LockFiles { get; private set; }

            /// <summary>
            /// Indicates that errors can occur during load, for instance, for comparison purposes
            /// </summary>
            public bool AllowErrorsDuringLoad { get { return ErrorsDuringLoad != null; } }

            /// <summary>
            /// The errors that occured during the load of the file
            /// </summary>
            public List<ElementLog> ErrorsDuringLoad { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="basePath"></param>
            /// <param name="lockFiles"></param>
            /// <param name="allowErrors"></param>
            public LoadDepends(string basePath, bool lockFiles, List<ElementLog> errors)
            {
                BasePath = basePath;
                LockFiles = lockFiles;
                ErrorsDuringLoad = errors;
            }

            public override void visit(Generated.Dictionary obj, bool visitSubNodes)
            {
                Dictionary dictionary = (Dictionary)obj;

                if (dictionary.allNameSpaceRefs() != null)
                {
                    foreach (NameSpaceRef nameSpaceRef in dictionary.allNameSpaceRefs())
                    {
                        NameSpace nameSpace = nameSpaceRef.LoadNameSpace(LockFiles, AllowErrorsDuringLoad);
                        if (nameSpace != null)
                        {
                            dictionary.appendNameSpaces(nameSpace);
                        }
                        else
                        {
                            ErrorsDuringLoad.Add(new ElementLog(ElementLog.LevelEnum.Error, "Cannot load file " + nameSpaceRef.FileName));
                        }
                    }
                    dictionary.allNameSpaceRefs().Clear();
                }
                if (dictionary.allTestRefs() != null)
                {
                    foreach (FrameRef testRef in dictionary.allTestRefs())
                    {
                        Frame frame = testRef.LoadFrame(LockFiles, AllowErrorsDuringLoad);
                        if (frame != null)
                        {
                            dictionary.appendTests(frame);
                        }
                        else
                        {
                            ErrorsDuringLoad.Add(new ElementLog(ElementLog.LevelEnum.Error, "Cannot load file " + testRef.FileName));
                        }
                    }
                    dictionary.allTestRefs().Clear();
                }

                base.visit(obj, visitSubNodes);
            }

            public override void visit(Generated.NameSpace obj, bool visitSubNodes)
            {
                NameSpace nameSpace = (NameSpace)obj;

                if (nameSpace.allNameSpaceRefs() != null)
                {
                    foreach (NameSpaceRef nameSpaceRef in nameSpace.allNameSpaceRefs())
                    {
                        NameSpace subNameSpace = nameSpaceRef.LoadNameSpace(LockFiles, AllowErrorsDuringLoad);
                        if (subNameSpace != null)
                        {
                            nameSpace.appendNameSpaces(subNameSpace);
                        }
                        else
                        {
                            ErrorsDuringLoad.Add(new ElementLog(ElementLog.LevelEnum.Error, "Cannot load file " + nameSpaceRef.FileName));
                        }

                    }
                    nameSpace.allNameSpaceRefs().Clear();
                }

                base.visit(obj, visitSubNodes);
            }

            public override void visit(Generated.Specification obj, bool visitSubNodes)
            {
                Specification.Specification specification = (Specification.Specification)obj;

                if (specification.allChapterRefs() != null)
                {
                    foreach (ChapterRef chapterRef in specification.allChapterRefs())
                    {
                        Chapter chapter = chapterRef.LoadChapter(LockFiles, AllowErrorsDuringLoad);
                        if (chapter != null)
                        {
                            specification.appendChapters(chapter);
                        }
                        else
                        {
                            ErrorsDuringLoad.Add(new ElementLog(ElementLog.LevelEnum.Error, "Cannot load file " + chapterRef.FileName));
                        }
                    }
                    specification.allChapterRefs().Clear();
                }

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        /// Holds information about opened files in the system
        /// </summary>
        private class FileData
        {
            /// <summary>
            /// The name of the corresponding file
            /// </summary>
            public String FileName { get; private set; }

            /// <summary>
            /// The stream used to lock the file
            /// </summary>
            public FileStream Stream { get; private set; }

            /// <summary>
            /// The length of the lock section
            /// </summary>
            private long LockLength { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="fileName"></param>
            public FileData(String fileName)
            {
                FileName = fileName;
                Lock();
            }

            /// <summary>
            /// Locks the corresponding file
            /// </summary>
            public void Lock()
            {
                if (Stream == null && PleaseLockFiles)
                {
                    Stream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    LockLength = Stream.Length;
                    Stream.Lock(0, LockLength);
                }
            }

            /// <summary>
            /// Unlocks the corresponding file
            /// </summary>
            public void Unlock()
            {
                if (Stream != null && PleaseLockFiles)
                {
                    Stream.Unlock(0, LockLength);
                    Stream.Close();
                    Stream = null;
                }
            }
        }

        /// <summary>
        /// Lock all opened files
        /// </summary>
        private static List<FileData> openedFiles = new List<FileData>();

        /// <summary>
        /// Locks a single file
        /// </summary>
        /// <param name="filePath"></param>
        private static void LockFile(String filePath)
        {
            FileData data = new FileData(filePath);
            openedFiles.Add(data);
        }

        /// <summary>
        /// Unlocks all files locked by the system
        /// </summary>
        public static void UnlockAllFiles()
        {
            foreach (FileData data in openedFiles)
            {
                data.Unlock();
            }
        }

        /// <summary>
        /// Locks all files loaded in the system
        /// </summary>
        public static void LockAllFiles()
        {
            foreach (FileData data in openedFiles)
            {
                data.Lock();
            }
        }

        /// <summary>
        /// Loads a document and handles its associated locks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class DocumentLoader<T>
            where T : class, IXmlBBase
        {
            /// <summary>
            /// Loads a file and locks it if required
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="enclosing"></param>
            /// <param name="lockFiles"></param>
            /// <returns></returns>
            public static T loadFile(string filePath, ModelElement enclosing, bool lockFiles)
            {
                T retVal = null;

                // Do not rely on XmlBFileContext since it does not care about encoding. 
                // File encoding is UTF-8
                StreamReader file = new StreamReader(filePath);
                XmlBStringContext ctxt = new XmlBStringContext(file.ReadToEnd());
                try
                {
                    Generated.ControllersManager.DesactivateAllNotifications();
                    retVal = Generated.acceptor.accept(ctxt) as T;
                    if (retVal != null)
                    {
                        retVal.setFather(enclosing);
                        if (lockFiles)
                        {
                            LockFile(filePath);
                        }
                    }
                }
                catch (XmlBooster.XmlBException excp)
                {
                    Log.Error(ctxt.errorMessage());
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
                finally
                {
                    Generated.ControllersManager.ActivateAllNotifications();
                }

                return retVal;
            }
        }

        /// <summary>
        /// Loads a dictionary and lock the file
        /// </summary>
        /// <param name="filePath">The path of the file which holds the dictionary data</param>
        /// <param name="efsSystem">The system for which this dictionary is loaded</param>
        /// <param name="lockFiles">Indicates that the files should be locked</param>
        /// <param name="allowErrors">Provides the list used to hold errors during the load. null indicates that no errors are tolerated during load</param>
        /// <returns></returns>
        public static Dictionary load(String filePath, EFSSystem efsSystem, bool lockFiles, List<ElementLog> errors)
        {
            Dictionary retVal = DocumentLoader<Dictionary>.loadFile(filePath, null, lockFiles);

            if (retVal != null)
            {
                retVal.FilePath = filePath;
                if (efsSystem != null)
                {
                    efsSystem.AddDictionary(retVal);
                }

                // Loads the dependancies for this .efs file
                try
                {
                    Generated.ControllersManager.DesactivateAllNotifications();
                    LoadDepends loadDepends = new LoadDepends(retVal.BasePath, lockFiles, errors);
                    loadDepends.visit(retVal);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    retVal = null;
                }
                finally
                {
                    Generated.ControllersManager.ActivateAllNotifications();
                }
            }

            if (retVal != null)
            {
                // Updates the contents of this .efs file
                try
                {
                    Generated.ControllersManager.DesactivateAllNotifications();
                    Updater updater = new Updater();
                    updater.visit(retVal);
                    if (retVal.Specifications != null)
                    {
                        retVal.Specifications.ManageTypeSpecs();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
                finally
                {
                    Generated.ControllersManager.ActivateAllNotifications();
                }

                if (retVal != null && efsSystem != null)
                {
                    retVal.CheckRules();
                }
            }

            return retVal;
        }

        /// <summary>
        /// Loads a specification and lock the file
        /// </summary>
        /// <param name="filePath">The name of the file which holds the dictionary data</param>
        /// <param name="dictionary">The dictionary for which the specification is loaded</param>
        /// <param name="lockFiles">Indicates that the files should be locked</param>
        /// <returns></returns>
        public static Specification.Specification loadSpecification(String filePath, Dictionary dictionary, bool lockFiles)
        {
            Specification.Specification retVal = DocumentLoader<Specification.Specification>.loadFile(filePath, dictionary, lockFiles);

            if (retVal == null)
            {
                throw new System.Exception("Cannot read file " + filePath);
            }

            return retVal;
        }

        /// <summary>
        /// Loads a translation dictionary and lock the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dictionary"></param>
        /// <param name="lockFiles">Indicates that the files should be locked</param>
        /// <returns></returns>
        public static TranslationDictionary loadTranslationDictionary(string filePath, DataDictionary.Dictionary dictionary, bool lockFiles)
        {
            TranslationDictionary retVal = DocumentLoader<TranslationDictionary>.loadFile(filePath, dictionary, lockFiles);

            if (retVal == null)
            {
                throw new System.Exception("Cannot read file " + filePath);
            }

            return retVal;
        }

        /// <summary>
        /// Loads a namespace and locks the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="enclosing"></param>
        /// <param name="lockFiles"></param>
        /// <param name="allowErrors"></param>
        /// <returns></returns>
        public static NameSpace loadNameSpace(string filePath, ModelElement enclosing, bool lockFiles, bool allowErrors)
        {
            NameSpace retVal = DocumentLoader<NameSpace>.loadFile(filePath, enclosing, lockFiles);

            if (retVal == null)
            {
                if (!allowErrors)
                {
                    throw new System.Exception("Cannot read file " + filePath);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Loads a frame and locks the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="enclosing"></param>
        /// <param name="lockFiles"></param>
        /// <param name="allowErrors"></param>
        /// <returns></returns>
        public static Frame loadFrame(string filePath, ModelElement enclosing, bool lockFiles, bool allowErrors)
        {
            Frame retVal = DocumentLoader<Frame>.loadFile(filePath, enclosing, lockFiles);

            if (retVal == null)
            {
                if (!allowErrors)
                {
                    throw new System.Exception("Cannot read file " + filePath);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Loads a chapter and locks the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dictionary"></param>
        /// <param name="lockFiles">Indicates that the files should be locked</param>
        /// <param name="allowErrors"></param>
        /// <returns></returns>
        public static Chapter loadChapter(string filePath, ModelElement enclosing, bool lockFiles, bool allowErrors)
        {
            Chapter retVal = DocumentLoader<Chapter>.loadFile(filePath, enclosing, lockFiles);

            if (retVal == null)
            {
                if (!allowErrors)
                {
                    throw new System.Exception("Cannot read file " + filePath);
                }
            }

            return retVal;
        }
    }
}
