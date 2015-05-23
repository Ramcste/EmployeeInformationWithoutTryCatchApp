using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using EmployeeInformation.DAL.DAO;

namespace EmployeeInformation.DAL.Gateway
{
    public class EmployeeGateway
    {
        DesignationGateway designationGateway = new DesignationGateway();

        public EmployeeGateway()
        {
            SqlConnectionObj =
                new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeConnectionString"].ConnectionString);
            SqlCommandObj = new SqlCommand();
            SqlCommandObj.Connection = SqlConnectionObj;
        }

        private SqlConnection SqlConnectionObj;
        private SqlCommand SqlCommandObj;


        public string Save(Employee anEmployee)
        {
            string message = "";
            SqlConnectionObj.Open();
            string query = String.Format("INSERT INTO tbl_EmployeeInfo VALUES('{0}','{1}','{2}','{3}')", anEmployee.Name,
                anEmployee.Email, anEmployee.Address, anEmployee.Designation.Id);
            SqlCommandObj.CommandText = query;
            SqlCommandObj.ExecuteNonQuery();
            SqlConnectionObj.Close();
            message = "Employee: " + anEmployee.Name + " has been saved";
            return message;
        }

        public string Update(Employee anEmployee)
        {
            string message = "";
            SqlConnectionObj.Open();
            string query =
                String.Format(
                    "UPDATE tbl_EmployeeInfo SET Name='{0}',Email='{1}',Address='{2}',DesignationId = {3} WHERE Id = {4}",
                    anEmployee.Name, anEmployee.Email, anEmployee.Address, anEmployee.Designation.Id,
                    anEmployee.Id);
            SqlCommandObj.CommandText = query;
            SqlCommandObj.ExecuteNonQuery();
            SqlConnectionObj.Close();
            message = "Information has been updated";
            return message;
        }

        public string Delete(Employee selectedEmployee)
        {
            string message = "";
            SqlConnectionObj.Open();
            string query = String.Format("DELETE FROM tbl_EmployeeInfo WHERE Id={0}", selectedEmployee.Id);
            SqlCommandObj.CommandText = query;
            SqlCommandObj.ExecuteNonQuery();
            SqlConnectionObj.Close();
            message = "Employee: " + selectedEmployee.Name + " has been deleted.";

            return message;
        }


        public List<Employee> GetAllEmployee()
        {
            string nameOfName = "";
            return GetAllEmployee(nameOfName);
        }

        public List<Employee> GetAllEmployee(string partOfName)
        {
            List<Employee> employees = new List<Employee>();
            SqlConnectionObj.Open();
            string query = String.Format("SELECT * FROM tbl_EmployeeInfo");

            if (partOfName != "")
            {
                query += String.Format(" WHERE Name LIKE '%{0}%'", partOfName);
            }

            query += " ORDER BY Name";

            SqlCommandObj.CommandText = query;
            SqlDataReader reader = SqlCommandObj.ExecuteReader();
            while (reader.Read())
            {
                Employee anEmployee = new Employee();
                anEmployee.Id = Convert.ToInt32(reader["Id"]);
                anEmployee.Name = reader["Name"].ToString();
                anEmployee.Email = reader["Email"].ToString();
                anEmployee.Address = reader["Address"].ToString();
                anEmployee.Designation = designationGateway.GetDesignation(Convert.ToInt32(reader["DesignationId"]));
                employees.Add(anEmployee);
            }
            SqlConnectionObj.Close();
            return employees;
        }

        public Employee FindEmployee(string email)
        {
            Employee anEmployee = null;
            SqlConnectionObj.Open();
            string query = String.Format("SELECT * FROM tbl_EmployeeInfo WHERE Email='{0}'", email);
            SqlCommandObj.CommandText = query;
            SqlDataReader reader = SqlCommandObj.ExecuteReader();

            if (reader != null)
            {
                while (reader.Read())
                {
                    anEmployee = new Employee();
                    anEmployee.Id = Convert.ToInt32(reader["Id"]);
                    anEmployee.Name = reader["Name"].ToString();
                    anEmployee.Email = reader["Email"].ToString();
                    anEmployee.Address = reader["Address"].ToString();
                    anEmployee.Designation = designationGateway.GetDesignation(Convert.ToInt32(reader["DesignationId"]));
                    SqlConnectionObj.Close();
                    return anEmployee;
                }
            }
            SqlConnectionObj.Close();
            return null;

        }

        public bool HasEmployeeWithEmail(string email)
        {
            if (FindEmployee(email) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}