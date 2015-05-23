using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using EmployeeInformation.DAL.DAO;

namespace EmployeeInformation.DAL.Gateway
{
    public class DesignationGateway
    {
        public DesignationGateway()
        {
            SqlConnectionObj =
                new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeConnectionString"].ConnectionString);
            SqlCommandObj = new SqlCommand();
            SqlCommandObj.Connection = SqlConnectionObj;
        }

        private SqlConnection SqlConnectionObj;
        private SqlCommand SqlCommandObj;

        public List<Designation> GetAll()
        {
            List<Designation> designations = new List<Designation>();
            SqlConnectionObj.Open();
            string query = String.Format("SELECT * FROM tbl_Designation");
            SqlCommandObj.CommandText = query;
            SqlDataReader readerObj = SqlCommandObj.ExecuteReader();
            while (readerObj.Read())
            {
                Designation aDesignation = new Designation();
                aDesignation.Id = Convert.ToInt32(readerObj["Id"]);
                aDesignation.Code = readerObj["Code"].ToString();
                aDesignation.Title = readerObj["Title"].ToString();
                designations.Add(aDesignation);
            }
            SqlConnectionObj.Close();
            return designations;
        }

        public Designation GetDesignation(int designationId)
        {
            SqlConnectionObj.Open();
            string query = String.Format("SELECT * FROM tbl_Designation WHERE Id='{0}'", designationId);
            SqlCommandObj.CommandText = query;
            SqlDataReader reader = SqlCommandObj.ExecuteReader();
            Designation aDesignation = new Designation();
            while (reader.Read())
            {
                aDesignation.Id = Convert.ToInt32(reader["Id"]);
                aDesignation.Code = reader["Code"].ToString();
                aDesignation.Title = reader["Title"].ToString();
            }
            SqlConnectionObj.Close();
            return aDesignation;
        }

        public bool Save(Designation aDesignation)
        {
            SqlConnectionObj.Open();
            string query = String.Format("INSERT INTO tbl_Designation VALUES('{0}','{1}')", aDesignation.Code,
                aDesignation.Title);
            SqlCommandObj.CommandText = query;
            SqlCommandObj.ExecuteNonQuery();
            SqlConnectionObj.Close();
            return true;
        }

        public bool HasThisDesignationCode(string code)
        {
            SqlConnectionObj.Open();
            string query = String.Format("SELECT * FROM tbl_Designation WHERE Code='{0}'", code);
            SqlCommandObj.CommandText = query;
            SqlDataReader reader = SqlCommandObj.ExecuteReader();
            bool hasRows = reader.HasRows;
            SqlConnectionObj.Close();
            if (reader != null)
            {
                return hasRows;
            }
            return false;
        }

        public bool HasThisDesignationTitle(string title)
        {
            SqlConnectionObj.Open();
            string query = String.Format("SELECT * FROM tbl_Designation WHERE Title='{0}'", title);
            SqlCommandObj.CommandText = query;
            SqlDataReader reader = SqlCommandObj.ExecuteReader();
            bool canRead = reader.Read();
            SqlConnectionObj.Close();
            if (reader != null)
            {
                return canRead;
            }
            return false;
        }
    }
}