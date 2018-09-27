using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsAPI.Models;
using DataTrans;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web;
using System.Data;

namespace WebFormsAPI.Class
{
    public class ProductOps
    {
        private static List<Products> products;

        public static List<Products> GetAllProducts()
        {
            System.Data.DataSet dtd = new System.Data.DataSet();
            dtd.ReadXml(HttpContext.Current.Server.MapPath("~/App_Data/DropDownMsgList.xml"));
            JavaScriptSerializer jsontest = new JavaScriptSerializer();

            List<Products> objNewData = new List<Products>();
            if (dtd != null)
            {
                objNewData = (from p in dtd.Tables[0].AsEnumerable()
                              select new Products()
                              {
                                  ID = p.Field<String>("SelTabID"),
                                  Name = p.Field<String>("DDLMsg")
                              }).ToList();
            }

            return objNewData;
        }
        public static List<Products> GetProduct(string ProductID)
        {
            List<Products> objlstPrd = new List<Products>();
            products = GetAllProducts();
            Products objProd = new Products();
           objProd= new Products()
           {
               ID = products.Find(p => p.ID.Equals(ProductID)).ID,
               Name = products.Find(p => p.ID.Equals(ProductID)).Name,
               Category = products.Find(p => p.ID.Equals(ProductID)).Category,
               Price = products.Find(p => p.ID.Equals(ProductID)).Price
           };

           objlstPrd.Add(objProd);
           return objlstPrd;

            //products = (select new Products()
            //            {
            //                ID= products.Find(p=>p.ID.Equals(ProductID)).ID,
            //            }
            //            ).
                //
            //return products;
        }
        public static void RemoveProduct(string studentID)
        {
            //Code Logic to delete a student
        }
        public static void AddProduct(Products student)
        {
            //Code Logic to Add a new student.
        }
        public static void UpdateProduct(Products student)
        {
            //Code Logic to Update a student.
        }
    }
}
