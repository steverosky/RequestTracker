using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestTracker.Interfaces;
using RequestTracker.Models.BaseModels.RequestModels;
using RequestTracker.Models.BaseModels.ResponseModels;
using RequestTracker.Models.DBModels;
using RequestTracker.Services;
using System.IdentityModel.Tokens.Jwt;

namespace RequestTracker.Controllers
{
    //[Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EF_dataContext _context;
        public IConfiguration _configuration;
        private readonly IDBServices _db;
        private readonly IEmailService _email;


        public UserController(EF_dataContext eF_DataContext, IConfiguration config, IDBServices dBServices, IEmailService email)
        {
            _context = eF_DataContext;
            _configuration = config;
            _db = dBServices;
            _email = email;
        }


        // GET: api/<UserController>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult Get()
        {
            ResponseType type = ResponseType.Success;
            try
            {
                IEnumerable<GetUsersModel> data = (IEnumerable<GetUsersModel>)_db.GetEmployees();

                if (!data.Any())
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            };
        }

        // GET api/<UserController>/5
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpGet]
        [Route("GetUserByEmail")]
        public IActionResult Get(string email)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                GetUsersModel data = _db.GetEmployeeByEmail(email);
                if (data == null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            };
        }

        // POST api/<UserController>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPost]
        [Route("AddUser")]
        public IActionResult Post([FromBody] AddUserModel model)
        {
            // Validate the model and the default role
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseType type = ResponseType.Success;
                _db.AddUser(model);

                return Ok(ResponseHandler.GetAppResponse(type, model));

            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        // PUT api/<UserController>/6
        [HttpPut]
        [Route("ChangePass")]
        public IActionResult ChangePass([FromBody] ChangePassModel user)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                _db.ChangePass(user);
                return Ok(ResponseHandler.GetAppResponse(type, user));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        // login  api/<UserController>/6
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(EmployeeModel model)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                var test = await _db.CreateTokenAsync(model.Email, model.Password);

                return Ok(ResponseHandler.GetAppResponse(type, test));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }

        // Make Request  api/<UserController>/6
        [AllowAnonymous]
        [HttpPost]
        [Route("MakeRequest")]
        public IActionResult MakeRequest(MakeRequestModel model)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                _db.MakeRequest(model);
                return Ok(ResponseHandler.GetAppResponse(type, model));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }

        //Get All Requests Manager  api/<UserController>/6
        [HttpGet]
        [Route("GetAllRequestsManager")]
        public IActionResult GetAllRequestsManager(int stat)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                IEnumerable<GetRequestsModel> data = (IEnumerable<GetRequestsModel>)_db.GetRequests(stat);

                if (!data.Any())
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));
            }

            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }

        //Get All Requests Admin  api/<UserController>/6
        [HttpGet]
        [Route("GetAllRequestsAdmin")]
        public IActionResult GetAllRequestsAdmin(int stat)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                IEnumerable<GetRequestsModelAdmin> data = (IEnumerable<GetRequestsModelAdmin>)_db.GetRequestsAdmin(stat);

                if (!data.Any())
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));
            }

            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }


        //Get All Requests Employee  api/<UserController>/6
        [HttpGet]
        [Route("GetAllRequestsEmployee")]
        public IActionResult GetAllRequestsEmployee(int stat)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                IEnumerable<GetRequestsModelEmployee> data = (IEnumerable<GetRequestsModelEmployee>)_db.GetRequestsEmployee(stat);

                if (!data.Any())
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));
            }

            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }

        //Approve Request  api/<UserController>/6
        [HttpPost]
        [Route("ApproveRequest")]
        public IActionResult ApproveRequest(string id)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                _db.ApproveRequest(id);
                return Ok(ResponseHandler.GetAppResponse(type, "Request Approved"));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }


        //Reject Request  api/<UserController>/6
        [HttpPost]
        [Route("RejectRequest")]
        public IActionResult RejectRequest(string id, string reason)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                _db.RejectRequest(id, reason);
                return Ok(ResponseHandler.GetAppResponse(type, "Request Denied"));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }

        //Approve Request  api/<UserController>/6
        [HttpPost]
        [Route("SeeAdminRequest")]
        public IActionResult SeeAdminRequest(string id)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                _db.SeeAdminRequest(id);
                return Ok(ResponseHandler.GetAppResponse(type, "Request Reviewed (See Admin)"));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }

        }

        // DELETE api/<UserController>/5
        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult Delete(int id)
        {
            try
            {
                ResponseType type = ResponseType.Success;

                _db.DeleteUser(id);
                return Ok(ResponseHandler.GetAppResponse(type, "Delete Success"));
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //search keyword
        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string keyword)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                List<GetRequestsModelAdmin> data = _db.GetKeyword(keyword);
                if (data == null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            };
        }

