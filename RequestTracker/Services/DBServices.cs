using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RequestTracker.Interfaces;
using RequestTracker.Models.BaseModels.RequestModels;
using RequestTracker.Models.BaseModels.ResponseModels;
using RequestTracker.Models.DBModels;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RequestTracker.Services
{
    public class DBServices : IDBServices
    {
        private EF_dataContext _context;
        private IEmailService _email;
        public IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DBServices(EF_dataContext context, IEmailService email, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _email = email;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }


        // Authenticate  user details email & password for login
        public async Task<EmployeeModel> GetUser(string Email, string Password)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == Email);
            var decoded = DecodeFrom64(user.Password);
            if (user is not null && decoded == Password)
            {
                return user;
            }

            return null;
        }




        //this function Convert to Encord your Password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //this function Convert to Decord your Password
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }




        //generates a random password from 0-1 and a-z lower and uppercase using the random class
        public static string GetRandomPassword(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }

        public static string DefaultPass()
        {
            int length = 10;

            string password = GetRandomPassword(length);

            return password;
        }


        // Get all employees
        public List<GetUsersModel> GetEmployees()
        {
            var roleclaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (roleclaim == "admin")
            {
                List<GetUsersModel> response = new List<GetUsersModel>();

                var dataList = _context.Employees.ToList();

                foreach (var row in dataList)
                {
                    // Retrieve the name of the employee's department
                    var dept = _context.Departments.FirstOrDefault(d => d.DeptId == row.DeptId).DeptName;

                    // Retrieve the name of the employee's manager
                    var manager = _context.Employees.FirstOrDefault(m => m.UserId == row.ManagerId).Name;

                    // Retrieve the name of the employee's role
                    var role = _context.Roles.FirstOrDefault(r => r.RoleId == row.RoleId).RoleName;

                    response.Add(new GetUsersModel()
                    {
                        Id = row.UserId,
                        Name = row.Name,
                        Email = row.Email,
                        Password = DecodeFrom64(row.Password),
                        Department = dept,
                        Manager = manager,
                        Role = role,
                        Status = row.Status

                    });

                }
                return response.OrderBy(e => e.Id).ToList();
            }
            else
            {
                throw new Exception("UnAuthorized Request");
            }

        }

        //get employee by email
        public GetUsersModel GetEmployeeByEmail(string email)
        {
            var roleclaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            GetUsersModel response = new GetUsersModel();
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email);
            if (employee is not null && roleclaim == "admin")
            {
                var dept = _context.Departments.FirstOrDefault(d => d.DeptId == employee.DeptId).DeptName;
                var manager = _context.Employees.FirstOrDefault(m => m.UserId == employee.ManagerId).Name;
                var role = _context.Roles.FirstOrDefault(r => r.RoleId == employee.RoleId).RoleName;
                return new GetUsersModel()
                {
                    Id = employee.UserId,
                    Name = employee.Name,
                    Email = employee.Email,
                    Password = DecodeFrom64(employee.Password),
                    Department = dept,
                    Role = role,
                    Status = employee.Status,
                    Manager = manager
                };
            }

            return null;
        }

        //post or add new user
        public void AddUser(AddUserModel user)
        {
            //get role of user for approval
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            // check if user Email exists before adding and auto-increment the id
            var EmailList = _context.Employees.Select(p => p.Email).ToList();

            if (EmailList.Contains(user.Email))
            {
                throw new Exception("email already exists");
            }
            else if (role == "admin")
            {
                var idList = _context.Employees.Select(x => x.UserId).ToList();
                var maxId = idList.Any() ? idList.Max() : 0;
                //var maxId = idList.Max();
                user.Id = maxId + 1;
                string Password = DefaultPass();
                user.Password = EncodePasswordToBase64(Password);


                EmployeeModel dbTable = new EmployeeModel();
                dbTable.UserId = user.Id;
                dbTable.Name = user.Name;
                dbTable.Email = user.Email;
                dbTable.Password = user.Password;
                dbTable.DeptId = user.DepartmentId;
                dbTable.Status = "InActive";
                dbTable.RoleId = user.RoleId;
                dbTable.ManagerId = user.ManagerId;
                _context.Employees.Add(dbTable);
                _context.SaveChanges();


                //read html file from current directory and pass it to the email body
                string FilePath = Directory.GetCurrentDirectory() + "\\index2.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[username]", user.Name).Replace("[email]", user.Email).Replace("[Password]", Password).Replace("[logo]", "cid:image1");

                //send mail
                _email.sendMail(MailText, user.Email);
            }
        }

        //change password
        public void ChangePass(ChangePassModel user)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Email == user.Email);
            var pass = DecodeFrom64(employee.Password);
            if (employee is not null && pass == user.CurrentPassword)
            {
                employee.Password = EncodePasswordToBase64(user.NewPassword);
                employee.Status = "Active";
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Invalid email or password");
            }

        }

        //make a product request
        public void MakeRequest(MakeRequestModel request)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.UserId == request.EmployeeId);
            //check if user Status is Active ie. user has changed password
            if (employee.Status == "Active")
            {

                //retrieve id list and auto increment
                var idList = _context.Requests.Select(x => x.RequestId).ToList();
                var maxId = idList.Any() ? idList.Max() : 0;
                request.Id = maxId + 1;
                var status = _context.Status.FirstOrDefault(s => s.StatusId == 1).StatusName;
                string requestid = request.Id.ToString();

                var manager = _context.Managers.FirstOrDefault(m => m.ManagerId == employee.ManagerId);

                RequestModel dbTable = new RequestModel();
                dbTable.RequestId = request.Id;
                dbTable.UserId = request.EmployeeId;
                dbTable.RequestDesc = request.Description;
                dbTable.CategoryId = request.CategoryId;
                dbTable.ManagerReview = status;
                dbTable.AdminReview = status;
                dbTable.DeptId = employee.DeptId;
                dbTable.DateTime = DateTime.UtcNow;
                _context.Requests.Add(dbTable);
                _context.SaveChanges();

                //send mail to manager
                string FilePath1 = Directory.GetCurrentDirectory() + "\\adminNotify.html";
                StreamReader strg = new StreamReader(FilePath1);
                string MailText1 = strg.ReadToEnd();
                strg.Close();
                MailText1 = MailText1.Replace("[requestid]", requestid).Replace("[logo]", "cid:image1");

                _email.sendMail(MailText1, manager.ManagerEmail);
            }
            else
            {
                throw new Exception("Please change the default password");
            }
        }

        //get all requests for managers
        public List<GetRequestsModel> GetRequests(int stat)
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (role == "manager")
            {
                List<GetRequestsModel> response = new List<GetRequestsModel>();
                //get the department of the manager
                var deptId = _context.Employees.Where(x => x.Email == email).Select(x => x.DeptId).FirstOrDefault();
                //filter the requests by the department
                var dataList1 = _context.Requests.Where(x => x.DeptId == deptId).ToList();
                var review = _context.Status.FirstOrDefault(s => s.StatusId == stat).StatusName;
                
                var dataList = new List<RequestModel>();
                if (stat == 1)
                {
                    dataList = dataList1.Where(x => x.ManagerReview == review ).ToList();
                }
                else if (stat == 2)
                {
                    dataList = dataList1.Where(x => x.ManagerReview == review ).ToList();
                }
                else if (stat == 3)
                {
                    dataList = dataList1.Where(x => x.ManagerReview == review ).ToList();
                }
                
                else if (stat == 5)// where 5 = all requests
                {
                    dataList = dataList1;
                }
                else
                {
                    throw new Exception("No Records");
                };

                foreach (var row in dataList)
                {
                    // Retrieve the name of the employee
                    var employee = _context.Employees.FirstOrDefault(e => e.UserId == row.UserId).Name;

                    // Retrieve the name of the employee's manager
                    var category = _context.Categories.FirstOrDefault(c => c.CategoryId == row.CategoryId).CategoryName;

                    response.Add(new GetRequestsModel()
                    {
                        RequestId = row.RequestId,
                        Description = row.RequestDesc,
                        Category = category,
                        Name = employee,
                        DateTime = row.DateTime,
                        ManangerReview = row.ManagerReview,
                        AdminReview = row.AdminReview

                    });
                }
                return response.OrderBy(e => e.RequestId).ToList();
            }
            else
            {
                throw new Exception("UnAuthorized Request");
            }
        }

        //get all requests for admin
        public List<GetRequestsModelAdmin> GetRequestsAdmin(int stat)
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role == "admin")
            {
                var review = _context.Status.FirstOrDefault(s => s.StatusId == stat).StatusName;
                List<GetRequestsModelAdmin> response = new List<GetRequestsModelAdmin>();
                var dataList = new List<RequestModel>();
                if (stat == 1)
                {
                    dataList = _context.Requests.Where(x => x.ManagerReview == "Approved" && x.AdminReview==review).ToList();
                }
                else if (stat == 2)
                {
                    dataList = _context.Requests.Where(x => x.ManagerReview == review && x.AdminReview == review).ToList();
                }                
                else if (stat == 3)
                {
                    dataList = _context.Requests.Where(x => x.ManagerReview == "Approved" && x.AdminReview == review || x.AdminReview == "See Admin").ToList();
                }
               
                else if(stat == 5)// where 5 = all requests
                {
                    dataList = _context.Requests.ToList();
                }
                else
                {
                    throw new Exception("No Records");
                };
                 
                foreach (var row in dataList)
                {
                    // Retrieve the name of the employee
                    var employee = _context.Employees.FirstOrDefault(e => e.UserId == row.UserId).Name;

                    // Retrieve the name of the category
                    var category = _context.Categories.FirstOrDefault(c => c.CategoryId == row.CategoryId).CategoryName;

                    // Retrieve the name of the employee's department
                    var department = _context.Departments.FirstOrDefault(d => d.DeptId == row.UserId).DeptName;


                    response.Add(new GetRequestsModelAdmin()
                    {
                        RequestId = row.RequestId,
                        Description = row.RequestDesc,
                        Category = category,
                        Name = employee,
                        Department = department,
                        DateTime = row.DateTime,
                        ManangerReview = row.ManagerReview,
                        AdminReview = row.AdminReview

                    });
                }
                return response.OrderBy(e => e.RequestId).ToList();
            }
            else
            {
                throw new Exception("UnAuthorized Request");
            }

        }

        //get all requests for employees
        public List<GetRequestsModelEmployee> GetRequestsEmployee(int stat)
        {
            //var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            var id = _context.Employees.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefault();
            var dataList1 = _context.Requests.Where(e => e.UserId == id).ToList();

            var review = _context.Status.FirstOrDefault(s => s.StatusId == stat).StatusName;
            var dataList = new List<RequestModel>();
            if (stat == 1)
            {
                dataList = dataList1.Where(x => x.AdminReview == review).ToList();
            }
            else if (stat == 2)
            {
                dataList = dataList1.Where(x => x.AdminReview == review).ToList();
            }
            else if (stat == 3)
            {
                dataList = dataList1.Where(x => x.ManagerReview == review || x.AdminReview == review || x.AdminReview == "See Admin").ToList();
            }
            
            else if (stat == 5)// where 5 = all requests
            {
                dataList = dataList1;
            }
            else
            {
                throw new Exception("No Records");
            };

            List<GetRequestsModelEmployee> response = new List<GetRequestsModelEmployee>();

            foreach (var row in dataList)
            {
                // Retrieve the name of the category
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == row.CategoryId).CategoryName;

                response.Add(new GetRequestsModelEmployee()
                {
                    RequestId = row.RequestId,
                    Description = row.RequestDesc,
                    Category = category,
                    DateTime = row.DateTime,
                    ManangerReview = row.ManagerReview,
                    AdminReview = row.AdminReview

                });
            }
            return response.OrderBy(e => e.RequestId).ToList();
           
        }

        //approve request
        public void ApproveRequest(int id)
        {
            //get role of user for approval
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            //retrieve status and requests from db
            var request = _context.Requests.FirstOrDefault(r => r.RequestId == id);
            var status = _context.Status.FirstOrDefault(s => s.StatusId == 2).StatusName;
            var employee = _context.Employees.FirstOrDefault(e => e.UserId == request.UserId);
            var manager = _context.Managers.FirstOrDefault(m => m.ManagerId == employee.ManagerId);
            //var admin = _context.Employees.FirstOrDefault(a => a.RoleId == 2);
            var admin = (from u in _context.Employees
                         join r in _context.Roles on u.RoleId equals r.RoleId
                         where r.RoleId == 2
                         select u.Email).FirstOrDefault();

            string requestid = request.RequestId.ToString();

            //if role is manager
            if (request is not null && role == "manager")
            {
                
                request.ManagerReview = status;
                _context.SaveChanges();

                //send mail to employee
                //read html file from current directory and pass it to the email body
                string FilePath = Directory.GetCurrentDirectory() + "\\managerApproval.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[username]", employee.Name).Replace("[requestid]", requestid).Replace("[logo]", "cid:image1");
                                
                _email.sendMail(MailText, employee.Email);

                //send mail to admin
                string FilePath1 = Directory.GetCurrentDirectory() + "\\adminNotify.html";
                StreamReader strg = new StreamReader(FilePath1);
                string MailText1 = strg.ReadToEnd();
                strg.Close();
                MailText1 = MailText1.Replace("[requestid]", requestid).Replace("[logo]", "cid:image1");

                _email.sendMail(MailText1, admin);
            }
            else if (request is not null && role == "admin")
            {
                request.AdminReview = status;
                _context.SaveChanges();

                string FilePath = Directory.GetCurrentDirectory() + "\\finalApprove.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[requestid]", requestid).Replace("[logo]", "cid:image1");

                _email.sendMail(MailText, employee.Email);
                _email.sendMail(MailText, manager.ManagerEmail);
            }
            else
            {
                throw new Exception("Invalid request");
            }

        }

        //reject request
        public void RejectRequest(int id, string reason)
        {
            //get role of user for approval
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            //retrieve status and requests from db
            var request = _context.Requests.FirstOrDefault(r => r.RequestId == id);
            var employee = _context.Employees.FirstOrDefault(e => e.UserId == request.UserId);
            var status = _context.Status.FirstOrDefault(s => s.StatusId == 3).StatusName;
            var manager = _context.Managers.FirstOrDefault(m => m.ManagerId == employee.ManagerId);


            string requestid = request.RequestId.ToString();

            //if role is manager
            if (request is not null && role == "manager")
            {
                request.ManagerReview = status;
                request.RejectReason = reason;
                _context.SaveChanges();

                string FilePath = Directory.GetCurrentDirectory() + "\\managerReject.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[username]", employee.Name).Replace("[requestid]", requestid).Replace("[logo]", "cid:image1").Replace("[who]", "Manager").Replace("[reason]", reason);

                _email.sendMail(MailText, employee.Email);
            }
            else if (request is not null && role == "admin")
            {
                request.AdminReview = status;
                request.RejectReason = reason;

                _context.SaveChanges();

                string FilePath = Directory.GetCurrentDirectory() + "\\managerReject.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[username]", employee.Name).Replace("[requestid]", requestid).Replace("[logo]", "cid:image1").Replace("[who]", "Admin").Replace("[reason]", reason);

                _email.sendMail(MailText, employee.Email);
                _email.sendMail(MailText, manager.ManagerEmail);
            }
            else
            {
                throw new Exception("Invalid request");
            }

        }

       
        public async Task<object> CreateTokenAsync(string email, string password)
        {
            var token1 = "";
            double expireTime = 0;
            if (email != null && password != null)
            {
                //var user = (from u in _context.Employees
                //            join r in _context.Roles on u.RoleId equals r.RoleId
                //            where u.Email == Email && u.Password == Password
                //            select new { u, r }).FirstOrDefault();

                var user = await GetUser(email, password);
                var role = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId).RoleName;


                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtConfig:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Email, user.Email.ToString()),
                        new Claim(ClaimTypes.Role, role.ToString()),

                        };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["JwtConfig:Issuer"],
                        _configuration["JwtConfig:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(120),
                        signingCredentials: signIn);


                    token1 = new JwtSecurityTokenHandler().WriteToken(token);
                    expireTime = token.ValidTo.Subtract(DateTime.UtcNow).TotalSeconds;
                    double expiryTimeInSeconds = Math.Ceiling(expireTime);


                    return new TokenResponse()
                    {
                        Token = token1,
                        Role = role,
                        Email = email,
                        ExpiryTime = expiryTimeInSeconds,
                        Name = user.Name,
                        Id = user.UserId,
                        ManagerId = user.ManagerId,
                        RoleId = user.RoleId,
                        DeptId = user.DeptId,
                        Status = user.Status
                        
                    };
                }
                else
                {
                    throw new Exception("Invalid credentials");
                }

            }
            else
            {
                throw new Exception("Invalid credentials");
            }

        }

    }
}

