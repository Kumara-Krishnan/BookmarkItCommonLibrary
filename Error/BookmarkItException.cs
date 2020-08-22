using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author kumar-4031
*
* @date - 2/29/2020 10:34:35 PM 
*/
namespace BookmarkItCommonLibrary.Error
{
    public class BookmarkItException : Exception
    {
        public BookmarkItException() : base() { }

        public BookmarkItException(string message) : base(message) { }

        public BookmarkItException(string message, Exception innerException) : base(message, innerException) { }
    }
}