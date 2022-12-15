using TimeKeeper.Funtions;
using TimeKeeper.Models;

namespace TimeKeeper.Funtions
{
    public class TimeKeeperMemberShipProvider : Encryptions
    {
        private readonly TimeKeeperDBContext DB;

        public TimeKeeperMemberShipProvider(TimeKeeperDBContext context)
        {
            DB = context;
        }


        public static bool ValidateUser(string user, string password)
        {
            if(!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                return true;
            }



            return false;
        }

       
    }
}
