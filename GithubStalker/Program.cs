using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using GithubStalker.Classes;

namespace GithubStalker
{
    class Program
    {
        //Main Properties
        public static UserInformation UserInfo;
        public static string Line = "----------------------------------- \n \n ";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter your UserName: ");
            string SearchUserName = Console.ReadLine();

            using (WebClient webClient = new WebClient())
            {

                //Say hello to GitHub
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");


                //Grab JSON file from Github
                string json = webClient.DownloadString("https://api.github.com/users/" + SearchUserName);


                //Convert JSON to user-able variables
                UserInfo = JsonConvert.DeserializeObject<UserInformation>(json);

                //Grab UserInformation from JSON Github (user info) 
                //
                //Declare our properties to grab from JSON object
                string myLogin = UserInfo.login;
                string myName = UserInfo.name;
                string myUrl = UserInfo.URL;
                string RepoUrl = UserInfo.Repositories_URL;
                string myFollowers = UserInfo.Followers;
                string myFollowing = UserInfo.Following;
                string myRepoCount = UserInfo.public_repos;

                Console.WriteLine(Line);
                Console.WriteLine("Name : {0}", myName);
                Console.WriteLine("Your username: {0}", myLogin);
                Console.WriteLine("URL: {0}",myUrl);
                Console.WriteLine("Followers: {0}", myFollowers);
                Console.WriteLine("Following: {0}", myFollowing);
                Console.WriteLine("Your number of Repositories: {0}", myRepoCount);


                //Output to Console
                Console.ReadLine();
            }

        }
    }
}
