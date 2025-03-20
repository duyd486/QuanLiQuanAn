using QuanLiQuanAn.DBContext;

namespace QuanLiQuanAn.Singleton
{
    class DatabaseSingleton
    {
        private static DatabaseSingleton? instance;
        public QuanannhatContext db;
        private DatabaseSingleton()
        {
            db = new QuanannhatContext();
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
