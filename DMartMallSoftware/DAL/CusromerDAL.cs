using DMartMallSoftware.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static NuGet.Packaging.PackagingConstants;

namespace DMartMallSoftware.DAL
{
    public class CusromerDAL:HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public CusromerDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));

        }

        public List<CustomerModel> GetAllCustomers(string? MobileNo)
        {
            MobileNo = MobileNo == null ? "" : MobileNo;
            List<CustomerModel> clist = new List<CustomerModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY Id desc) as SrNo,Id,Name,MobileNo,Address,SubTotal,TotalDiscount,
                GrandTotal,Remark from tblCustomer where IsDeleted=0 and (MobileNo like '%'+@MobileNo+'%' or @MobileNo is null ) ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    CustomerModel c = new CustomerModel();
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.Name = dr["Name"].ToString(); 
                    c.MobileNo = dr["MobileNo"].ToString(); 
                    c.Address = dr["Address"].ToString(); 
                    c.SubTotal = (float)Convert.ToDouble(dr["SubTotal"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.GrandTotal = (float)Convert.ToDouble(dr["GrandTotal"]);
                    c.Remark = dr["Remark"].ToString();
                    clist.Add(c);
                }
            }
            con.Close();
            return clist;
        }

        public CustomerModel GetSelectedCustomers(int MobileNo)
        {
            CustomerModel c = new CustomerModel();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY Id desc) as SrNo,Id,Name,MobileNo,Address,SubTotal,TotalDiscount,
                GrandTotal,Remark from tbl_Customer where IsDeleted=0 and MobileNo=@MobileNo ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.Name = dr["Name"].ToString();
                    c.MobileNo = dr["MobileNo"].ToString();
                    c.Address = dr["Address"].ToString();
                    c.SubTotal = (float)Convert.ToDouble(dr["SubTotal"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.GrandTotal = (float)Convert.ToDouble(dr["GrandTotal"]);
                    c.Remark = dr["Remark"].ToString();
                }
            }
            con.Close();
            return c;
            
          
        }
        public CustomerModel GetCustomerById(int Id)
        {
            CustomerModel c = new CustomerModel();
            string qry = @"Select Id,Name,MobileNo,Address,SubTotal,TotalDiscount,
                GrandTotal,Remark from tbl_Customer where Id=@Id and IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.Name = dr["Name"].ToString();
                    c.MobileNo = dr["MobileNo"].ToString();
                    c.Address = dr["Address"].ToString();
                    c.SubTotal = (float)Convert.ToDouble(dr["SubTotal"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.GrandTotal = (float)Convert.ToDouble(dr["GrandTotal"]);
                    c.Remark = dr["Remark"].ToString();
                    c.Cartdetails = GetCartItems(c.Id);
                    //con.Open();
                }
            }
            con.Close();
            return c;
        }

        public List<CartModel> GetCartItems(int Id)
        {
            con.Close();
            List<CartModel> clist = new List<CartModel>();
            string qry1 = @"Select ROW_NUMBER() OVER(ORDER BY c.Id desc) as SrNo,c.Id,c.OrderId,c.ProductId,p.Name,p.Price,p.DiscountId,d.DiscountPerc,c.Quentity,c.TotalAmount,
            c.TotalDiscount,c.NetAmount,c.Discount from tbl_Product p left join tbl_Cart c on p.Id=c.ProductId left join
            tbl_Discount d on p.DiscountId=d.Id where c.Id=any(select Id from tbl_Cart where OrderId=@OrderId)";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@OrderId", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    CartModel c = new CartModel();
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.OrderId = Convert.ToInt32(dr["OrderId"]);
                    c.ProductId = Convert.ToInt32(dr["ProductId"]);
                    c.Name = dr["Name"].ToString();
                    c.Price = (float)Convert.ToDouble(dr["Price"]);
                    c.DiscountId = Convert.ToInt32(dr["DiscountId"]);
                    c.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                    c.Quentity = Convert.ToInt32(dr["Quentity"]);
                    c.TotalAmount = (float)Convert.ToDouble(dr["TotalAmount"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.NetAmount = (float)Convert.ToDouble(dr["NetAmount"]);
                    c.Discount = (float)Convert.ToDouble(dr["Discount"]);
                    clist.Add(c);
                }
            }
            //con.Close();
            return clist;
        }

        public int AddCustomer(CustomerModel customer)
        {
          
            string qry1 = @"select Id from tbl_Customer Where MobileNo=@MobileNo and IsDeleted=0;";
          
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer.Id = Convert.ToInt32(dr["Id"]);
                }
                if(customer.Id != 0)
                {
                    return -1;
                }
            }
            con.Close();
            string qry = @"insert into tbl_Customer(Name,MobileNo,Address,SubTotal,TotalDiscount,GrandTotal,Remark,CreatedBy,CreatedDate) 
                            values(@Name,@MobileNo,@Address,0,0,0,'Pending',@CreatedBy,@CreatedDate);";
            cmd = new SqlCommand(qry, con);
            customer.CreatedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
            cmd.Parameters.AddWithValue("@Address", customer.Address);
            cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@CreatedDate", customer.CreatedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

      
        public int AddProduct(CartModel cart,int CustId)
        {
            int result = 0;
            string query1 = @"select SubTotal,TotalDiscount,GrandTotal from tbl_Customer where Id=@Id ";
            cmd = new SqlCommand(query1, con);
            cmd.Parameters.AddWithValue("@Id", CustId);
            con.Open();
            dr = cmd.ExecuteReader();
           
            if (dr.HasRows)
            {
                if(dr.Read())
                {
                    float subtotal = (float)Convert.ToDouble(dr["SubTotal"]);
                    float grandtotal = (float)Convert.ToDouble(dr["GrandTotal"]);
                    float totaldiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    con.Close();

                    string qry = @"insert into tbl_Cart(ProductId,Quentity,OrderId,Discount,Amount,TotalAmount,TotalDiscount,NetAmount,CreatedBy,CreatedDate)
                                      VALUES(@ProductId,@Quentity,@OrderId,0,0,0,0,0,@CreatedBy,@CreatedDate);";

                    cmd = new SqlCommand(qry, con);
                    cart.CreatedDate = DateTime.Now;
                    cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                    cmd.Parameters.AddWithValue("@Quentity", cart.Quentity);
                    cmd.Parameters.AddWithValue("@OrderId", CustId);
                    cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                    cmd.Parameters.AddWithValue("@CreatedDate", cart.CreatedDate);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                    string qury = @"select top 1 Id from tbl_Cart where CreatedDate<=Current_Timestamp order by Id desc";
                    cmd = new SqlCommand(qury, con);
                    con.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cart.Id = Convert.ToInt32(dr["Id"]);
                        }
                    }
                    con.Close();
                    
                    var pquery = "select Name,Price,DiscountId from tbl_Product where Id=@Id";
                    cmd = new SqlCommand(pquery, con);
                    cmd.Parameters.AddWithValue("@Id", cart.ProductId);
                   con.Open();
                    dr = cmd.ExecuteReader();
                    
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cart.Name = dr["Name"].ToString();
                            cart.Price = (float)Convert.ToDouble(dr["Price"]);
                            cart.DiscountId = Convert.ToInt32(dr["DiscountId"]);
                        }
                    }
                    con.Close();
                    var pquery1 = "select DiscountPerc from tbl_Discount where Id=@Id";
                    cmd = new SqlCommand(pquery1, con);
                    cmd.Parameters.AddWithValue("@Id", cart.DiscountId);
                   con.Open();
                    dr = cmd.ExecuteReader();
                    
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cart.DiscountPerc= (float)Convert.ToDouble(dr["DiscountPerc"]);
                            cart.Discount= (cart.Price / 100) * cart.DiscountPerc;
                            cart.TotalDiscount = cart.Discount * cart.Quentity;
                            cart.TotalAmount = cart.Price * cart.Quentity;
                            cart.NetAmount = cart.TotalAmount - cart.TotalDiscount;
                        }
                    }
                    con.Close();
                    var qry1 = @"update tbl_Cart set TotalAmount=@TotalAmount,TotalDiscount=@TotalDiscount,
                                    NetAmount=@NetAmount ,Discount=@Discount where Id=@Id";
                    cmd = new SqlCommand(qry1, con);
                    cmd.Parameters.AddWithValue("@TotalAmount", cart.TotalAmount);
                    cmd.Parameters.AddWithValue("@TotalDiscount", cart.TotalDiscount);
                    cmd.Parameters.AddWithValue("@NetAmount", cart.NetAmount);
                    cmd.Parameters.AddWithValue("@Discount", cart.Discount);
                    cmd.Parameters.AddWithValue("@Id", cart.Id);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                    subtotal = subtotal + cart.TotalAmount;
                    grandtotal = grandtotal + cart.NetAmount;
                    totaldiscount = totaldiscount + cart.TotalDiscount;

                    ProductModel p=new ProductModel();
                    var countquery = "Select quentity from tbl_Product where Id=@Id";
                    cmd = new SqlCommand(countquery, con);
                    cmd.Parameters.AddWithValue("@Id", cart.ProductId);
                    con.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            p.Quentity = Convert.ToInt32(dr["Quentity"]);
                        }
                    }
                    con.Close();

                    p.Quentity=p.Quentity-cart.Quentity;
                    var updatecountquery = "update tbl_Product set quentity=@quentity where Id=@Id";
                    cmd = new SqlCommand(updatecountquery, con);
                    cmd.Parameters.AddWithValue("@quentity", p.Quentity);
                    cmd.Parameters.AddWithValue("@Id", cart.ProductId);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();

                    var qry2 = @"update tbl_Customer set SubTotal=@SubTotal,GrandTotal=@GrandTotal,
                                    Totaldiscount=@Totaldiscount where Id=@Id";
                    cmd = new SqlCommand(qry2, con);
                    cmd.Parameters.AddWithValue("@SubTotal", subtotal);
                    cmd.Parameters.AddWithValue("@GrandTotal", grandtotal);
                    cmd.Parameters.AddWithValue("@Totaldiscount", totaldiscount);
                    cmd.Parameters.AddWithValue("@Id", CustId);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    if (result == 1)
                        result = CustId;
                   
                }
            }
            con.Close();
            return result;
        }

        public List<ProductModel> LoadProducts()
        {
            List<ProductModel> product = new List<ProductModel>();
            string qry = "select Id,Name,Price from tbl_Product where IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    product.Add(new ProductModel
                    {
                        Id = dr.GetInt32("Id"),
                        Name=dr.GetString("Name"),
                        Price=(float)dr.GetDouble("Price")
                    });
                }


            }
            con.Close();
            return product;
        }

    }
}
