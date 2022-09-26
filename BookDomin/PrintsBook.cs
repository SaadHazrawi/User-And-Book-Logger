using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDomin
{
    public class PrintsBook
    {
        public int Id { get; set; }
        //اسم دار الطباعة
        public string LibName { get; set; }
        public int BookID { get; set; }
        public Book Book { get; set; } = new Book();
    }
}
