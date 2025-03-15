using QuanLiQuanAn.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanAn.Singleton
{
    class DatabaseSingleton
    {
        private static DatabaseSingleton? instance;
        public QuanannhatContext db = new QuanannhatContext();
        private DatabaseSingleton()
        {
        }
        public static DatabaseSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseSingleton();
            }
            return instance;
        }
    }
}
