using System.Collections.Generic;
using System.Data;
using System.Linq;
using EmployeePortal.Models;
using Microsoft.Data.SqlClient;

namespace EmployeePortal.Services
{
    public class EmployeeADOService
    {
        private readonly string _connectionString;

        public EmployeeADOService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("DefaultConnection is missing in appsettings.json");
            }
        }

        public List<EmployeeViewModel> GetAll()
        {
            var list = new List<EmployeeViewModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetEmployees", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new EmployeeViewModel
                        {
                            EmployeeId = (int)dr["Employee_Id"],
                            Name = dr["Name"].ToString(),
                            Email = dr["Email"].ToString(),
                            HireDate = (DateTime)dr["HireDate"],
                            DepartmentName = dr["DepartmentName"].ToString()
                        });
                    }
                }
            }

            return list;
        }


        public Employees GetById(int id)
        {
            Employees emp = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetEmployeeById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", id);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        emp = new Employees
                        {
                            EmployeeId = (int)dr["Employee_Id"],
                            Name = dr["Name"].ToString(),
                            Email = dr["Email"].ToString(),
                            HireDate = (DateTime)dr["HireDate"],
                            DepartmentId = (int)dr["Department_Id"]
                        };
                    }
                }
            }
            return emp;
        }

        public void Add(Employees emp)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AddEmployee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", emp.Name);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@HireDate", emp.HireDate);
                cmd.Parameters.AddWithValue("@DepartmentId", emp.DepartmentId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool IsEmailDuplicate(string email, int? employeeId = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CheckDuplicateEmail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId ?? 0); // 0 or NULL for new records

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public List<Department> GetDepartments()
        {
            var list = new List<Department>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetDepartment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Department
                        {
                            DepartmentId = (int)dr["Department_Id"],
                            Name = dr["Name"].ToString(),
                            LocationId = (int)dr["Location_Id"]
                        });
                    }
                }
            }

            return list;
        }
        public List<Location> GetLocations()
        {
            var list = new List<Location>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetLocation", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Location
                        {
                            LocationId = (int)dr["Location_Id"],
                            Name = dr["Name"].ToString(),
                            City = dr["City"].ToString()
                        });
                    }
                }
            }

            return list;
        }


        public void Update(Employees emp)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateEmployee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                cmd.Parameters.AddWithValue("@Name", emp.Name);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@HireDate", emp.HireDate);
                cmd.Parameters.AddWithValue("@DepartmentId", emp.DepartmentId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteEmployee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
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