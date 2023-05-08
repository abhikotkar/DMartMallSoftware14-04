﻿using DMartMallSoftware.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml.Linq;
using static NuGet.Packaging.PackagingConstants;

namespace DMartMallSoftware.DAL
{
    public class CusromerDAL : HttpContextAccessor
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
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY tblCustomer.Id desc) as SrNo,tblCustomer.Id,Name,MobileNo,Address
                                                                            ,TotalAmt,TotalDiscount,PayAmt,RemarkId,
                                                                            r.Remark from tblCustomer left join tblRemark r 
			                                                                on tblCustomer.RemarkId=r.Id where tblCustomer.IsDeleted=0 
			                                                                and (MobileNo like '%'+@MobileNo+'%' or @MobileNo is null ) ";
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
                    c.RemarkId = Convert.ToInt32(dr["RemarkId"]);
                    c.Name = dr["Name"].ToString();
                    c.MobileNo = dr["MobileNo"].ToString();
                    c.Address = dr["Address"].ToString();
                    c.TotalAmt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.PayAmt = (float)Convert.ToDouble(dr["PayAmt"]);
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
            string qry = @"Select tblCustomer.Id,Name,MobileNo,Address
                                                                            ,TotalAmt,TotalDiscount,PayAmt,
                                                                            r.Remark from tblCustomer left join tblRemark r 
			                                                                on tblCustomer.RemarkId=r.Id where tblCustomer.IsDeleted=0 
			                                                                and tblCustomer.Id=@Id";
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
                    c.TotalAmt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.PayAmt = (float)Convert.ToDouble(dr["PayAmt"]);
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
            string qry1 = @"Select ROW_NUMBER() OVER(ORDER BY c.Id desc) as SrNo,c.Id,c.ProductId,u.Unit,
			                                            s.Name,s.Price,s.DiscountId,d.DiscountPerc,c.Quantity,c.TotalAmt,
                                                        c.TotalDiscount,c.NetAmt,c.Discount from tblStock s 
			                                            left join tblCart c on s.Id=c.ProductId 
			                                            left join tblDiscount d on s.DiscountId=d.Id
			                                            left join tblUnit u on s.UnitId=u.Id
			                                             where c.CustId=@Id and c.IsDeleted=0";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    CartModel c = new CartModel();
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.ProductId = Convert.ToInt32(dr["ProductId"]);
                    c.Unit = dr["Unit"].ToString();
                    c.Name = dr["Name"].ToString();
                    c.Price = (float)Convert.ToDouble(dr["Price"]);
                    c.DiscountId = Convert.ToInt32(dr["DiscountId"]);
                    c.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                    c.Quantity = Convert.ToInt32(dr["Quantity"]);
                    c.TotalAmt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.NetAmt = (float)Convert.ToDouble(dr["NetAmt"]);
                    c.Discount = (float)Convert.ToDouble(dr["Discount"]);
                    clist.Add(c);
                }
            }
            //con.Close();
            return clist;
        }

        public int AddCustomer(CustomerModel customer)
        {

            string qry1 = @"select Id from tblCustomer Where MobileNo=@MobileNo and IsDeleted=0;";

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
                if (customer.Id != 0)
                {
                    return -1;
                }
            }
            con.Close();
            string qry = @"insert into tblCustomer(Name,MobileNo,Address,TotalAmt,TotalDiscount,PayAmt,RemarkId,CreatedBy,CreatedDate,IsDeleted) 
                            values(@Name,@MobileNo,@Address,0,0,0,1,@CreatedBy,@CreatedDate,0);";
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


        public int AddProduct(CartModel cart, int CustId)
        {
            int result = 0;
            string query2 = @"select 1 from tblStock where Id=@ProductId  and UnitId=@UnitId and IsDeleted=0";
            cmd = new SqlCommand(query2, con);
            cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
            cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
            con.Open();
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                con.Close();
                string query1 = @"select TotalAmt,TotalDiscount,PayAmt from tblCustomer where Id=@Id and RemarkId=1 and IsDeleted=0 ";
                cmd = new SqlCommand(query1, con);
                cmd.Parameters.AddWithValue("@Id", CustId);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        float subtotal = (float)Convert.ToDouble(dr["TotalAmt"]);
                        float grandtotal = (float)Convert.ToDouble(dr["PayAmt"]);
                        float totaldiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                        con.Close();
                        string querys1 = @" select Id,Quantity,Price,Discount,TotalAmt,TotalDiscount,NetAmt from tblcart 
                                                                    where CustId=@CustId and IsDeleted=0 and ProductId=@ProductId and UnitId=@UnitId";
                        cmd = new SqlCommand(querys1, con);
                        cmd.Parameters.AddWithValue("@CustId", CustId);
                        cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                        cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
                        con.Open();
                        dr = cmd.ExecuteReader();
                        //con.Close() ;
                        if (dr.HasRows)
                        {
                            if (dr.Read())
                            {
                                cart.Id = Convert.ToInt32(dr["Id"]);
                                var quantity = Convert.ToInt32(dr["Quantity"]);
                                var price = (float)Convert.ToDouble(dr["Price"]);
                                var discount = (float)Convert.ToDouble(dr["Discount"]);
                                var totalAmt = (float)Convert.ToDouble(dr["TotalAmt"]);
                                var totalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                                var netAmt = (float)Convert.ToDouble(dr["NetAmt"]);
                                con.Close();


                                //cart.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                                //.Discount = (cart.Price / 100) * cart.DiscountPerc;
                                //cart.Quantity = cart.Quantity + quantity;
                                cart.TotalDiscount = (discount * cart.Quantity);
                                cart.TotalAmt = (price * cart.Quantity);
                                cart.NetAmt = (cart.TotalAmt - cart.TotalDiscount);
                                subtotal = subtotal + cart.TotalAmt;
                                grandtotal = grandtotal + cart.NetAmt;
                                totaldiscount = totaldiscount + cart.TotalDiscount;
                                cart.NetAmt = (cart.TotalAmt - cart.TotalDiscount) + netAmt;
                                cart.TotalDiscount = (discount * cart.Quantity) + totalDiscount;
                                cart.TotalAmt = (price * cart.Quantity) + totalAmt;
                                var qqq = cart.Quantity + quantity;
                                var qrys2 = @"update tblCart set TotalAmt=@TotalAmt,TotalDiscount=@TotalDiscount,
                                    NetAmt=@NetAmt ,Quantity=@Quantity where Id=@Id";
                                cmd = new SqlCommand(qrys2, con);
                                cmd.Parameters.AddWithValue("@TotalAmt", cart.TotalAmt);
                                cmd.Parameters.AddWithValue("@TotalDiscount", cart.TotalDiscount);
                                cmd.Parameters.AddWithValue("@NetAmt", cart.NetAmt);
                                cmd.Parameters.AddWithValue("@Quantity",qqq);
                                cmd.Parameters.AddWithValue("@Id", cart.Id);
                                con.Open();
                                result = cmd.ExecuteNonQuery();
                                con.Close();

                            }
                        }
                        else
                        {
                            con.Close();
                            string qry = @"insert into tblCart(CustId,ProductId,UnitId,Quantity,Price,Discount,TotalAmt,TotalDiscount,NetAmt,CreatedBy,CreatedDate,IsDeleted)
                                      VALUES(@CustId,@ProductId,@UnitId,@Quantity,0,0,0,0,0,@CreatedBy,@CreatedDate,0);";

                            cmd = new SqlCommand(qry, con);
                            cart.CreatedDate = DateTime.Now;
                            cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
                            cmd.Parameters.AddWithValue("@CustId", CustId);
                            cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
                            cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                            cmd.Parameters.AddWithValue("@CreatedDate", cart.CreatedDate);
                            con.Open();
                            result = cmd.ExecuteNonQuery();
                            con.Close();
                            string qury = @"select top 1 Id from tblCart where CreatedDate<=Current_Timestamp order by Id desc";
                            cmd = new SqlCommand(qury, con);
                            con.Open();
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    cart.Id = Convert.ToInt32(dr["Id"]);
                                }
                            }
                            con.Close();

                            var pquery = "select Name,Price,DiscountId,UnitId from tblStock where Id=@Id";
                            cmd = new SqlCommand(pquery, con);
                            cmd.Parameters.AddWithValue("@Id", cart.ProductId);
                            con.Open();
                            dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    cart.Name = dr["Name"].ToString();
                                    cart.Price = (float)Convert.ToDouble(dr["Price"]);
                                    cart.DiscountId = Convert.ToInt32(dr["DiscountId"]);
                                    cart.UnitId = Convert.ToInt32(dr["UnitId"]);
                                }
                            }
                            con.Close();
                            var pquery1 = "select DiscountPerc from tblDiscount where Id=@Id";
                            cmd = new SqlCommand(pquery1, con);
                            cmd.Parameters.AddWithValue("@Id", cart.DiscountId);
                            con.Open();
                            dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    cart.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                                    cart.Discount = (cart.Price / 100) * cart.DiscountPerc;
                                    cart.TotalDiscount = cart.Discount * cart.Quantity;
                                    cart.TotalAmt = cart.Price * cart.Quantity;
                                    cart.NetAmt = cart.TotalAmt - cart.TotalDiscount;
                                }
                            }
                            con.Close();
                            var qry1 = @"update tblCart set TotalAmt=@TotalAmt,TotalDiscount=@TotalDiscount,
                                    NetAmt=@NetAmt ,Discount=@Discount,Price=@Price where Id=@Id";
                            cmd = new SqlCommand(qry1, con);
                            cmd.Parameters.AddWithValue("@TotalAmt", cart.TotalAmt);
                            cmd.Parameters.AddWithValue("@TotalDiscount", cart.TotalDiscount);
                            cmd.Parameters.AddWithValue("@NetAmt", cart.NetAmt);
                            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
                            cmd.Parameters.AddWithValue("@Price", cart.Price);
                            cmd.Parameters.AddWithValue("@Id", cart.Id);
                            con.Open();
                            result = cmd.ExecuteNonQuery();
                            con.Close();

                            subtotal = subtotal + cart.TotalAmt;
                            grandtotal = grandtotal + cart.NetAmt;
                            totaldiscount = totaldiscount + cart.TotalDiscount;
                        }
                        ProductModel p = new ProductModel();
                        var countquery = "Select Quantity,UnitId from tblStock where Id=@Id";
                        cmd = new SqlCommand(countquery, con);
                        cmd.Parameters.AddWithValue("@Id", cart.ProductId);
                        con.Open();
                        dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            if (dr.Read())
                            {
                                p.Quantity = Convert.ToInt32(dr["Quantity"]);
                                p.UnitId = Convert.ToInt32(dr["UnitId"]);
                            }
                        }
                        con.Close();

                        p.Quantity = p.Quantity - cart.Quantity;
                        var updatecountquery = "update tblStock set quantity=@quantity where Id=@Id";
                        cmd = new SqlCommand(updatecountquery, con);
                        cmd.Parameters.AddWithValue("@quantity", p.Quantity);
                        cmd.Parameters.AddWithValue("@Id", cart.ProductId);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        con.Close();

                        var qry2 = @"	update tblCustomer set TotalAmt=@TotalAmt,PayAmt=@PayAmt,
                                    Totaldiscount=@Totaldiscount where Id=@Id";
                        cmd = new SqlCommand(qry2, con);
                        cmd.Parameters.AddWithValue("@TotalAmt", subtotal);
                        cmd.Parameters.AddWithValue("@PayAmt", grandtotal);
                        cmd.Parameters.AddWithValue("@Totaldiscount", totaldiscount);
                        cmd.Parameters.AddWithValue("@Id", CustId);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        if (result == 1)
                            result = CustId;
                        con.Close();
                    }
                }
            }
            return result;
        }

        public int UpdateCustomerDetails(CustomerModel customer)
        {
            int result = 0;
            int id = 0;
            string qry1 = @"Select Id from tblCustomer where MobileNo=@MobileNo and Id!=@Id and IsDeleted=0";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
            cmd.Parameters.AddWithValue("@Id", customer.Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    id = Convert.ToInt32(dr["Id"]);
                    //break;
                }
            }
            con.Close();
            if (id == 0)
            {
                string qry = @"update tblCustomer set Name=@Name,MobileNo=@MobileNo,Address=@Address,
                                                ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
                cmd = new SqlCommand(qry, con);
                customer.ModifiedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id", customer.Id);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@ModifiedDate", customer.ModifiedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            return result;
        }

        public int CancelCustomerBill(int Id)
        {
            string qry = "update tblCustomer set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);
            var ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if(result>=1)
            {
                string qry1 = "update tblCart set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where CustId=@CustId";
                cmd = new SqlCommand(qry1, con);

                cmd.Parameters.AddWithValue("@CustId", Id);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            return result;
        }
        public List<ProductModel> LoadProducts()
        {
            List<ProductModel> product = new List<ProductModel>();
            string qry = "select Id,Name,Price from tblStock where IsDeleted=0 and Quantity>0";
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
                        Name = dr.GetString("Name"),
                        Price = (float)dr.GetDouble("Price")
                    });
                }


            }
            con.Close();
            return product;
        }


        public List<UnitModel> LoadUnit()
        {
            List<UnitModel> unit = new List<UnitModel>();
            string qry = "select * from tblUnit where IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    unit.Add(new UnitModel
                    {
                        Id = dr.GetInt32("Id"),
                        Unit = dr.GetString("Unit")
                    });
                }


            }
            con.Close();
            return unit;
        }

    }
}
