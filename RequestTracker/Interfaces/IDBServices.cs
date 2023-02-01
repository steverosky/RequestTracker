using RequestTracker.Models.BaseModels.RequestModels;
using RequestTracker.Models.BaseModels.ResponseModels;
using RequestTracker.Models.DBModels;

namespace RequestTracker.Interfaces
{
    public interface IDBServices
    {
        public Task<EmployeeModel> GetUser(string Email, string Password);
        //public string EncodePasswordToBase64(string password);
        //public string GetRandomPassword(int length);
        //public string DefaultPass();
        List<GetUsersModel> GetEmployees();
        GetUsersModel GetEmployeeByEmail(string email);
        void AddUser(AddUserModel user);
        void ChangePass(ChangePassModel user);
        Task<object> CreateTokenAsync(string email, string password);
        public void MakeRequest(MakeRequestModel request);
        public List<GetRequestsModel> GetRequests(int stat);
        public List<GetRequestsModelAdmin> GetRequestsAdmin(int stat);
        public List<GetRequestsModelEmployee> GetRequestsEmployee(int stat);
        public void ApproveRequest(string id);
        public void RejectRequest(string id, string reason);
        public void SeeAdminRequest(string id);
        public void DeleteUser(int id);
<<<<<<<<< Temporary merge branch 1
        //public void ForgetPassword(string email);
        //public void ResetPassword(ForgetPassModel request);
=========
        public List<GetRequestsModelAdmin> GetKeyword(string keyword);
        //public void ForgetPassword(string email);
        //public void ResetPassword(ForgetPassModel request);
>>>>>>> Stashed changes
>>>>>>>>> Temporary merge branch 2

        //public List<GetRequestsModel> GetRequestsById(int stat);





      }
}
