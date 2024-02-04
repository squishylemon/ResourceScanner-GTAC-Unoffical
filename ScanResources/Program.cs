using System;
using System.IO;
using System.Xml;

namespace FolderScannerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the XML file
            string xmlFilePath = "server.xml";

            // Path to the folder to scan
            string folderPath = "resources";

            // Load the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            // Get the <resources> node
            XmlNode resourcesNode = xmlDoc.SelectSingleNode("/server/resources");

            int successfulCount = 0;
            int failedCount = 0;

            // Check if the resources node is found
            if (resourcesNode != null)
            {
                // Remove all existing resource elements
                resourcesNode.RemoveAll();

                // Get all directories in the specified folder
                string[] directories = Directory.GetDirectories(folderPath);

                // Iterate through each directory
                foreach (string directory in directories)
                {
                    // Get the name of the directory
                    string directoryName = new DirectoryInfo(directory).Name;

                    // Check if the directory name is enclosed in square brackets
                    if (!directoryName.StartsWith("[") || !directoryName.EndsWith("]"))
                    {
                        // Create a new XML element for the resource
                        XmlElement resourceElement = xmlDoc.CreateElement("resource");
                        resourceElement.SetAttribute("src", directoryName);

                        try
                        {
                            // Append the new element to the resources node
                            resourcesNode.AppendChild(resourceElement);
                            successfulCount++;
                            // Display success message in green
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Successfully scanned: {directoryName}");
                        }
                        catch (Exception ex)
                        {
                            failedCount++;
                            // Display error message in red if adding to XML fails
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Failed to scan {directoryName}: {ex.Message}");
                        }
                    }
                    else
                    {
                        // Skip folders enclosed in square brackets (assumed as group folders)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Skipping group folder: {directoryName}");
                    }
                }

                // Save the modified XML document
                xmlDoc.Save(xmlFilePath);

                // Display the total count of successful scans
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"All {successfulCount} resources successfully scanned.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                // Display an error if the <resources> node is not found in the XML file
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: <resources> node not found in the XML file.");
            }

            // Prompt to close the application
            Console.WriteLine("Press any key to close...");
            Console.ReadKey(); // Wait for any key press to close the console window
        }
    }
}
