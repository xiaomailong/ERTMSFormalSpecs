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
namespace Utils
{
    using System;
    using System.IO;

    /// <summary>
    /// The class versioned writer overwrites a new file when the preceding contents is different from the original one
    /// </summary>
    public class VersionedWriter : StreamWriter
    {
        /// <summary>
        /// The file currently being created
        /// </summary>
        private String TargetPath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        public VersionedWriter(String path)
            : base(CreateTempStream(path))
        {
            TargetPath = path;
        }

        /// <summary>
        /// When the file is created, compare its contents with the original one. 
        /// Overwrite the target file if the contents differ
        /// </summary>
        public override void Close()
        {
            String tempFilePath = ((FileStream)BaseStream).Name;
            base.Close();

            if (!SameFiles(tempFilePath))
            {
                ReplaceContents(tempFilePath);
            }

            File.Delete(tempFilePath);
        }

        /// <summary>
        /// Checks if the file contents are the same
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        private bool SameFiles(string tempFilePath)
        {
            bool retVal = true;

            try
            {
                StreamReader original = new StreamReader(TargetPath);
                StreamReader newFile = new StreamReader(tempFilePath);
                while (retVal && !original.EndOfStream && !newFile.EndOfStream)
                {
                    string originalLine = original.ReadLine().Replace("\r", "");
                    string newLine = newFile.ReadLine().Replace("\r", "");

                    retVal = originalLine.Equals(newLine);
                }
                retVal = retVal && (original.EndOfStream == newFile.EndOfStream);

                original.Close();
                newFile.Close();
            }
            catch (IOException)
            {
                retVal = false;
            }

            return retVal;
        }

        /// <summary>
        /// Replaces the contents of the original file with the newly created file
        /// </summary>
        /// <param name="tempFilePath"></param>
        private void ReplaceContents(string tempFilePath)
        {
            // Replace the original file with the new file
            StreamReader newFile = new StreamReader(tempFilePath);
            StreamWriter original = new StreamWriter(TargetPath);
            while (!newFile.EndOfStream)
            {
                String newLine = newFile.ReadLine();
                original.WriteLine(newLine);
            }
            original.Close();
            newFile.Close();
        }

        /// <summary>
        /// Creates a new temporary FileStream 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private static FileStream CreateTempStream(string original)
        {
            string extension = Path.GetExtension(original);
            string basename = Path.GetFileNameWithoutExtension(original);
            string fileName = Path.Combine(Path.GetTempPath(), basename + Guid.NewGuid().ToString() + extension);
            return new FileStream(fileName, FileMode.Create);
        }
    }
}
