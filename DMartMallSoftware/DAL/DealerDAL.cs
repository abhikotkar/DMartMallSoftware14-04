using DMartMallSoftware.Models;
using System.Data;
using System.Data.SqlClient;

namespace DMartMallSoftware.DAL
{
    public class DealerDAL : HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public DealerDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));

        }

        public List<DemandModel> ShowAllDemandsForDealer(string Name, int status)
        {
            Name = Name == null ? "" : Name;
            List<DemandModel> plist = new List<DemandModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY d.Id desc) as SrNo,d.Id,d.Name,u.Unit,d.Quantity
                            ,d.Price,d.Total,d.PayStatusId,ps.PayStatus,d.StatusId,st.Status,d.CreatedDate,d.ModifiedDate
                            from tblDemand d left join tblUnit u on d.UnitId=u.Id and u.IsDeleted=0
			                left join tblStatus st on d.StatusId=st.Id and st.IsDeleted=0
			                left join tblPayStatus ps on d.PayStatusId=ps.Id and ps.IsDeleted=0
			                 where
							(@Name='' or d.Name  like '%'+@Name+'%') and
							(@status=0 or d.StatusId =@status) and
                            d.IsDeleted=0 and d.DealerId=@DealerId and d.StatusId!=3";
            cmd = new SqlCommand(qry, con);
            SqlParameter par = new SqlParameter();

            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@status", status));
            cmd.Parameters.Add(new SqlParameter("@DealerId", HttpContext.Session.GetString("Id")));
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    DemandModel p = new DemandModel();
                    p.SrNo = Convert.ToInt32(dr["SrNo"]);
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Name = dr["Name"].ToString();
                    p.Unit = dr["Unit"].ToString();
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = (float)Convert.ToDouble(dr["Price"]);
                    p.Total = (float)Convert.ToDouble(dr["Total"]);
                    p.PayStatusId = Convert.ToInt32(dr["PayStatusId"]);
                    p.PayStatus = dr["PayStatus"].ToString();
                    p.StatusId = Convert.ToInt32(dr["StatusId"]);
                    if (p.StatusId == 12)
                    {
                        p.Status = "Delivered";
                    }
                    else
                    {
                        p.Status = dr["Status"].ToString();
                    }
                    p.CreatedDate = (DateTime)dr["CreatedDate"];
                    p.ModifiedDate = (DateTime)dr["ModifiedDate"];
                    plist.Add(p);
                }
            }
            con.Close();
            return plist;
        }

        public DemandModel GetDemandById(int Id)
        {
            DemandModel p = new DemandModel();
            string qry = @"Select d.Id,d.Name,u.Unit,d.Quantity
                            ,d.Price,d.Total,d.PayStatusId,ps.PayStatus,d.StatusId,st.Status,d.CreatedDate,d.ModifiedDate
                            from tblDemand d left join tblUnit u on d.UnitId=u.Id and u.IsDeleted=0
			                left join tblStatus st on d.StatusId=st.Id and st.IsDeleted=0
			                left join tblPayStatus ps on d.PayStatusId=ps.Id and ps.IsDeleted=0
			                 where d.IsDeleted=0 and d.DealerId=@DealerId and d.Id=@Id";
            cmd = new SqlCommand(qry, con);
            SqlParameter par = new SqlParameter();

            cmd.Parameters.Add(new SqlParameter("@DealerId", HttpContext.Session.GetString("Id")));
            cmd.Parameters.Add(new SqlParameter("@Id", Id));
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Name = dr["Name"].ToString();
                    p.Unit = dr["Unit"].ToString();
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = (float)Convert.ToDouble(dr["Price"]);
                    p.Total = (float)Convert.ToDouble(dr["Total"]);
                    p.PayStatusId = Convert.ToInt32(dr["PayStatusId"]);
                    p.PayStatus = dr["PayStatus"].ToString();
                    p.StatusId = Convert.ToInt32(dr["StatusId"]);
                    p.Status = dr["Status"].ToString();
                    p.CreatedDate = (DateTime)dr["CreatedDate"];
                    p.ModifiedDate = (DateTime)dr["ModifiedDate"];
                }
            }
            con.Close();
            return p;
        }

        public int ConfirmByDealer(int Id)
        {
            string qry = "update tblDemand set StatusId=5,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int PayAgain(int Id)
        {
            string qry = "update tblDemand set PayStatusId=1, StatusId=10,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int ShippingByDealer(int Id)
        {
            string qry = "update tblDemand set StatusId=6,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int DeliveredByDealer(int Id)
        {
            string qry = "update tblDemand set StatusId=7,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int DeleteDemand(int Id)
        {
            string qry = "update tblDemand set StatusId=3,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int AcceptRequest(DemandModel demand)
        {
            var quantity = 0;
            string qry1 = @"Select Quantity from tblDemand where IsDeleted=0 and Id=@Id";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Id", demand.Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    quantity = Convert.ToInt32(dr["Quantity"]);
                    break;
                }
            }
            con.Close();
            demand.Total = demand.Price * quantity;
            string qry = @"update tblDemand set Price=@Price,Total=@Total,StatusId=2,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);
            demand.ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Id", demand.Id);
            cmd.Parameters.AddWithValue("@Price", demand.Price);
            cmd.Parameters.AddWithValue("@Total", demand.Total);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", demand.ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public List<StatusModel> Loadstatus()
        {
            List<StatusModel> disc = new List<StatusModel>();
            string qry = "select * from tblStatus where IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    disc.Add(new StatusModel
                    {
                        Id = dr.GetInt32("Id"),
                        Status = dr.GetString("Status")
                    });
                }


            }
            con.Close();
            return disc;
        }
    }
}
