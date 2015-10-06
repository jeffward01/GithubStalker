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

        //user Commit Information
        public static string commitUserName;
        public static string commitEmail;

        //user author information
        public static string AuthorInfo;

        

         static void Main(string[] args)
        {
            
                SearchParameter = IntroPrompt();

                //Method to open up first Stream
                getUserInfo(SearchParameter);
                
                    //Method to open up second Stream
                    InitialConsoleOutput(SearchParameter);
                    Console.WriteLine(Line);
                    Console.WriteLine("Press Enter to Continue....");
                    Console.ReadLine();
                    Controller();
                        
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



        //First initial output
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

        public static void Controller()
        {
            Console.WriteLine(Line);
            Console.WriteLine("If you would like to view Commits for each Repository this user has... Please type commit \n If you would like to view Issues for each Repository this user has... Please type issue \n If you would like to search again for a new user.... Please type search \n  If you would like to exit...  Please type exit");
            string input = Console.ReadLine().ToLower();

            switch (input)
            {
                case "commit":
                    Console.WriteLine("commit");
                    grabCommits();
                    Console.ReadLine();
                    break;
                case "issues":
                    Console.WriteLine("issues");
                    Console.ReadLine();
                    break;
                case "search":
                    Console.WriteLine("search");
                    Console.ReadLine();
                    break;
                case "exit":
                    exitPrompt();
                    break;
                default:
                    Controller();
                    break;

            }
        }
        public static void exitPrompt()
        {
            Console.Clear();
            Console.WriteLine(Line);
            Console.WriteLine("Are you sure you want to exit? (yes/no)");
            string input = Console.ReadLine();

            switch (input)
            {
                case "yes":
                    Environment.Exit(0);
                    break;
                case "no":
                    Controller();
                    break;
                default:
                    exitPrompt();
                    break;

            }
        }

        public static void grabCommits()
        {
            Console.WriteLine("Please type in the name of the repository (EXACTLY HOW YOU SEE IT) you wish to view the commits for: ");
            string mySearchRepo = Console.ReadLine();
            string searchProfile = SearchParameter;

            //New Stream)
            using (WebClient webClient = new WebClient())
            {
                //Say hello to GitHub
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");


                //Validate user input


                //Grab repository file from Github
                string repoinfo = webClient.DownloadString("https://api.github.com/repos/" + searchProfile + "/" + mySearchRepo + "/commits");
                

                //Convert JSON to user-able variables
                //


           
                //Grabusername and Email from commit
                List<Commit> myCommitInfor = new List<Commit>();
                myCommitInfor = JsonConvert.DeserializeObject<List<Commit>>(repoinfo);

                

                foreach (var item in myCommitInfor)
                {
                    AuthorInfo = item.author.name;
                    Console.WriteLine(" Url: {0} \n  Item.Author.Name = {1} ------ \n ", item.url, AuthorInfo);
                }
                


                List<Author> myDeserializedObjList = new List<Author>();
                myDeserializedObjList = (List<Author>)Newtonsoft.Json.JsonConvert.DeserializeObject(repoinfo, typeof(List<Author>));

                foreach (var item in myDeserializedObjList)
                {
                 //   Console.WriteLine("myAuthor is: {0}", myDeserializedObjList.name);
                }
            }





        }
    }
        
    

}