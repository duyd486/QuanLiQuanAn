using QuanLiQuanAn.DBContext;

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
