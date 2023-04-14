using DMartMallSoftware.Models;
using System.Data;
using System.Data.SqlClient;

namespace DMartMallSoftware.DAL
{
    public class StockDAL : HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public StockDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));

        }

        public List<StockModel> GetStock(string Name)
        {
            Name = Name == null ? "" : Name;
            List<StockModel> plist = new List<StockModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY s.Id desc) as SrNo,s.Id,s.Name,s.Price,s.Quantity,u.Unit,d.DiscountPerc
                            from tblStock s left join tblDiscount d on s.DiscountId=d.Id and d.IsDeleted=0
			                left join tblUnit u on s.UnitId=u.Id and u.IsDeleted=0
			                 where s.IsDeleted=0 and s.Name like '%'+@Name+'%'";
            cmd = new SqlCommand(qry, con); 
            SqlParameter par = new SqlParameter();

            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    StockModel p = new StockModel();
                    p.SrNo = Convert.ToInt32(dr["SrNo"]);
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Name = dr["Name"].ToString();
                    p.Price = (float)Convert.ToDouble(dr["Price"]);
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Unit = dr["Unit"].ToString();
                    p.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                    plist.Add(p);
                }
            }
            con.Close();
            return plist;
        }

        public StockModel GetStockById(int Id)
        {
            StockModel p = new StockModel();
            string qry = @"Select s.Id,s.Name,s.Price,s.Quantity,u.Unit,d.DiscountPerc
                            from tblStock s left join tblDiscount d on s.DiscountId=d.Id and d.IsDeleted=0
			                left join tblUnit u on s.UnitId=u.Id and u.IsDeleted=0
			                 where s.IsDeleted=0 and s.Id=@Id";
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
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Unit = dr["Unit"].ToString();
                    p.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                }
            }
            con.Close();
            return p;
        }

        public int UpdateStock(StockModel stock)
        {
            string qry = @"update tblStock set Name=@Name,Price=@Price,Quantity=@Quantity,UnitId=@UnitId,DiscountId=@DiscountId,
                            ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);
            stock.ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Id", stock.Id);
            cmd.Parameters.AddWithValue("@Name", stock.Name);
            cmd.Parameters.AddWithValue("@Price", stock.Price);
            cmd.Parameters.AddWithValue("@Quantity", stock.Quantity);
            cmd.Parameters.AddWithValue("@UnitId", stock.UnitId);
            cmd.Parameters.AddWithValue("@DiscountId", stock.DiscountId);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", stock.ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int DeleteStock(int Id)
        {
            string qry = "update tblStock set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public List<DiscountModel> LoadDiscount()
        {
            List<DiscountModel> discount = new List<DiscountModel>();
            string qry = "select * from tblDiscount where IsDeleted=0";
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
                    });
                }


            }
            con.Close();
            return discount;
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
                        Unit =dr.GetString("Unit")
                    });
                }


            }
            con.Close();
            return unit;
        }
    }
}
