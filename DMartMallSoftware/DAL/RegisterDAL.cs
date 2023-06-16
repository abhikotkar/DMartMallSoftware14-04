using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;

namespace DMartMallSoftware.DAL
{
    public class RegisterDAL : HttpContextAccessor
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;




        public RegisterDAL(IConfiguration configuration)
        {
            con = new SqlConnection(configuration.GetValue<string>("ConnectionStrings:SqlConnection"));
            //httpContext= httpContextAccessor;
        }

        //Login Staff
        public StaffModel UserLogin(StaffModel staff)
        {
            StaffModel s = new StaffModel();
            string qry = "select * from tblStaff where Email=@Email and IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Email", staff.Email);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    s.Id = Convert.ToInt32(dr["Id"]);
                    s.Name = dr["Name"].ToString();
                    s.Email = dr["Email"].ToString();
                    s.Password = dr["Password"].ToString();
                    s.UserTypeId = Convert.ToInt32(dr["UserTypeId"]);
                    s.IsConfirmed = (bool)dr["IsConfirmed"];
                }
            }
            con.Close();
            return s;
        }
        //Add staff members
        public int AddStaff(StaffModel staff)
        {
            var result = 0;
            var id = 0;
            string qry1 = "select Id from tblStaff where (Email=@Email or MobileNo=@MobileNo) and IsDeleted=0";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Email", staff.Email);
            cmd.Parameters.AddWithValue("@MobileNo", staff.MobileNo);
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
                result = 2;
                if (staff.Password == staff.ConfirmPassword)
                {
                    string qry = @"insert into tblStaff(Name,Email,GenderId,MobileNo,Address,UserTypeId,QuetionId,Answer,Password,Salary,IsConfirmed,CreatedDate,IsDeleted) 
                            values(@Name,@Email,@GenderId,@MobileNo,@Address,@UserTypeId,@QuetionId,@Answer,@Password,0,0,@CreatedDate,0);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                    cmd = new SqlCommand(qry, con);
                    staff.CreatedDate = DateTime.Now;
                    cmd.Parameters.AddWithValue("@Name", staff.Name);
                    cmd.Parameters.AddWithValue("@Email", staff.Email);
                    cmd.Parameters.AddWithValue("@GenderId", staff.GenderId);
                    cmd.Parameters.AddWithValue("@MobileNo", staff.MobileNo);
                    cmd.Parameters.AddWithValue("@Address", staff.Address);
                    cmd.Parameters.AddWithValue("@UserTypeId", staff.UserTypeId);
                    cmd.Parameters.AddWithValue("@QuetionId", staff.QuetionId);
                    cmd.Parameters.AddWithValue("@Answer", staff.Answer);
                    cmd.Parameters.AddWithValue("@Password", staff.Password);
                    cmd.Parameters.AddWithValue("@CreatedDate", staff.CreatedDate);
                    //new SqlCommand("select Id from tbl_UserType where UserType=@UserType", con).Parameters.AddWithValue("@UserType", staff.UserType));
                    con.Open();
                    result = (int)cmd.ExecuteScalar();
                    con.Close();
                    string qry2 = @"update tblStaff set CreatedBy=@Id,ModifiedBy=@Id where Id=@Id";
                    cmd = new SqlCommand(qry2, con);
                    cmd.Parameters.AddWithValue("@Id", result);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                    return result;
                }
            }
            return result;
        }



        //Show all Staff members
        public List<StaffModel> GetStaff(string Name, int TextId, string IsConfirmed)
        {
            Name = Name == null ? "" : Name;
            TextId = TextId == null ? 0 : TextId;
            IsConfirmed = IsConfirmed == "No" ? "false" : IsConfirmed == "Yes" ? "true" : "";
            List<StaffModel> plist = new List<StaffModel>();
            string qry = @"Select ROW_NUMBER() OVER(ORDER BY s.Id desc) as SrNo,s.Id,s.Name,s.Email,s.MobileNo,g.Gender,s.Address,
                            u.UserType,s.IsConfirmed,st.Name as AprovedBy,s.CreatedDate as JoinDate
			                from tblStaff s 
							join tblStaff st on st.Id=s.ModifiedBy and st.IsDeleted=0
			                left join tblUserType u on s.UserTypeId=u.Id and u.IsDeleted=0
                            left join tblGender g on s.GenderId=g.Id
			                where 
							(@Name='' or s.Name  like '%'+@Name+'%') and
							(@typeId=0 or u.Id =@typeId) and
                            (@IsConfirmed='' or s.IsConfirmed =@IsConfirmed) and
                            s.UserTypeId!=5  and
                            s.Id!=@Id
							and s.IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            SqlParameter par = new SqlParameter();

            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@Id", HttpContext.Session.GetString("Id")));
            cmd.Parameters.Add(new SqlParameter("@typeId", TextId));
            cmd.Parameters.Add(new SqlParameter("@IsConfirmed", IsConfirmed));
            //cmd.Parameters.AddWithValue("@textSearch", textSearch);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    StaffModel p = new StaffModel();
                    p.SrNo = Convert.ToInt32(dr["SrNo"]);
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Name = dr["Name"].ToString();
                    p.Email = dr["Email"].ToString();
                    p.MobileNo = dr["MobileNo"].ToString();
                    p.Gender = dr["Gender"].ToString();
                    p.Address = dr["Address"].ToString();
                    p.UserType = dr["UserType"].ToString();
                    p.AprovedBy = dr["AprovedBy"].ToString();
                    p.IsConfirmed = (bool)dr["IsConfirmed"];
                    p.JoinDate = (DateTime)dr["JoinDate"];
                    plist.Add(p);
                }
            }
            con.Close();
            return plist;
        }

        //Show staff member by Id
        public StaffModel GetStaffById(int Id)
        {
            StaffModel s = new StaffModel();

            string qry = @"select s.Id,s.Name,s.Email,s.MobileNo,g.Gender,s.Address,u.UserType,s.Salary,s.IsConfirmed,s.Answer,st.Name as AprovedBy from tblStaff s 
							    join tblStaff st on st.Id=s.ModifiedBy and st.IsDeleted=0
                                left join tblUserType u on s.UserTypeId=u.Id and u.IsDeleted=0
                                left join tblGender g on s.GenderId=g.Id
                                where s.Id=@Id and s.IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    s.Id = Convert.ToInt32(dr["Id"]);
                    s.Name = dr["Name"].ToString();
                    s.Email = dr["Email"].ToString();
                    s.MobileNo = dr["MobileNo"].ToString();
                    s.Gender = dr["Gender"].ToString();
                    s.AprovedBy = dr["AprovedBy"].ToString();
                    s.Address = dr["Address"].ToString();
                    s.UserType = dr["UserType"].ToString();
                    s.Answer = dr["Answer"].ToString();
                    s.Salary = (float)Convert.ToDouble(dr["Salary"]);
                    s.IsConfirmed = (bool)dr["IsConfirmed"];
                }
            }
            con.Close();
            return s;
        }

        public int UpdateStaff(StaffModel staff)
        {
            string qry = @"update tblStaff set UserTypeId=@UserTypeId,Salary=@Salary,IsConfirmed=@IsConfirmed,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);
            staff.ModifiedDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@Id", staff.Id);
            cmd.Parameters.AddWithValue("@Salary", staff.Salary);
            cmd.Parameters.AddWithValue("@UserTypeId", staff.UserTypeId);
            cmd.Parameters.AddWithValue("@IsConfirmed", staff.IsConfirmed);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", staff.ModifiedDate);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        public int UpdateMyProfile(StaffModel staff)
        {
            int result = 0;
            var id = 0;
            string qry1 = "select 1 from tblStaff where (Email=@Email or MobileNo=@MobileNo) and IsDeleted=0 and Id!=Id";
            cmd = new SqlCommand(qry1, con);
            cmd.Parameters.AddWithValue("@Email", staff.Email);
            cmd.Parameters.AddWithValue("@MobileNo", staff.MobileNo);
            cmd.Parameters.AddWithValue("@Id", staff.Id);
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
                string qry = @"update tblStaff set Name=@Name,Email=@Email,MobileNo=@MobileNo,Address=@Address,QuetionId=@QuetionId,Answer=@Answer
                             where Id=@Id and IsDeleted=0";
                cmd = new SqlCommand(qry, con);
                staff.ModifiedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id", staff.Id);
                cmd.Parameters.AddWithValue("@Name", staff.Name);
                cmd.Parameters.AddWithValue("@Email", staff.Email);
                cmd.Parameters.AddWithValue("@MobileNo", staff.MobileNo);
                cmd.Parameters.AddWithValue("@Address", staff.Address);
                cmd.Parameters.AddWithValue("@QuetionId", staff.QuetionId);
                cmd.Parameters.AddWithValue("@Answer", staff.Answer);
                con.Open();
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            return result;

        }

        public int DeleteMember(int Id)
        {
            string qry = "update tblStaff set IsDeleted=1,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate where Id=@Id";
            cmd = new SqlCommand(qry, con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ModifiedBy", HttpContext.Session.GetString("Id"));
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }




        //check answer

        public int CheckAns(StaffModel staff)
        {
            int id = 0;
            string qry = @"select Id from tblStaff where Email=@Email and QuetionId=@QuetionId and Answer=@Answer and IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Email", staff.Email);
            cmd.Parameters.AddWithValue("@QuetionId", staff.QuetionId);
            cmd.Parameters.AddWithValue("@Answer", staff.Answer);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr["Id"]);
                }
            }
            con.Close();
            return id;
        }



        public int forgate(StaffModel staff)
        {
            int result = 0;
            if (staff.Password == staff.ConfirmPassword)
            {

                string qry = @"update tblStaff set Password=@Password where Id=@Id and IsDeleted=0";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Password", staff.Password);
                cmd.Parameters.AddWithValue("@Id", staff.Id);
                con.Open();
                result = cmd.ExecuteNonQuery();

                con.Close();

            }
            return result;
        }


        public int ChangePass(StaffModel staff)
        {
            int id = 0;
            string qry = @"select Id from tblStaff where Password=@Password and Id=@Id and IsDeleted=0 ";
            cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@Password", staff.OldPassword);
            cmd.Parameters.AddWithValue("@Id", staff.Id);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr["Id"]);
                }
            }
            con.Close();
            int result = 0;
            if (id == staff.Id)
            {
                result = 2;
                if (staff.Password == staff.ConfirmPassword)
                {

                    string qry1 = @"update tblStaff set Password=@Password where Id=@Id and IsDeleted=0";
                    cmd = new SqlCommand(qry1, con);
                    cmd.Parameters.AddWithValue("@Password", staff.Password);
                    cmd.Parameters.AddWithValue("@Id", staff.Id);
                    con.Open();
                    result = cmd.ExecuteNonQuery();

                    con.Close();

                }
            }
            return result;
        }

        //Load userType,gender,quetion for dropdown

        public List<UserTypes> LoadUser()
        {
            List<UserTypes> users = new List<UserTypes>();
            string qry = "select * from tblUserType where Id!=5 and IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    users.Add(new UserTypes
                    {
                        Id = dr.GetInt32("Id"),
                        UserType = dr.GetString("UserType")
                    });
                }


            }
            con.Close();
            return users;
        }


        public List<GenderModel> LoadGender()
        {
            List<GenderModel> users = new List<GenderModel>();
            string qry = "select * from tblGender";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    users.Add(new GenderModel
                    {
                        Id = dr.GetInt32("Id"),
                        Gender = dr.GetString("Gender")
                    });
                }


            }
            con.Close();
            return users;
        }

        public List<QuetionModel> LoadQuestion()
        {
            List<QuetionModel> users = new List<QuetionModel>();
            string qry = "select * from tblQuetion where IsDeleted=0";
            cmd = new SqlCommand(qry, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    users.Add(new QuetionModel
                    {
                        Id = dr.GetInt32("Id"),
                        Quetion = dr.GetString("Quetion")
                    });
                }


            }
            con.Close();
            return users;
        }

    }
}
