using DMartMallSoftware.Models;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DMartMallSoftware.DAL
{
    public class DemandDAL : HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public DemandDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));

        }

        public int AddDemand(DemandModel demand)
        {
            int result = 0;
            DemandModel staff = new DemandModel();
            string qry = "select Id,Quantity from tblDemand where Name=@Name and UnitId=@UnitId and DealerId=@DealerId and StatusId=1 and IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Name", demand.Name);
            cmd.Parameters.AddWithValue("@UnitId", demand.UnitId);
            cmd.Parameters.AddWithValue("@DealerId", demand.DealerId);
            staff.Id = 0;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    staff.Id = dr.GetInt32("Id");
                    staff.Quantity = dr.GetInt32("Quantity");
                    break;
                }


            }
            con.Close();
            if (staff.Id != 0)
            {
                staff.Quantity = staff.Quantity + demand.Quantity;
                string qry1 = @"  update tblDemand set Quantity=@Quantity,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate
                                  where Id=@Id";
                cmd = new SqlCommand(qry1, con);
                demand.ModifiedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id", staff.Id);
                cmd.Parameters.AddWithValue("@Quantity", staff.Quantity);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", demand.ModifiedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                string qry1 = @"  insert into tblDemand(Name,UnitId,Quantity,Price,Total,DealerId,PayStatusId,StatusId,CreatedBy,CreatedDate,ModifiedDate,IsDeleted) 
                                values(@Name,@UnitId,@Quantity,0,0,@DealerId,1,1,@CreatedBy,@CreatedDate,@CreatedDate,0)";
                cmd = new SqlCommand(qry1, con);
                demand.CreatedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Name", demand.Name);
                cmd.Parameters.AddWithValue("@UnitId", demand.UnitId);
                cmd.Parameters.AddWithValue("@Quantity", demand.Quantity);
                cmd.Parameters.AddWithValue("@DealerId", demand.DealerId);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@CreatedDate", demand.CreatedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
                result = 2;
            }
            return result;
        }

        public List<DemandModel> ShowAllDemandsForStaff(string Name, int dealer, int status)
        {
            Name = Name == null ? "" : Name;
            List<DemandModel> plist = new List<DemandModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY d.Id desc) as SrNo,d.Id,d.Name,u.Unit,d.Quantity
                            ,d.Price,d.Total,s.Name as Dealer,d.PayStatusId,ps.PayStatus,d.StatusId,st.Status,d.CreatedDate,d.ModifiedDate
                            from tblDemand d left join tblUnit u on d.UnitId=u.Id and u.IsDeleted=0
			                left join tblStaff s on d.DealerId=s.Id and s.IsDeleted=0
			                left join tblStatus st on d.StatusId=st.Id and st.IsDeleted=0
			                left join tblPayStatus ps on d.PayStatusId=ps.Id and ps.IsDeleted=0
			                 where
							(@Name='' or d.Name  like '%'+@Name+'%') and
							(@dealer=0 or d.DealerId =@dealer) and
							(@status=0 or d.StatusId =@status) and
                            d.IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            SqlParameter par = new SqlParameter();

            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@dealer", dealer));
            cmd.Parameters.Add(new SqlParameter("@status", status));
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
                    p.Dealer = dr["Dealer"].ToString();
                    p.PayStatusId = Convert.ToInt32(dr["PayStatusId"]);
                    p.PayStatus = dr["PayStatus"].ToString();
                    p.StatusId = Convert.ToInt32(dr["StatusId"]);
                    p.Status = dr["Status"].ToString();
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
            string qry = @"Select d.Id,d.Name,d.UnitId,u.Unit,d.Quantity
                            ,d.Price,d.Total,s.Name as Dealer,d.PayStatusId,ps.PayStatus,d.StatusId,st.Status,d.CreatedDate,d.ModifiedDate
                            from tblDemand d left join tblUnit u on d.UnitId=u.Id and u.IsDeleted=0
			                left join tblStaff s on d.DealerId=s.Id and s.IsDeleted=0
			                left join tblStatus st on d.StatusId=st.Id and st.IsDeleted=0
			                left join tblPayStatus ps on d.PayStatusId=ps.Id and ps.IsDeleted=0
			                 where d.IsDeleted=0 and d.Id=@Id";
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
                    p.Unit = dr["Unit"].ToString();
                    p.UnitId = Convert.ToInt32(dr["UnitId"]);
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = (float)Convert.ToDouble(dr["Price"]);
                    p.Total = (float)Convert.ToDouble(dr["Total"]);
                    p.Dealer = dr["Dealer"].ToString();
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

        public int DeleteDemand(int Id)
        {
            string qry = "update tblDemand set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int RejectOffer(int Id)
        {
            string qry = "update tblDemand set StatusId=9,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int ConfirmByAdmin(int Id)
        {
            string qry = "update tblDemand set PayStatusId=2,StatusId=4,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }


        public int NotDelinered(int Id)
        {
            string qry = "update tblDemand set StatusId=11,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }
        public int DeliveredByAdmin(int Id)
        {
            string qry = "update tblDemand set StatusId=8,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int AddToStock(StockModel stock)
        {
            int result = 0;
            StockModel stock1 = new StockModel();
            string qry = "select Id,Quantity from tblStock where Name=@Name and UnitId=@UnitId and IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Name", stock.Name);
            cmd.Parameters.AddWithValue("@UnitId", stock.UnitId);
            stock1.Id = 0;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    stock1.Id = dr.GetInt32("Id");
                    stock1.Quantity = dr.GetInt32("Quantity");
                    break;
                }


            }
            con.Close();
            if (stock1.Id != 0)
            {
                stock1.Quantity = stock1.Quantity + stock.Quantity;
                string qry1 = @"  update tblStock set Quantity=@Quantity,Price=@Price,DiscountId=@DiscountId,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate
                                  where Id=@Id";
                cmd = new SqlCommand(qry1, con);
                stock.ModifiedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id", stock1.Id);
                cmd.Parameters.AddWithValue("@Quantity", stock1.Quantity);
                cmd.Parameters.AddWithValue("@Price", stock.Price);
                cmd.Parameters.AddWithValue("@DiscountId", stock.DiscountId);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", stock.ModifiedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                string qry1 = @"  insert into tblStock(Name,UnitId,Quantity,Price,DiscountId,CreatedBy,CreatedDate,IsDeleted) 
                                values(@Name,@UnitId,@Quantity,@Price,@DiscountId,@CreatedBy,@CreatedDate,0)";
                cmd = new SqlCommand(qry1, con);
                stock.CreatedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Name", stock.Name);
                cmd.Parameters.AddWithValue("@UnitId", stock.UnitId);
                cmd.Parameters.AddWithValue("@Quantity", stock.Quantity);
                cmd.Parameters.AddWithValue("@Price", stock.Price);
                cmd.Parameters.AddWithValue("@DiscountId", stock.DiscountId);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@CreatedDate", stock.CreatedDate);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            if (result == 1)
            {
                string qry2 = "update tblDemand set StatusId=12,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
                cmd = new SqlCommand(qry2, con);

                cmd.Parameters.AddWithValue("@Id", stock.Id);
                cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
                cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            return result;
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

        public List<UnitModel> LoadUnit1(int un)
        {
            List<UnitModel> unit = new List<UnitModel>();
            string qry = "select * from tblUnit where IsDeleted=0 and Id!=@Id";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Id", un);
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

        public List<StaffModel> LoadDealer()
        {
            List<StaffModel> staff = new List<StaffModel>();
            string qry = "select * from tblStaff where UserTypeId=6 and IsDeleted=0 and IsConfirmed=1";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    staff.Add(new StaffModel
                    {
                        Id = dr.GetInt32("Id"),
                        Name = dr.GetString("Name")
                    });
                }


            }
            con.Close();
            return staff;
        }

        public List<DiscountModel> LoadDisc()
        {
            List<DiscountModel> disc = new List<DiscountModel>();
            string qry = "select * from tblDiscount where IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    disc.Add(new DiscountModel
                    {
                        Id = dr.GetInt32("Id"),
                        DiscountPerc = (float)dr.GetDouble("DiscountPerc")
                    });
                }


            }
            con.Close();
            return disc;
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
