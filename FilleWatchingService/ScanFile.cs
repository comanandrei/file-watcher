using System;
using System.IO;

namespace FilleWatchingService
{
    class ScanFile
    {
        private const string IsCloned = "isCloned";

        public ScanFile( string filePath)
        {

            var movingPath = prepareFileToMove(filePath);

            //if the file si cloned should not be moved, must be erased (because the file is already in the transition location)
            if (movingPath == IsCloned)
            {
                deleteFile(filePath);
            }
            else
            {
                moveFile(filePath, movingPath);
            }

        }

        //Check if the file must be moved and create the new location of the file.
        //Return the transition path
        private string prepareFileToMove(string filePath)
        {
            var entityFile = new FileInfo(filePath);

            //extension without dot "."
            string extensionFile = Util.GetSimpleExtension(entityFile.Extension);

            //transition folder
            string transitionPath = Util.GetTransitionPath();
            //Transition location found
            if (!String.IsNullOrEmpty(transitionPath))
            {
                //the extension folder (from transition folder)
                string newPath = transitionPath + Path.DirectorySeparatorChar + extensionFile;

                //The directory doesn't exists
                if (!Directory.Exists(newPath))
                {
                    // Try to create the directory.
                    try
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The directory couldn't been created : {0}", e.ToString());
                    }
                }
                string FileName = setFileName(entityFile, newPath);

                //if the file si cloned it does not have to be moved
                //there is no need for transaction location
                if (FileName == IsCloned) return IsCloned;

                //return the transition location
                return newPath + Path.DirectorySeparatorChar + FileName + entityFile.Extension;
            }
            else
            {
                //todo:  test if is works
                throw new ApplicationException("The transition path is not found.");
            }

        }

        //Verify if is another file in the transition folder with the same name
        private string setFileName(FileInfo entityFile, string newFullPath)
        {
            int count = 1;

            string tempFileName = Path.GetFileNameWithoutExtension(entityFile.Name);

            string baseNewFullPath = newFullPath;
            newFullPath  = newFullPath+ Path.DirectorySeparatorChar + tempFileName;
            Boolean isClone = false;
            //while there is a file with the same name
            while (File.Exists(newFullPath + entityFile.Extension))
            {
                //verify if the files are identical
                isClone = filesCompare(entityFile.FullName, newFullPath + entityFile.Extension);

                //if the files are identical, exit from loop
                if (isClone) break;

                //change the file name scanned
                tempFileName = string.Format("{0}({1})", tempFileName, count++);
                newFullPath = Path.Combine(newFullPath, tempFileName);
            }

            if (isClone) return IsCloned;

            return tempFileName;
        }

        //Verify if scaned file has a doppelganger in transition
        //Use the md5 hashing to compare the files content 
        private Boolean filesCompare(string filepath1, string filepath2)
        {
            using (var reader1 = new System.IO.FileStream(filepath1, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var reader2 = new System.IO.FileStream(filepath2, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] hash1;
                    byte[] hash2;

                    using (var md51 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        md51.ComputeHash(reader1);
                        hash1 = md51.Hash;
                    }

                    using (var md52 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        md52.ComputeHash(reader2);
                        hash2 = md52.Hash;
                    }

                    int j = 0;
                    for (j = 0; j < hash1.Length; j++)
                    {
                        if (hash1[j] != hash2[j])
                        {
                            break;
                        }
                    }

                    //if both hashed contents are identical
                    if (j == hash1.Length)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }

        //Move the file
        private void moveFile(string filePath, string movingPath)
        {
            try
            {
                //To move a file or folder to a new location:
                System.IO.File.Move(filePath, movingPath);
            }
            catch (Exception)
            {
                throw new FileNotFoundException("The file couldn't been moved!");
            }
        }

        //Delete the file
        private void deleteFile(string filePath)
        {
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (Exception)
            {
                throw new FileNotFoundException("The file couldn't been deleted!");
            }
        }

    }
}
