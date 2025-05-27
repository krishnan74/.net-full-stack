using TwitterClone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Exceptions;
using TwitterClone.Models;

namespace TwitterClone.Repositories
{
    public class TweetRepository : Repository<int, Tweet>
    {
        public TweetRepository() : base()
        {
        }
        
        public override ICollection<Tweet> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("No Tweets found");
            }
            return _items;
        }

        public override Tweet GetById(int id)
        {
            var employee = _items.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Tweet not found");
            }
            return employee;
        }

        protected override int GenerateID()
        {
            if (_items.Count == 0)
            {
                return 1;
            }
            else
            {
                return _items.Max(e => e.Id) + 1;
            }
        }
    }

}