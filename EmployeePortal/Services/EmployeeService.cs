using System.Collections.Generic;
using System.Data;
using System.Linq;
using EmployeePortal.Models;
using Microsoft.Data.SqlClient;

namespace EmployeePortal.Services
{
    public class EmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("DefaultConnection is missing in appsettings.json");
            }
        }

        public List<Employee> GetAllEmployees()
        {
            var list = new List<Employee>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetAllEmployees", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Employee
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Department = reader["Department"].ToString(),
                    Salary = (decimal)reader["Salary"],
                    Age = (int)reader["Age"]
                });
            }
            return list;
        }

        public void InsertEmployee(Employee emp)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_InsertEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@Age", emp.Age);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public Employee GetEmployeeById(int id)
        {
            Employee emp = new Employee();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetEmployeeById", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                emp =  new Employee
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Department = reader["Department"].ToString(),
                    Age = (int)reader["Age"],
                    Salary = (decimal)reader["Salary"]
                };
            }
            return emp;
        }

        public void UpdateEmployee(Employee emp)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", emp.Id);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@Age", emp.Age);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteEmployee(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_DeleteEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        //Sample with inline queries
        //public void Add(Employee emp)
        //{
        //    using var con = new SqlConnection(_connectionString);
        //    con.Open();
        //    var cmd = new SqlCommand("INSERT INTO Employees (Name, Email, Department, Salary) VALUES (@Name, @Email, @Department, @Salary)", con);
        //    cmd.Parameters.AddWithValue("@Name", emp.Name);
        //    cmd.Parameters.AddWithValue("@Email", emp.Email);
        //    cmd.Parameters.AddWithValue("@Department", emp.Department);
        //    cmd.Parameters.AddWithValue("@Salary", emp.Salary);
        //    cmd.ExecuteNonQuery();
        //}

        //public void Update(Employee emp)
        //{
        //    using var con = new SqlConnection(_connectionString);
        //    con.Open();
        //    var cmd = new SqlCommand("UPDATE Employees SET Name=@Name, Email=@Email, Department=@Department, Salary=@Salary WHERE Id=@Id", con);
        //    cmd.Parameters.AddWithValue("@Id", emp.Id);
        //    cmd.Parameters.AddWithValue("@Name", emp.Name);
        //    cmd.Parameters.AddWithValue("@Email", emp.Email);
        //    cmd.Parameters.AddWithValue("@Department", emp.Department);
        //    cmd.Parameters.AddWithValue("@Salary", emp.Salary);
        //    cmd.ExecuteNonQuery();
        //}

        //public void Delete(int id)
        //{
        //    using var con = new SqlConnection(_connectionString);
        //    con.Open();
        //    var cmd = new SqlCommand("DELETE FROM Employees WHERE Id=@Id", con);
        //    cmd.Parameters.AddWithValue("@Id", id);
        //    cmd.ExecuteNonQuery();
        //}
    }

}