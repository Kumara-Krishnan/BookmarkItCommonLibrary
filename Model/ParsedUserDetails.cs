using BookmarkItCommonLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model
{
    public sealed class ParsedUserDetails
    {
        public UserDetails User { get; set; }

        public string AccessToken { get; set; }

        public ParsedUserDetails(UserDetails user, string accessToken)
        {
            User = user;
            AccessToken = accessToken;
        }
    }
}
