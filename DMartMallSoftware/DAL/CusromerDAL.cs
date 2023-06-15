using DMartMallSoftware.Models;
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

        /*public List<CustomerModel> GetAllCustomers(string? MobileNo)
        {
            MobileNo = MobileNo == null ? "" : MobileNo;
            List<CustomerModel> clist = new List<CustomerModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY tblCustomer.Id desc) as SrNo,tblCustomer.Id,Name,MobileNo,Address
                                                                            ,TotalQuantity,TotalAmt,TotalDiscount,PayAmt,RemarkId,
                                                                            r.Remark from tblCustomer left join tblRemark r 
			                                                                on tblCustomer.RemarkId=r.Id where tblCustomer.IsDeleted=0 
			                                                                and (MobileNo like '%'+@MobileNo+'%' or @MobileNo is null ) 
                                                                            and tblCustomer.CreatedBy =@CreatedBy";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
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
                    c.TotalQuantity = Convert.ToInt32(dr["TotalQuantity"]);
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
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY Id desc) as SrNo,Id,Name,MobileNo,Address,TotalQuantity,SubTotal,TotalDiscount,
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
                    c.TotalQuantity = Convert.ToInt32(dr["TotalQuantity"]);
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
            string qry = @"Select tblCustomer.Id,Name,MobileNo,Address,TotalQuantity
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
                    c.TotalQuantity = Convert.ToInt32(dr["TotalQuantity"]);
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
        public CartModel GetCartItemById(int Id)
        {
            CartModel c = new CartModel();
            string qry1 = @"Select c.Id,c.ProductId,u.Unit,c.CustId,
			                                            s.Name,s.Price,s.DiscountId,d.DiscountPerc,c.Quantity,c.TotalAmt,
                                                        c.TotalDiscount,c.NetAmt,c.Discount from tblStock s 
			                                            left join tblCart c on s.Id=c.ProductId 
			                                            left join tblDiscount d on s.DiscountId=d.Id
			                                            left join tblUnit u on s.UnitId=u.Id
			                                             where c.Id=@Id and c.IsDeleted=0";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.CustId = Convert.ToInt32(dr["CustId"]);
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
                }
            }
            //con.Close();
            return c;
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
            string qry = @"insert into tblCustomer(Name,MobileNo,Address,TotalQuantity,TotalAmt,TotalDiscount,PayAmt,RemarkId,CreatedBy,CreatedDate,IsDeleted) 
                            values(@Name,@MobileNo,@Address,0,0,0,0,1,@CreatedBy,@CreatedDate,0);";
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
            if (cart.ProductId == 0)
            {
                string query = @"select Id from tblStock where Name=@Name  and UnitId=@UnitId and IsDeleted=0";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", cart.Name);
                cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        cart.ProductId = Convert.ToInt32(dr["Id"]);
                    }
                    
                }
            }
            con.Close();
            string query2 = @"select 1 from tblStock where Id=@ProductId  and UnitId=@UnitId and IsDeleted=0";
            cmd = new SqlCommand(query2, con);
            cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
            cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
            con.Open();
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                con.Close();
                string query1 = @"select TotalAmt,TotalDiscount,PayAmt,TotalQuantity from tblCustomer where Id=@Id and RemarkId=1 and IsDeleted=0 ";
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
                        float totalquantity = Convert.ToInt32(dr["TotalQuantity"]);
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
                                totalquantity = (totalquantity) + cart.Quantity;
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
                            totalquantity = totalquantity + cart.Quantity;
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
                                    Totaldiscount=@Totaldiscount,TotalQuantity=@TotalQuantity where Id=@Id";
                        cmd = new SqlCommand(qry2, con);
                        cmd.Parameters.AddWithValue("@TotalAmt", subtotal);
                        cmd.Parameters.AddWithValue("@PayAmt", grandtotal);
                        cmd.Parameters.AddWithValue("@Totaldiscount", totaldiscount);
                        cmd.Parameters.AddWithValue("@TotalQuantity", totalquantity);
                        cmd.Parameters.AddWithValue("@Id", CustId);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        if (result == 1)
                            result = CustId;
                        con.Close();
                    }
                }
                con.Close();
            }
            con.Close() ;
            return result;
        }

        public int EditCartItem(CartModel cart)
        {
            float totalquantity;
            float subtotal;
            float grandtotal;
            float totaldiscount;
            float totalamt=0;
            float netamt=0;
            float totaldisc= 0;
            int qua= 0;
            int quantity = 0;
            int productid= 0;
            int custid = 0;
            int result = 0;
            var query = @"select Id,TotalAmt,TotalDiscount,NetAmt,CustId,Quantity,ProductId from tblcart where Id=@Id and IsDeleted=0";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", cart.Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    custid = Convert.ToInt32(dr["CustId"]);
                    totalamt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    netamt = (float)Convert.ToDouble(dr["NetAmt"]);
                    totaldisc = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    qua = Convert.ToInt32(dr["Quantity"]);
                    productid = Convert.ToInt32(dr["ProductId"]);
                }
            }
            con.Close();
            int Id = cart.Id;
            var qrys2 = @"update tblCart set IsDeleted=1 where Id=@Id";
            cmd = new SqlCommand(qrys2, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            result = cmd.ExecuteNonQuery();
            con.Close();
            //cart.ProductId = productid;
            if(cart.Name!=null && (cart.UnitId!=null|| cart.UnitId!=0))
                result = AddProduct(cart, custid);
            if (result >= 1)
            {
                var countquery = "Select Quantity from tblStock where Id=@Id";
                cmd = new SqlCommand(countquery, con);
                cmd.Parameters.AddWithValue("@Id", productid);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        quantity = Convert.ToInt32(dr["Quantity"]);
                    }
                }

                con.Close();
                string query1 = @"select TotalQuantity,TotalAmt,TotalDiscount,PayAmt from tblCustomer where Id=@Id and RemarkId=1 and IsDeleted=0 ";
                cmd = new SqlCommand(query1, con);
                cmd.Parameters.AddWithValue("@Id", custid);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        totalquantity = Convert.ToInt32(dr["TotalQuantity"]);
                        subtotal = (float)Convert.ToDouble(dr["TotalAmt"]);
                        grandtotal = (float)Convert.ToDouble(dr["PayAmt"]);
                        totaldiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);

                        subtotal = subtotal - totalamt;
                        grandtotal = grandtotal - netamt;
                        totaldiscount = totaldiscount - totaldisc;
                        totalquantity = totalquantity - qua;
                        quantity = quantity + qua;
                        con.Close();
                        var updatecountquery = "update tblStock set quantity=@quantity where Id=@Id";
                        cmd = new SqlCommand(updatecountquery, con);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@Id", productid);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        con.Close();

                        var qry2 = @"	update tblCustomer set TotalAmt=@TotalAmt,PayAmt=@PayAmt,
                                    Totaldiscount=@Totaldiscount,TotalQuantity=@TotalQuantity where Id=@Id";
                        cmd = new SqlCommand(qry2, con);
                        cmd.Parameters.AddWithValue("@TotalAmt", subtotal);
                        cmd.Parameters.AddWithValue("@PayAmt", grandtotal);
                        cmd.Parameters.AddWithValue("@Totaldiscount", totaldiscount);
                        cmd.Parameters.AddWithValue("@TotalQuantity", totalquantity);
                        cmd.Parameters.AddWithValue("@Id", custid);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        if (result == 1)
                            result = custid;
                        con.Close();

                    }
                }
                con.Close();
               
                
            }
            else
            {
                var qrys5 = @"update tblCart set IsDeleted=0 where Id=@Id";
                cmd = new SqlCommand(qrys5, con);
                cmd.Parameters.AddWithValue("@Id", Id);
                con.Open();
                result = cmd.ExecuteNonQuery();
                if (result == 1)
                    result = custid;
                con.Close();
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

        public int RemoveCartItem(int Id)
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
            if (result >= 1)
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
            string qry = "select distinct Name from tblStock where IsDeleted=0 and Quantity>0 order by Name ";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    product.Add(new ProductModel
                    {
                        Name = dr.GetString("Name")
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
        }*/

        public List<CustomerModelV1> GetAllCustomers(string? MobileNo)
        {
            MobileNo = MobileNo == null ? "" : MobileNo;
            List<CustomerModelV1> clist = new List<CustomerModelV1>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY Id desc) as SrNo,Id,Name,MobileNo,Address
                                                                            from tblCustomerV1  where IsDeleted=0 
			                                                                and (MobileNo like '%'+@MobileNo+'%' or @MobileNo is null ) 
                                                                            and CreatedBy =@CreatedBy";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    CustomerModelV1 c = new CustomerModelV1();
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.Name = dr["Name"].ToString();
                    c.MobileNo = dr["MobileNo"].ToString();
                    c.Address = dr["Address"].ToString();
                    clist.Add(c);
                }
            }
            con.Close();
            return clist;
        }

        /* public CustomerModelV1 GetCustomerById(int Id)
         {
             CustomerModelV1 c = new CustomerModelV1();
             string qry = @"Select Id,Name,MobileNo,Address from tblCustomerV1 where IsDeleted=0 and Id=@Id";
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
                     //con.Open();
                 }
             }
             con.Close();
             return c;
         }*/
        public CustomerModelV1 GetCustomerById(int Id, int searchText)
        {
            CustomerModelV1 c = new CustomerModelV1();

            string qry = @"Select Id,Name,MobileNo,Address from tblCustomerV1 where IsDeleted=0 and Id=@Id";
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
                    c.OrderDetails = GetOrders(c.Id, searchText);
                    //con.Open();
                }
            }
            con.Close();

            return c;
        }

        public List<OrderModel> GetOrders(int Id, int searchText)
        {
            con.Close();
            List<OrderModel> clist = new List<OrderModel>();
            string qry1 = @"Select ROW_NUMBER() OVER(ORDER BY o.Id desc) as SrNo,o.Id,Name,CustId,
			                                            TotalQuantity,TotalAmt,TotalDiscount,PayAmt,RemarkId,
                                                        Remark  from tblOrder o 
			                                            left join tblRemark r on r.Id=o.RemarkId 
			                                             where o.CustId=@Id and o.IsDeleted=0 and (o.Id=@searchText or @searchText=0)";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@searchText", searchText);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    OrderModel c = new OrderModel();
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.Name = dr["Name"].ToString();
                    c.CustId = Convert.ToInt32(dr["CustId"]);
                    c.TotalQuantity = Convert.ToInt32(dr["TotalQuantity"]);
                    c.TotalAmt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.PayAmt = (float)Convert.ToDouble(dr["PayAmt"]);
                    c.RemarkId = Convert.ToInt32(dr["RemarkId"]);
                    c.Remark = dr["Remark"].ToString();
                    clist.Add(c);
                }
            }
            //con.Close();
            return clist;
        }
        public int UpdateCustomerDetails(CustomerModelV1 customer)
        {
            int result = 0;
            int id = 0;
            string qry1 = @"Select Id from tblCustomerV1 where MobileNo=@MobileNo and Id!=@Id and IsDeleted=0";
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
                string qry = @"update tblCustomerV1 set Name=@Name,MobileNo=@MobileNo,Address=@Address,
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

        public int CancelCustomer(int Id)
        {
            int id = 0;
            string qry1 = @"Select top 1 Id from tblOrder where CustId=@Id  and IsDeleted=0 ";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    id = Convert.ToInt32(dr["Id"]);
                }
            }
            con.Close();
            if (id == 0)
            {
                string qry = "update tblCustomerV1 set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
                cmd = new SqlCommand(qry, con);
                var ModifiedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();/*
            if (result >= 1)
            {
                string qry1 = "update tblCart set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where CustId=@CustId";
                cmd = new SqlCommand(qry1, con);

                cmd.Parameters.AddWithValue("@CustId", Id);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }*/
            }
            return id;
        }
        public int CancelOrder(int Id)
        {
            string qry = "update tblOrderItem set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where OrderId=@Id";
            cmd = new SqlCommand(qry, con);
            var ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            string qry1 = "update tblOrder set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry1, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
            con.Open();
            result = cmd.ExecuteNonQuery();
            con.Close();

            return result;
        }
        public int AddCustomer(CustomerModelV1 customer)
        {

            string qry1 = @"select Id from tblCustomerV1 Where MobileNo=@MobileNo and IsDeleted=0;";

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
            string qry = @"insert into tblCustomerV1(Name,MobileNo,Address,CreatedBy,CreatedDate,IsDeleted) 
                            values(@Name,@MobileNo,@Address,@CreatedBy,@CreatedDate,0);";
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

        public int AddOrder(int CustId)
        {
            int result = 0;
            string qry1 = @"select Id from tblOrder Where CustId=@CustId and RemarkId=1 and IsDeleted=0;";

            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@CustId", CustId);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    result = Convert.ToInt32(dr["Id"]);
                }
                /*if (customer.Id != 0)
                {
                    return -1;
                }*/
            }
            con.Close();
            if (result == 0)
            {
                string qry3 = @"select Name from tblCustomerV1 Where Id=@CustId and IsDeleted=0;";
                var custname = "";
                cmd = new SqlCommand(qry3, con);
                cmd.Parameters.AddWithValue("@CustId", CustId);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        custname = (dr["Name"].ToString());
                    }
                }
                con.Close();
                Random random = new Random();
                int x = random.Next(1000);
                string Name = custname + x;
                string qry = @"insert into tblOrder(Name,CustId,TotalQuantity,TotalAmt,TotalDiscount,PayAmt,RemarkId,CreatedBy,CreatedDate,IsDeleted) 
                            values(@Name,@CustId,0,0,0,0,1,@CreatedBy,@CreatedDate,0)
                            select cast(scope_identity() as int) as Id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@CustId", CustId);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        result = Convert.ToInt32(dr["Id"]);
                    }
                }
                con.Close();
            }
            return result;
        }

        public OrderModel GetOrderById(int Id)
        {
            OrderModel c = new OrderModel();
            string qry = @"Select o.Id,Name,CustId,TotalQuantity
                            ,TotalAmt,TotalDiscount,PayAmt,RemarkId,
                            r.Remark,o.CreatedDate as OrderDate from tblOrder o left join tblRemark r 
			                on o.RemarkId=r.Id where o.IsDeleted=0 
			                and o.Id=@Id";
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
                    c.CustId = Convert.ToInt32(dr["CustId"]);
                    c.TotalQuantity = Convert.ToInt32(dr["TotalQuantity"]);
                    c.TotalAmt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    c.TotalDiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    c.PayAmt = (float)Convert.ToDouble(dr["PayAmt"]);
                    c.RemarkId = Convert.ToInt32(dr["RemarkId"]);
                    c.Remark = dr["Remark"].ToString();
                    c.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    c.OrderItem = GetCartItems(c.Id);
                    //con.Open();
                }
            }
            con.Close();
            return c;
        }

        public List<OrderItemModel> GetCartItems(int Id)
        {
            con.Close();
            List<OrderItemModel> clist = new List<OrderItemModel>();
            string qry1 = @"Select ROW_NUMBER() OVER(ORDER BY oi.Id desc) as SrNo,oi.Id,oi.ProductId,u.Unit,oi.OrderId,
			    s.Name,s.Price,s.DiscountId,d.DiscountPerc,oi.Quantity,oi.TotalAmt,
                oi.TotalDiscount,oi.NetAmt,oi.Discount from tblOrderItem oi
			    left join tblStock s on s.Id=oi.ProductId 
			    left join tblDiscount d on s.DiscountId=d.Id
			    left join tblUnit u on s.UnitId=u.Id
			        where oi.OrderId=@Id and oi.IsDeleted=0";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    OrderItemModel c = new OrderItemModel();
                    c.SrNo = Convert.ToInt32(dr["SrNo"]);
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.ProductId = Convert.ToInt32(dr["ProductId"]);
                    c.Unit = dr["Unit"].ToString();
                    c.Name = dr["Name"].ToString();
                    c.Price = (float)Convert.ToDouble(dr["Price"]);
                    c.DiscountId = Convert.ToInt32(dr["DiscountId"]);
                    c.OrderId = Convert.ToInt32(dr["OrderId"]);
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

        public int AddProduct(OrderItemModel cart, int CustId)
        {
            int result = 0;
            if (cart.ProductId == 0)
            {
                string query = @"select Id from tblStock where Name=@Name  and UnitId=@UnitId and IsDeleted=0";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", cart.Name);
                cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        cart.ProductId = Convert.ToInt32(dr["Id"]);
                    }

                }
            }
            con.Close();
            string query2 = @"select 1 from tblStock where Id=@ProductId  and UnitId=@UnitId and IsDeleted=0 and Quantity>=@Quantity";
            cmd = new SqlCommand(query2, con);
            cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
            cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            con.Open();
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                con.Close();
                string query1 = @"select TotalAmt,TotalDiscount,PayAmt,TotalQuantity from tblOrder where Id=@Id and RemarkId=1 and IsDeleted=0 ";
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
                        float totalquantity = Convert.ToInt32(dr["TotalQuantity"]);
                        con.Close();
                        string querys1 = @" select Id,Quantity,Price,Discount,TotalAmt,TotalDiscount,NetAmt from tblOrderItem 
                                                                    where OrderId=@OrderId and IsDeleted=0 and ProductId=@ProductId and UnitId=@UnitId";
                        cmd = new SqlCommand(querys1, con);
                        cmd.Parameters.AddWithValue("@OrderId", CustId);
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
                                totalquantity = (totalquantity) + cart.Quantity;
                                cart.NetAmt = (cart.TotalAmt - cart.TotalDiscount) + netAmt;
                                cart.TotalDiscount = (discount * cart.Quantity) + totalDiscount;
                                cart.TotalAmt = (price * cart.Quantity) + totalAmt;
                                var qqq = cart.Quantity + quantity;
                                var qrys2 = @"update tblOrderItem set TotalAmt=@TotalAmt,TotalDiscount=@TotalDiscount,
                                    NetAmt=@NetAmt ,Quantity=@Quantity where Id=@Id";
                                cmd = new SqlCommand(qrys2, con);
                                cmd.Parameters.AddWithValue("@TotalAmt", cart.TotalAmt);
                                cmd.Parameters.AddWithValue("@TotalDiscount", cart.TotalDiscount);
                                cmd.Parameters.AddWithValue("@NetAmt", cart.NetAmt);
                                cmd.Parameters.AddWithValue("@Quantity", qqq);
                                cmd.Parameters.AddWithValue("@Id", cart.Id);
                                con.Open();
                                result = cmd.ExecuteNonQuery();
                                con.Close();

                            }
                        }
                        else
                        {
                            con.Close();
                            string qry = @"insert into tblOrderItem(OrderId,ProductId,UnitId,IsConfirmed,Quantity,Price,Discount,TotalAmt,TotalDiscount,NetAmt,CreatedBy,CreatedDate,IsDeleted)
                                      VALUES(@OrderId,@ProductId,@UnitId,0,@Quantity,0,0,0,0,0,@CreatedBy,@CreatedDate,0)
                                        select cast(scope_identity() as int) as Id;";

                            cmd = new SqlCommand(qry, con);
                            cart.CreatedDate = DateTime.Now;
                            cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
                            cmd.Parameters.AddWithValue("@OrderId", CustId);
                            cmd.Parameters.AddWithValue("@UnitId", cart.UnitId);
                            cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                            cmd.Parameters.AddWithValue("@CreatedDate", cart.CreatedDate);
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
                            var qry1 = @"update tblOrderItem set TotalAmt=@TotalAmt,TotalDiscount=@TotalDiscount,
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
                            totalquantity = totalquantity + cart.Quantity;
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

                        var qry2 = @"	update tblOrder set TotalAmt=@TotalAmt,PayAmt=@PayAmt,
                                    Totaldiscount=@Totaldiscount,TotalQuantity=@TotalQuantity where Id=@Id";
                        cmd = new SqlCommand(qry2, con);
                        cmd.Parameters.AddWithValue("@TotalAmt", subtotal);
                        cmd.Parameters.AddWithValue("@PayAmt", grandtotal);
                        cmd.Parameters.AddWithValue("@Totaldiscount", totaldiscount);
                        cmd.Parameters.AddWithValue("@TotalQuantity", totalquantity);
                        cmd.Parameters.AddWithValue("@Id", CustId);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        if (result == 1)
                            result = CustId;
                        con.Close();
                    }
                }
                con.Close();
            }
            con.Close();
            return result;
        }


        public OrderItemModel GetOrderItemById(int Id)
        {
            OrderItemModel c = new OrderItemModel();
            string qry1 = @"Select oi.Id,oi.ProductId,u.Unit,oi.OrderId,
			    s.Name,s.Price,s.DiscountId,d.DiscountPerc,oi.Quantity,oi.TotalAmt,
                oi.TotalDiscount,oi.NetAmt,oi.Discount from tblOrderItem oi
			    left join tblStock s on s.Id=oi.ProductId 
			    left join tblDiscount d on s.DiscountId=d.Id
			    left join tblUnit u on s.UnitId=u.Id
			        where oi.Id=@Id and oi.IsDeleted=0";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.OrderId = Convert.ToInt32(dr["OrderId"]);
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
                }
            }
            //con.Close();
            return c;
        }

        public int EditOrderItem(OrderItemModel order)
        {
            float totalquantity;
            float subtotal;
            float grandtotal;
            float totaldiscount;
            float totalamt = 0;
            float netamt = 0;
            float totaldisc = 0;
            int qua = 0;
            int quantity = 0;
            int productid = 0;
            int orderid = 0;
            int result = 0;
            var query = @"select Id,TotalAmt,TotalDiscount,NetAmt,OrderId,Quantity,ProductId from tblOrderItem where Id=@Id and IsDeleted=0";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", order.Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    orderid = Convert.ToInt32(dr["OrderId"]);
                    totalamt = (float)Convert.ToDouble(dr["TotalAmt"]);
                    netamt = (float)Convert.ToDouble(dr["NetAmt"]);
                    totaldisc = (float)Convert.ToDouble(dr["TotalDiscount"]);
                    qua = Convert.ToInt32(dr["Quantity"]);
                    productid = Convert.ToInt32(dr["ProductId"]);
                }
            }
            con.Close();
            int Id = order.Id;
            var qrys2 = @"update tblOrderItem set IsDeleted=1 where Id=@Id";
            cmd = new SqlCommand(qrys2, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            result = cmd.ExecuteNonQuery();
            con.Close();
            //cart.ProductId = productid;
            if (order.Name != null && (order.UnitId != null || order.UnitId != 0))
                result = AddProduct(order, orderid);
            if (result >= 1)
            {
                var countquery = "Select Quantity from tblStock where Id=@Id";
                cmd = new SqlCommand(countquery, con);
                cmd.Parameters.AddWithValue("@Id", productid);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        quantity = Convert.ToInt32(dr["Quantity"]);
                    }
                }

                con.Close();
                string query1 = @"select TotalQuantity,TotalAmt,TotalDiscount,PayAmt from tblOrder where Id=@Id and RemarkId=1 and IsDeleted=0 ";
                cmd = new SqlCommand(query1, con);
                cmd.Parameters.AddWithValue("@Id", orderid);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        totalquantity = Convert.ToInt32(dr["TotalQuantity"]);
                        subtotal = (float)Convert.ToDouble(dr["TotalAmt"]);
                        grandtotal = (float)Convert.ToDouble(dr["PayAmt"]);
                        totaldiscount = (float)Convert.ToDouble(dr["TotalDiscount"]);

                        subtotal = subtotal - totalamt;
                        grandtotal = grandtotal - netamt;
                        totaldiscount = totaldiscount - totaldisc;
                        totalquantity = totalquantity - qua;
                        quantity = quantity + qua;
                        con.Close();
                        var updatecountquery = "update tblStock set quantity=@quantity where Id=@Id";
                        cmd = new SqlCommand(updatecountquery, con);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@Id", productid);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        con.Close();

                        var qry2 = @"	update tblOrder set TotalAmt=@TotalAmt,PayAmt=@PayAmt,
                                    Totaldiscount=@Totaldiscount,TotalQuantity=@TotalQuantity where Id=@Id";
                        cmd = new SqlCommand(qry2, con);
                        cmd.Parameters.AddWithValue("@TotalAmt", subtotal);
                        cmd.Parameters.AddWithValue("@PayAmt", grandtotal);
                        cmd.Parameters.AddWithValue("@Totaldiscount", totaldiscount);
                        cmd.Parameters.AddWithValue("@TotalQuantity", totalquantity);
                        cmd.Parameters.AddWithValue("@Id", orderid);
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                        if (result == 1)
                            result = orderid;
                        con.Close();

                    }
                }
                con.Close();


            }
            else
            {
                var qrys5 = @"update tblOrderItem set IsDeleted=0 where Id=@Id";
                cmd = new SqlCommand(qrys5, con);
                cmd.Parameters.AddWithValue("@Id", Id);
                con.Open();
                result = cmd.ExecuteNonQuery();
                if (result == 1)
                    result = orderid;
                con.Close();
            }
            return result;
        }
        public int PayAmt(int Id)
        {

            string qry = @"update tblOrderItem set IsConfirmed=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate
                                    where OrderId=@OrderId and IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            var ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@OrderId", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result >= 1)
            {
                string qry1 = @"update tblOrder set RemarkId=2,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate
                                    where Id=@Id and IsDeleted=0";
                cmd = new SqlCommand(qry1, con);
                cmd.Parameters.AddWithValue("@Id", Id);
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
            string qry = "select distinct Name from tblStock where IsDeleted=0 and Quantity>0 order by Name ";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    product.Add(new ProductModel
                    {
                        Name = dr.GetString("Name")
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
