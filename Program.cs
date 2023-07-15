using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace windows_icons8_downloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inspect the desired icon, copy the outer html, and paste it here: ");
            string outerHtml = Console.ReadLine();

            string iconId = outerHtml.Substring(outerHtml.IndexOf("id=") + 3);
            iconId = iconId.Substring(0, iconId.IndexOf("&amp;"));

            string name = outerHtml.Substring(outerHtml.IndexOf("alt=\"") + 5);
            name = name.Substring(0, name.IndexOf("\""));

            Console.WriteLine("What's the desired SVG image size, in pixels?");
            string size = Console.ReadLine().ToLower().Replace("px", "").Trim();

            string format = "svg";

            string url = "https://img.icons8.com/?size=" + size + "&id=" + iconId + "&format=svg";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Downloads\\" + name + "." + format;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send a GET request to the URL synchronously
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a stream and save it to a file
                        using (Stream contentStream = response.Content.ReadAsStreamAsync().Result)
                        {
                            using (FileStream fileStream = File.Create(filePath))
                            {
                                contentStream.CopyTo(fileStream);
                            }
                        }

                        Console.WriteLine("File downloaded successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            Process.Start(Path.GetDirectoryName(filePath));
        }
    }
}
