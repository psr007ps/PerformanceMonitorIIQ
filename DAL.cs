using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Refresh_ID
{
    public class DAL
    {
        SqlConnection coniiq;
        SqlCommand cmdiiq;
        DataSet ds = new DataSet();
        SqlDataAdapter daiiq;
        //internal SqlConnection GetConnectionString(string type)
        //{
        //    coniiq = new SqlConnection(ConfigurationManager.ConnectionStrings["coniiq"].ConnectionString);
        //    return coniiq;
        //}


        public DataSet GetTasks()
        {
            try
            {
                //coniiq = GetConnectionString("conIIQ");
                coniiq = new SqlConnection(ConfigurationManager.ConnectionStrings["coniiq"].ConnectionString);
                cmdiiq = new SqlCommand(@"select name, dateadd(s,convert(integer,LEFT(convert(varchar,created),10)),'19691231 20:00:00:000') as created,
  dateadd(s,convert(integer,LEFT(convert(varchar,completed),10)),'19691231 20:00:00:000') as completed, completion_status as status from [identityiq].[spt_task_result] where name IN
  ('Refresh Identity Cubes')", coniiq);
                daiiq = new SqlDataAdapter(cmdiiq);
                daiiq.Fill(ds);
                coniiq.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine( ex.ToString());
                Console.ReadKey();
            }
            return ds;
        }


        DataSet dsm = new DataSet();
        public DataSet GetMaintenance()
        {
            try
            {
                //coniiq = GetConnectionString("conIIQ");
                coniiq = new SqlConnection(ConfigurationManager.ConnectionStrings["coniiq"].ConnectionString);
                cmdiiq = new SqlCommand(@"select name, dateadd(s,convert(integer,LEFT(convert(varchar,created),10)),'19691231 20:00:00:000') as created,
  dateadd(s,convert(integer,LEFT(convert(varchar,completed),10)),'19691231 20:00:00:000') as completed, completion_status as status from [identityiq].[spt_task_result] where name IN
  ('Perform Maintenance', 'Perform Maintenance Background Workflows')", coniiq);
                daiiq = new SqlDataAdapter(cmdiiq);
                daiiq.Fill(dsm);
                coniiq.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            return dsm;
        }



        
        public DataSet GetNHESCCAccounts()
        {
            string application = Program.Application;
            DataSet dsnhescc = new DataSet();
            try
            {
                //coniiq = GetConnectionString("conIIQ");
                coniiq = new SqlConnection(ConfigurationManager.ConnectionStrings["coniiq"].ConnectionString);
                cmdiiq = new SqlCommand((@"select count(a.display_name) as noofaccounts 
                    from [identityiq].[identityiq].[spt_identity] as a
                    inner join [identityiq].[identityiq].[spt_link] as b on a.id=b.identity_id
                    where b.application in ('" + application + @"')"), coniiq);
                daiiq = new SqlDataAdapter(cmdiiq);
                daiiq.Fill(dsnhescc);
                coniiq.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            return dsnhescc;
        }



        public DataSet GetWorkitem()
        {
            try
            {
                //coniiq = GetConnectionString("conIIQ");
                coniiq = new SqlConnection(ConfigurationManager.ConnectionStrings["coniiq"].ConnectionString);
                cmdiiq = new SqlCommand(@"SELECT dateadd(s,convert(integer,LEFT(convert(varchar,created),10)),'19691231 20:00:00:000') as created, dateadd(s,convert(integer,LEFT(convert(varchar,modified),10)),'19691231 20:00:00:000') as modified,[owner],[name],[description],[severity]
FROM [identityiq].[identityiq].[spt_work_item]where owner in ('1fec5c0451d0c8cb0151d0c9952b02fe')", coniiq);
                daiiq = new SqlDataAdapter(cmdiiq);
                daiiq.Fill(ds);
                coniiq.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            return ds;
        }

        public DataSet GetDuplicateWorkitem()
        {
            try
            {
                //coniiq = GetConnectionString("conIIQ");
                coniiq = new SqlConnection(ConfigurationManager.ConnectionStrings["coniiq"].ConnectionString);
                cmdiiq = new SqlCommand(@"with xAttributes as (select CONVERT(xml, b.attributes)as col, identity_request_id , name
from identityiq.[identityiq].spt_work_item as b where dateadd(s,convert(
integer,LEFT(convert(varchar,created),10)),'19691231 20:00:00:000') > '2017-04-19 00:00:00.000'
)
Select X.target_id, X.identity_request_id, X.Owner, MAX(X.name) as WORKITEM, MAX(CreatedDate) as CreatedDate from(
select target_id,a.name, a.identity_request_id, dateadd(s,convert(integer,LEFT(convert(varchar,a.created),10)),
  '19691231 20:00:00:000') as CreatedDate,
project1.value('./@value','nvarchar(100)') as Owner
  from identityiq.[identityiq].spt_work_item as a,  xAttributes c
  cross apply c.col.nodes('/Attributes/Map/entry')  as T(project) 
  cross apply c.col.nodes('/Attributes/Map/entry/value/ApprovalSet/ApprovalItem')  as T1(project1)
  where project.value('./@key','nvarchar(100)') = 'approvalSet' and
  project1.value('./@application','nvarchar(100)') = 'Continuum' and
  dateadd(s,convert(integer,LEFT(convert(varchar,a.created),10)),
  '19691231 20:00:00:000') > '2017-04-19 00:00:00.000' and a.identity_request_id = c.identity_request_id and a.name = c.name)
  X group by X.target_id, X.identity_request_id, X.Owner
  having COUNT(*) > 1
", coniiq);
                daiiq = new SqlDataAdapter(cmdiiq);
                daiiq.Fill(ds);
                coniiq.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            return ds;
        }

    }
}


