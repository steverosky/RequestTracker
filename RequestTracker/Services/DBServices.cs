using Microsoft.EntityFrameworkCore;
using RequestTracker.Interfaces;
using RequestTracker.Models;
using RequestTracker.Models.BaseModels.ResponseModels;
using RequestTracker.Models.DBModels;
using System.Text;

namespace RequestTracker.Services
{
    public class DBServices : IDBServices
    {
        private EF_dataContext _context;

        public DBServices(EF_dataContext context)
        {
            _context = context;
        }

        // Authenticate  user details email & password for login
        public async Task<EmployeeModel> GetUser(string Email, string Password)
        {
            var user = await _context.Set<EmployeeModel>().FirstOrDefaultAsync(e => e.Email == Email);
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
        public async Task<List<GetUsersModel>> GetEmployees()
        {
            List<GetUsersModel> response = new List<GetUsersModel>();

            var employees = await _context.Set<GetUsersModel>().ToListAsync();
            employees.ForEach(row => response.Add(new GetUsersModel()
            {
                Id = row.Id,
                Name = row.Name,
                Email = row.Email,
                Password = DecodeFrom64(row.Password),
                Department = row.Department,
                Role = row.Role,
                Manager = row.Manager

            }));

            return response.OrderBy(e => e.Id).ToList();

        }
        vgcgc\\

    }
}
