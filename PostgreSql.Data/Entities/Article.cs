using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSql.Data.Entities
{
    public class Article : BaseEntity
    {

        public string Title { get; set; }
        
        public string Description { get; set; }
    }
}
