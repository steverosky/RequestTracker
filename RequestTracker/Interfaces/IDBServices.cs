﻿using RequestTracker.Models.BaseModels.RequestModels;
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
        public void ApproveRequest(int id);
        public void RejectRequest(int id, string reason);
        public void SeeAdminRequest(int id);

        //public List<GetRequestsModel> GetRequestsById(int stat);





      }
}