////get all requests by keyword
//public List<GetRequestsModel> GetRequestsById(int stat)
//{
//    List<GetRequestsModel> response = new List<GetRequestsModel>();
//    var review = _context.Status.FirstOrDefault(s => s.StatusId == stat).StatusName;

//    var dataList = _context.Requests.Where(r => r.ManagerReview == review && r.AdminReview == review).ToList();

//    foreach (var row in dataList)
//    {
//        // Retrieve the name of the employee
//        var employee = _context.Employees.FirstOrDefault(e => e.UserId == row.UserId).Name;

//        // Retrieve the name of the employee's manager
//        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == row.CategoryId).CategoryName;

//        response.Add(new GetRequestsModel()
//        {
//            RequestId = row.RequestId,
//            Description = row.RequestDesc,
//            Category = category,
//            Name = employee,
//            DateTime = row.DateTime,
//            ManangerReview = row.ManagerReview,
//            AdminReview = row.AdminReview

//        });
//    }
//    return response.OrderBy(e => e.RequestId).ToList();

//}






//// _context.Employees
//                        .FromSqlRaw("INSERT INTO users (UserId, Name, Email, Password, RoleId, ManagerId, DeptId) values " +
//                        "(user.UserId, user.Name, user.Email, user.Password, user.Status, user.RoleId, user.ManagerId, user.DeptId) dbTable.Id, dbTable.Name, " +
//                        "dbTable.Email, dbTable.Password, dbTable.Role, dbTable.Manager, dbTable.Department);
//                    dbTable.Department = _context.Departments.FromSqlRaw("SELECT FROM department WHERE Id = {0}", user.DepartmentId).Select(dbTable => dbTable.Name).FirstOrDefault();


