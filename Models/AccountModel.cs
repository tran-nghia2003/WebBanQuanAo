using Models.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AccountModel
    {
        private OnlineShopDbContext context = null;
        public AccountModel()
        {
            context = new OnlineShopDbContext();
        }
        public bool Login(string tenDangNhap, string matKhau)
        {
            object[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau)
            };

            var res = context.Database.SqlQuery<bool>("Sp_Account_Login @TenDangNhap, @MatKhau", sqlParams).SingleOrDefault();
            return res;
        }
    }

}
