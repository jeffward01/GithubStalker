using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubStalker.Classes
{
    class UserInformation
    {
        //Constructor
        public UserInformation()
        {
        }

        //Properties
        public string login { get; set; }
        public string name { get; set; }
        public string URL { get; set; }
        public string Repositories_URL { get; set; }
        public string Followers { get; set; }
        public string Following { get; set; }
        public string public_repos { get; set; }

    }
}
