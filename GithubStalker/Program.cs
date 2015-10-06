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
        public static RepositoryInformation RepoInfo;
        public static string SearchParameter;

        //User Basic Information Properties (First Segment)
        public static string myLogin;
        public static string myName;
        public static string myUrl;
        public static string RepoUrl;
        public static string myFollowers;
        public static string myFollowing;
        public static string myRepoCount;

        

        static void Main(string[] args)
        {
            while(true)
            {
                SearchParameter = IntroPrompt();

                //Method to open up first Stream
                if(getUserInfo(SearchParameter))
                {
                    //Method to open up second Stream
                    InitialConsoleOutput(SearchParameter);

                    break;
                }
            }


            Console.ReadLine();

        } //End Main()

        public static string IntroPrompt()
        {
            Console.WriteLine("Enter your UserName: ");
            string SearchParameter = Console.ReadLine();
            return SearchParameter;
        }
        public static bool getUserInfo(string Search)
        {
            //Validate user input
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    //Instatize UserInforamtion
                    UserInformation myInfo = new UserInformation();


                    //Say hello to GitHub
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                    //Grab userInformation file from Github
                    string json = webClient.DownloadString("https://api.github.com/users/" + Search);

                    //Convert JSON to user-able variables
                    myInfo = JsonConvert.DeserializeObject<UserInformation>(json);

                    //Grab UserInformation from JSON Github (user info) 
                    //
                    //Declare our properties to grab from JSON object
                    myLogin = myInfo.login;
                    myName = myInfo.name;
                    myUrl = myInfo.URL;
                    RepoUrl = myInfo.Repositories_URL;
                    myFollowers = myInfo.Followers;
                    myFollowing = myInfo.Following;
                    myRepoCount = myInfo.public_repos;

                }
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("An error was thrown in the first stream... Press enter to RESTART");
                Console.ReadLine();
                return false;
            }
        } //End Get User Info Method



        //First inital output
        public static void InitialConsoleOutput(string search)
        {
            //Start Output
            //Output Intro for first segment
            Console.WriteLine(Line);
            Console.WriteLine("Name : {0}", myName);
            Console.WriteLine("Your username: {0}", myLogin);
            Console.WriteLine("URL: {0}", myUrl);
            Console.WriteLine("Followers: {0}", myFollowers);
            Console.WriteLine("Following: {0} \n \n ", myFollowing);

            GenerateRepositories(search);
        }

        //Method to search for Repos (Second Segment)
      
        public static void GenerateRepositories(string search)
        {
            //New Stream)
            using (WebClient webClient = new WebClient())
            {
                //Say hello to GitHub
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");


                //Validate user input




                //Grab repository file from Github
                string repoinfo = webClient.DownloadString("https://api.github.com/users/" + search + "/repos");


                //Convert JSON to user-able variables
                //
                //Grab UserInformation from JSON Github (user info) 
                List<RepositoryInformation> SecondInfo = new List<RepositoryInformation>();
                SecondInfo = JsonConvert.DeserializeObject<List<RepositoryInformation>>(repoinfo);



                //Iterate over the List and populating properties (Statgazer_Count, RepoName, watcher_count)
                foreach (var item in SecondInfo)
                {
                    Console.WriteLine("Repository Name: {1} || Stars: {0}  || Watchers: {2}", item.stargazers_count, item.name,
                   item.watchers_count);
                }
            }


        }
    }
}