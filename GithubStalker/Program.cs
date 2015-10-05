using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;


namespace GithubStalker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your UserName: ");
            string SearchUserName = Console.ReadLine();

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                string json = webClient.DownloadString("https://api.github.com/users/" + SearchUserName);


                Console.WriteLine(json);
                Console.ReadLine();
            }

        }
    }
}
