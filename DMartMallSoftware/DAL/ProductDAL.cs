using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DMartMallSoftware.DAL
{
    public class ProductDAL : HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public ProductDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));
           
        }

        public List<ProductModel> GetStock()
        {
            List<ProductModel> plist = new List<ProductModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY p.Id desc) as SrNo,p.Id,p.Name,p.Price,p.Quentity,d.DiscountPerc
            from tbl_Product p left join tbl_Discount d on p.DiscountId=d.Id where p.IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ProductModel p = new ProductModel();
                    p.SrNo = Convert.ToInt32(dr["SrNo"]);
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Name = dr["Name"].ToString();
                    p.Price = (float)Convert.ToDouble(dr["Price"]);
                    p.Quentity = Convert.ToInt32(dr["Quentity"]);
                    p.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                    plist.Add(p);
                }
            }
            con.Close();
            return plist;
        }
        public int AddProduct(ProductModel product)
        {
            string qry = @"insert into tbl_Product(Name,Price,Quentity,DiscountId,CreatedBy,CreatedDate) 
                            values(@Name,@Price,@Quentity,@DiscountId,@CreatedBy,@CreatedDate);";
            cmd = new SqlCommand(qry, con);
            product.CreatedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quentity", product.Quentity);
            cmd.Parameters.AddWithValue("@DiscountId", product.DiscountId);
            cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@CreatedDate", product.CreatedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public List<DiscountModel> LoadDiscount()
        {
            List<DiscountModel> discount = new List<DiscountModel>();
            string qry = "select * from tbl_Discount where IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    discount.Add(new DiscountModel
                    {
                        Id = dr.GetInt32("Id"),
                        DiscountPerc = (float)dr.GetDouble("DiscountPerc")
                    }) ;
                }


            }
            con.Close();
            return discount;
        }

        public ProductModel GetProductById(int Id)
        {
            ProductModel p = new ProductModel();
            string qry = @"Select p.Id,p.Name,p.Price,p.Quentity,d.DiscountPerc
            from tbl_Product p left join tbl_Discount d on p.DiscountId=d.Id where p.Id=@Id and p.IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Name = dr["Name"].ToString();
                    p.Price = (float)Convert.ToDouble(dr["Price"]);
                    p.Quentity = Convert.ToInt32(dr["Quentity"]);
                    p.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                }
            }
            con.Close();
            return p;
        }

        public int UpdateProduct(ProductModel product)
        {
            string qry = @"update tbl_Product set Name=@Name,Price=@Price,Quentity=@Quentity,DiscountId=@DiscountId,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);
            product.ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quentity", product.Quentity);
            cmd.Parameters.AddWithValue("@DiscountId", product.DiscountId);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", product.ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int DeleteProduct(int Id)
        {
            string qry = "update tbl_Product set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }



    }
}
