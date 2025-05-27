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
    public class HashTagRepository : Repository<int, HashTag>
    {
        public HashTagRepository() : base()
        {
        }
        
        public override ICollection<HashTag> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("No HashTags found");
            }
            return _items;
        }

        public override HashTag GetById(int id)
        {
            var employee = _items.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new KeyNotFoundException("HashTag not found");
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