>>>>>>> Stashed changes
    }
}

        ////get all requests by keyword
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("GetAllRequestsByKeyword")]
        //public IActionResult GetRequestsById(int stat)
        //{
        //    ResponseType type = ResponseType.Success;
        //    try
        //    {
        //        IEnumerable<GetRequestsModel> data = (IEnumerable<GetRequestsModel>)_db.GetRequestsById(stat);

        //        if (!data.Any())
        //        {
        //            type = ResponseType.NotFound;
        //        }
        //        return Ok(ResponseHandler.GetAppResponse(type, data));
        //    }

        //    catch (Exception ex)
        //    {

        //        return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        //    }

        //}




        //// PUT api/<UserController>/5
        //[HttpPut]
        //[Route("UpdateUser")]
        //public IActionResult Put(int id, [FromBody] userModel model)
        //{
        //    // Load the JWT token from the request header
        //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        //    // Decode the JWT token and extract the payload
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        //    var userRole = jsonToken.Claims.First(claim => claim.Type == "Role").Value;

        //    if (HasPermission(userRole, "UpdateUser"))
        //    {

        //        try
        //        {
        //            ResponseType type = ResponseType.Success;
        //            _db.SaveUser(model);
        //            return Ok(ResponseHandler.GetAppResponse(type, model));
        //        }
        //        catch (Exception ex)
        //        {

        //            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        //        }
        //    }
        //    else
        //    {
        //        ResponseType type = ResponseType.Unauthorized;
        //        return BadRequest(ResponseHandler.GetAppResponse(type, null));
        //    }
        //}


        //// DELETE api/<UserController>/5
        //[HttpDelete]
        //[Route("DeleteUser")]
        //public IActionResult Delete(int id)
        //{
        //    // Load the JWT token from the request header
        //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        //    // Decode the JWT token and extract the payload
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        //    var userRole = jsonToken.Claims.First(claim => claim.Type == "Role").Value;

        //    if (HasPermission(userRole, "DeleteUser"))
        //    {
        //        try
        //        {
        //            ResponseType type = ResponseType.Success;

        //            _db.DeleteUser(id);
        //            return Ok(ResponseHandler.GetAppResponse(type, "Delete Success"));
        //        }
        //        catch (Exception ex)
        //        {

        //            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        //        }
        //    }
        //    else
        //    {
        //        ResponseType type = ResponseType.Unauthorized;
        //        return BadRequest(ResponseHandler.GetAppResponse(type, null));
        //    }
        //}


        //// ASSIGN ROLES api/<UserController>/5
        //[HttpPost]
        //[Route("AssignRoles")]
        //public IActionResult AssignRoles([FromBody] userModel model)
        //{
        //    // Load the JWT token from the request header
        //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        //    // Decode the JWT token and extract the payload
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        //    var userRole = jsonToken.Claims.First(claim => claim.Type == "Role").Value;

        //    if (HasPermission(userRole, "AssignRoles"))
        //    {
        //        try
        //        {
        //            ResponseType type = ResponseType.Success;

        //            _db.AssignRoles(model);
        //            return Ok(ResponseHandler.GetAppResponse(type, "Role Assigned Successfully"));
        //        }
        //        catch (Exception ex)
        //        {

        //            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        //        }
        //    }
        //    else
        //    {
        //        ResponseType type = ResponseType.Unauthorized;
        //        return BadRequest(ResponseHandler.GetAppResponse(type, null));
        //    }
        //}

        //// Method to check if the specified role is valid and supported by the application
        ////private bool IsValidRole(string role)
        ////{
        ////    // Check if the role is supported by the application
        ////    // and return the result
        ////    return (role == "user" || role == "admin" || role == "moderator");
        ////}


        //// Dictionary to map roles to their corresponding permissions
        //private Dictionary<string, List<string>> rolePermissions = new Dictionary<string, List<string>>()
        //{
        //    { "user", new List<string>() { "ChangePass", "UpdateUser", "Login" } },
        //    { "admin", new List<string>() { "ChangePass", "UpdateUser", "Login", "AddUser", "DeleteUser", "GetUsers", "GetUserById", "AssignRoles" } }
        //};

        //// Method to check if the user has the specified permission
        //private bool HasPermission(string role, string permission)
        //{
        //    // Check if the role exists in the dictionary and if it has the specified permission
        //    if (rolePermissions.ContainsKey(role) && rolePermissions[role].Contains(permission))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

 //[HttpPost]
//[Route("AddAdmin")]
//public async Task<IActionResult> AddAdmin([FromBody] userModel model)
//{
//    // Validate the model
//    if (!ModelState.IsValid)
//    {
//        return BadRequest(ModelState);
//    }

//    IdentityUser user = new()
//    {
//        Email = model.Email,
//        SecurityStamp = Guid.NewGuid().ToString(),

//    };
//    try
//    {
//        ResponseType type = ResponseType.Success;

//        //if (!await _roleManager.RoleExistsAsync(UserRoles.Admin)) 
//        //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
//        //if (!await _roleManager.RoleExistsAsync(UserRoles.User))
//        //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

//        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
//        {
//            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
//        }
//        if (await _roleManager.RoleExistsAsync(UserRoles.User))
//        {
//            await _userManager.AddToRoleAsync(user, UserRoles.User);
//        }


//        return Ok(ResponseHandler.GetAppResponse(type, model));

//    }
//    catch (Exception ex)
//    {

//        return BadRequest(ResponseHandler.GetExceptionResponse(ex));
//    }
//}
