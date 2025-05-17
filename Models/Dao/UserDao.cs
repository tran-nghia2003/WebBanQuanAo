using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Models.EF;
using Models.ViewModel;
using PagedList;

namespace Models.Dao
{
    public class UserDao
    {
        OnlineShopDbConext db = null;
        public UserDao()
        {
            db = new OnlineShopDbConext();
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
            
        }

        public IEnumerable<User> ListAllPaging(int page, int pageSize)
        {
            return db.Users.ToPagedList(page, pageSize);
        }

        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x=>x.UserName == userName); 
        }

        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }

        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }

        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

        public List<OrderViewModel> GetOrdersAndDetailsByCustomerId(long customerId)
        {
            var query = from order in db.Orders
                        join orderDetail in db.OrderDetails on order.ID equals orderDetail.OrderID
                        join product in db.Products on orderDetail.ProductID equals product.ID
                        where order.CustomerID == customerId
                        select new OrderViewModel()
                        {
                            NameProduct = product.Name,
                            Image = product.Image,
                            Price = orderDetail.Price,
                            Quantity = orderDetail.Quantity,
                            CreatedDate = order.CreatedDate,
                            TypePayment = order.TypePayment,
                            Status = order.Status

                        };
            query.OrderByDescending(x => x.CreatedDate);

            return query.ToList();
        }

        public List<OrderDetailViewModel> GetOrdersAndDetailsByOrderId(int orderId)
        {
            var query = from orderDetail in db.OrderDetails
                        join product in db.Products on orderDetail.ProductID equals product.ID
                        where orderDetail.OrderID == orderId
                        select new OrderDetailViewModel()
                        {
                            ProductID = product.ID,
                            NameProduct = orderDetail.NameProduct,
                            Image = product.Image,
                            Price = orderDetail.Price,
                            Quantity = orderDetail.Quantity,

                        };
            return query.ToList();
        }
    }
}
