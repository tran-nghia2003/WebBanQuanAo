using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.Dao
{
    public class ProductDao
    {
        OnlineShopDbConext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbConext();
        }

        public IPagedList<Product> ListByCategoryId(long categoryId, int pageNumber, int pageSize)
        {
            var categoryAndChildren = db.ProductCategories.Where(c => c.ID == categoryId || c.ParentID == categoryId).Select(c => c.ID).ToList();

            return db.Products.Where(x => categoryAndChildren.Contains((long)x.CategoryID)).OrderByDescending(x => x.CreateDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<Product> SearchPage(string KeyWord, int pageNumber, int pageSize)
        {


            return db.Products.Where(x => x.Name.Contains(KeyWord)).OrderByDescending(x => x.CreateDate).ToPagedList(pageNumber, pageSize);
        }


        public IEnumerable<Product> ListAllPaging(int page, int pageSize)
        {
            return db.Products.ToPagedList(page, pageSize);
        }


        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x=>x.TopHot == null).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();

        }

        public List<Product> ListRelatedProduct(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x=>x.ID != productId && x.CategoryID == product.CategoryID).Take(5).ToList();
        }

        public ProductImage ListImageProduct(string Code)
        {
            return db.ProductImages.Find(Code);
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public List<Product> ListProductAll()
        {
            return db.Products.ToList();
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }



    }
}
