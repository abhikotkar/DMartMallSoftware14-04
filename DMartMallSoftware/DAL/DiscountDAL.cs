using DMartMallSoftware.Models;
using System.Data.SqlClient;

namespace DMartMallSoftware.DAL
{
    public class DiscountDAL : HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public DiscountDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));

        }

        public List<DiscountModel> GetDiscountType()
        {
            List<DiscountModel> dlist = new List<DiscountModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY d.Id desc) as SrNo,d.Id,d.DiscountPerc,s.Name as AddedBy from tblDiscount d
                                                        left join tblStaff s on d.CreatedBy=s.Id and s.IsDeleted=0 where d.IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    DiscountModel d = new DiscountModel();
                    d.SrNo = Convert.ToInt32(dr["SrNo"]);
                    d.Id = Convert.ToInt32(dr["Id"]);
                    d.AddedBy = dr["AddedBy"].ToString();
                    d.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                    dlist.Add(d);
                }
            }
            con.Close();
            return dlist;
        }
        public int AddDiscountType(DiscountModel discount)
        {
            int result = 0;
            int id = 0;
            string qry1 = @"Select Id from tblDiscount where DiscountPerc=@DiscountPerc and IsDeleted=0 ";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@DiscountPerc",discount.DiscountPerc);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr["Id"]);
                    break;
                }
            }
            con.Close();
            if (id == 0)
            {
                string qry = @"insert into tblDiscount(DiscountPerc,CreatedBy,CreatedDate,IsDeleted) 
                            values(@DiscountPerc,@CreatedBy,@CreatedDate,0);";
                cmd = new SqlCommand(qry, con);
                discount.CreatedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@DiscountPerc", discount.DiscountPerc);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@CreatedDate", discount.CreatedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            return result;
        }

       
        public DiscountModel GetDiscountTypeById(int Id)
        {
            DiscountModel d = new DiscountModel();
            string qry = @"Select Id,DiscountPerc from tblDiscount where Id=@Id and IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    d.Id = Convert.ToInt32(dr["Id"]);
                    d.DiscountPerc = (float)Convert.ToDouble(dr["DiscountPerc"]);
                }
            }
            con.Close();
            return d;
        }

        public int UpdateDiscountType(DiscountModel discount)
        {
            int result = 0;
            int id = 0;
            string qry1 = @"Select Id from tblDiscount where DiscountPerc=@DiscountPerc and IsDeleted=0 ";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@DiscountPerc", discount.DiscountPerc);
            cmd.Parameters.AddWithValue("@Id", discount.Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr["Id"]);
                    break;
                }
            }
            con.Close();
            if (id == 0)
            {
                string qry = @"update tblDiscount set DiscountPerc=@DiscountPerc,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
                cmd = new SqlCommand(qry, con);
                discount.ModifiedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id", discount.Id);
                cmd.Parameters.AddWithValue("@DiscountPerc", discount.DiscountPerc);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", discount.ModifiedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            return result;
        }

        public int DeleteDiscountType(int Id)
        {
            string qry = "update tblDiscount set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
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
