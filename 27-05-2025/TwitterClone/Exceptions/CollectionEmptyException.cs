using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterClone.Exceptions
{
    public class CollectionEmptyException : Exception
    {
        private string _message = "The collection is empty.";
        public CollectionEmptyException(string message)
        {
            _message = message;
        }

        public override string Message => _message;


    }
}
