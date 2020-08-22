using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author kumar-4031
*
* @date - 3/1/2020 7:16:10 PM 
*/
namespace BookmarkItCommonLibrary.Model
{
    public sealed class UserStat
    {
        public readonly string UserId;

        public int TotalItemsCount { get; set; }

        public int UnreadItemsCount { get; set; }

        public int ReadItemsCount { get; set; }

        public UserStat(string userId)
        {
            UserId = userId;
        }
    }
}