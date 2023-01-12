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
        public List<GetRequestsModel> GetRequests();
        public List<GetRequestsModelAdmin> GetRequestsAdmin();
        public List<GetRequestsModelEmployee> GetRequestsEmployee(int id);
        public void ApproveRequest(int id);
        public void RejectRequest(int id);
        public List<GetRequestsModel> GetRequestsById(int stat);




        
      }
}
