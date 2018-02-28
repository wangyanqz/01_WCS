namespace WCS.Data
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;

    public static class DataBase
    {
        public static MySqlConnection dbConn;

        public static void add_port(string ls_pipeline, string ls_outport, string ls_Siteid, string ls_name, string ls_person_id, string ls_if_paijian, string ls_sub_siteid, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = dbConn,
                    CommandText = "add_port_in",
                    CommandType = CommandType.StoredProcedure
                };
                MySqlParameter parameter = new MySqlParameter("?ps_pipeline", MySqlDbType.VarChar)
                {
                    Value = ls_pipeline
                };
                command.Parameters.Add(parameter);
                MySqlParameter parameter2 = new MySqlParameter("?ps_port_id", MySqlDbType.VarChar)
                {
                    Value = ls_outport
                };
                command.Parameters.Add(parameter2);
                MySqlParameter parameter3 = new MySqlParameter("?ps_siteid", MySqlDbType.VarChar, 10)
                {
                    Value = ls_Siteid
                };
                command.Parameters.Add(parameter3);
                MySqlParameter parameter4 = new MySqlParameter("?ps_name", MySqlDbType.VarChar, 40)
                {
                    Value = ls_name
                };
                command.Parameters.Add(parameter4);
                MySqlParameter parameter5 = new MySqlParameter("?ps_person_id", MySqlDbType.VarChar, 10)
                {
                    Value = ls_person_id
                };
                command.Parameters.Add(parameter5);
                MySqlParameter parameter6 = new MySqlParameter("?ps_if_paijian", MySqlDbType.VarChar, 1)
                {
                    Value = ls_if_paijian
                };
                command.Parameters.Add(parameter6);
                MySqlParameter parameter7 = new MySqlParameter("?ps_sub_siteid", MySqlDbType.VarChar, 10)
                {
                    Value = ls_sub_siteid
                };
                command.Parameters.Add(parameter7);
                MySqlParameter parameter8 = new MySqlParameter("?ri_ret", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameter8);
                MySqlParameter parameter9 = new MySqlParameter("?rs_ret", MySqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameter9);
                command.ExecuteNonQuery();
                if (parameter8.Value.ToString() != "0")
                {
                    strErrorMsg = parameter9.Value.ToString().Trim();
                }
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message;
            }
        }

        public static void ClearAll(string ls_pipeline, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand("update tb_port_package set packagecode='',creattime=now() where SUBSTRING(portcode,1,10) ='" + ls_pipeline + "'", dbConn).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message.ToString();
            }
        }

        public static void ClearSelect(string ls_portcode, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand("update tb_port_package set packagecode='',creattime=now() where portcode='" + ls_portcode + "'", dbConn).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message.ToString();
            }
        }

        public static void delete_backup_history_data(MySqlConnection backupDbConn, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand
                {
                    Connection = backupDbConn,
                    CommandText = "delete_scanner_data",
                    CommandType = CommandType.StoredProcedure
                }.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message.ToString();
            }
        }

        public static void delete_bill_auto_supply(MySqlConnection opeDbConn, long ll_id, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand("delete from tb_bill_weight_ks where id =" + ll_id.ToString(), opeDbConn).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message;
            }
        }

        public static void delete_bill_hand(MySqlConnection opeDbConn, string ls_billcode, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand("delete from tb_bill_hand where billCode ='" + ls_billcode + "'", opeDbConn).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message;
            }
        }

        public static void delete_history_data(ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand
                {
                    Connection = dbConn,
                    CommandText = "delete_scanner_data",
                    CommandType = CommandType.StoredProcedure
                }.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message.ToString();
            }
        }

        public static void delete_port(int li_id, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand("delete from td_port where id =" + li_id.ToString(), dbConn).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message;
            }
        }

        public static void edit_port(int li_id, string ls_outport, string ls_Siteid, string ls_name, string ls_person_id, string ls_if_paijian, string ls_sub_siteid, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                new MySqlCommand("update td_port set port_id ='" + ls_outport + "',siteid ='" + ls_Siteid + "',SiteNAME ='" + ls_name + "',person_id ='" + ls_person_id + "',if_paijian ='" + ls_if_paijian + "',sub_siteid ='" + ls_sub_siteid + "' where id =" + li_id.ToString(), dbConn).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message;
            }
        }

        public static DataSet GetOutportInf(string ls_pipeline, ref string errorMsg)
        {
            errorMsg = string.Empty;
            DataSet set = new DataSet();
            string commandText = "SELECT id,port_id,siteid,SiteNAME,person_id,sub_siteid,if(if_paijian='1','是','否') if_paijian FROM td_port where pipeline='" + ls_pipeline + "'order by CAST(port_id AS SIGNED),siteid,SiteNAME";
            try
            {
                set = MySqlHelper.ExecuteDataset(dbConn, commandText);
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return null;
            }
            return set;
        }

        public static DataSet GetPortPackageInf(ref string errorMsg)
        {
            errorMsg = string.Empty;
            DataSet set = new DataSet();
            string commandText = "SELECT t.portcode,t.packagecode,t.creattime FROM tb_port_package t order by t.portcode";
            try
            {
                set = MySqlHelper.ExecuteDataset(dbConn, commandText);
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return null;
            }
            return set;
        }

        public static DataSet GetSortnumInf(DateTime startTime, DateTime endTime, ref string errorMsg)
        {
            errorMsg = string.Empty;
            string str = startTime.ToString("yyyyMMdd") + "000000";
            string str2 = endTime.ToString("yyyyMMdd") + "235959";
            DataSet set = new DataSet();
            string commandText = string.Empty;
            commandText = "SELECT batch_no,sum_num,sum_nodata,concat(ROUND(100*sum_nodata/sum_num,2),'%') as nodata_per,\r\n                          sum_nochannel,concat(ROUND(100*sum_nochannel/sum_num,2),'%') as noport_per,sum_error,\r\n                          sum_handcode,concat(ROUND(100*sum_handcode/sum_num,2),'%') as handcode_per,\r\n                          update_time,num1,num2,num3,num4,num5,num6,num7,num8,num9,num10,num11,num12,num13,num14 FROM tb_sort_num \r\n                    where sum_num>0 and batch_no between '" + str + "' and '" + str2 + "' union ALL SELECT '总计' as batch_no,sum(sum_num),sum(sum_nodata),concat(ROUND(100*sum(sum_nodata)/sum(sum_num),2),'%') as nodata_per,sum(sum_nochannel),concat(ROUND(100*sum(sum_nochannel)/sum(sum_num),2),'%') as noport_per,sum(sum_error),sum(sum_handcode),concat(ROUND(100*sum(sum_handcode)/sum(sum_num),2),'%') as handcode_per,null,sum(num1),sum(num2),sum(num3),sum(num4),sum(num5),sum(num6),sum(num7),sum(num8),sum(num9),sum(num10),sum(num11),sum(num12),sum(num13),sum(num14) FROM tb_sort_num \r\n                    where sum_num>0 and batch_no between '" + str + "' and '" + str2 + "' order by batch_no";
            try
            {
                set = MySqlHelper.ExecuteDataset(dbConn, commandText);
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return null;
            }
            return set;
        }

        public static int GetUnPackageNum(ref string errorMsg)
        {
            errorMsg = string.Empty;
            int num = 0;
            int num2 = 0;
            DataSet set = new DataSet();
            string commandText = "SELECT count(*) FROM td_port b where b.if_package='1'";
            try
            {
                num = int.Parse(MySqlHelper.ExecuteDataset(dbConn, commandText).Tables[0].Rows[0][0].ToString());
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return 0;
            }
            commandText = "SELECT count(*) FROM tb_port_package a,td_port b\r\n                    where CAST(a.portcode AS SIGNED)=CAST(b.port_id AS SIGNED) and b.if_package='1' and length(a.packagecode)>10";
            try
            {
                num2 = int.Parse(MySqlHelper.ExecuteDataset(dbConn, commandText).Tables[0].Rows[0][0].ToString());
            }
            catch (Exception exception2)
            {
                errorMsg = exception2.Message;
                return 0;
            }
            return (num - num2);
        }

        public static string Query_register_code()
        {
            string str;
            DataSet set = new DataSet();
            try
            {
                string commandText = "select ifnull(max(t.register_code),'') from td_register t";
                set = MySqlHelper.ExecuteDataset(dbConn, commandText);
                str = set.Tables[0].Rows[0][0].ToString().Trim();
                set.Dispose();
            }
            catch (Exception)
            {
                return "F";
            }
            return str;
        }

        public static string Query_register_date()
        {
            string str;
            DataSet set = new DataSet();
            try
            {
                string commandText = "select ifnull(max(t.register_date),'') from td_register t";
                set = MySqlHelper.ExecuteDataset(dbConn, commandText);
                str = set.Tables[0].Rows[0][0].ToString().Trim();
                set.Dispose();
            }
            catch (Exception)
            {
                return "F";
            }
            return str;
        }

        public static bool Update_register_code(string ls_code)
        {
            try
            {
                new MySqlCommand("update td_register t set t.register_code = '" + ls_code + "'", dbConn).ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Update_register_date(string ls_code)
        {
            try
            {
                new MySqlCommand("update td_register t set t.register_date = '" + ls_code + "'", dbConn).ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void UpdateSiteCode(string ls_bill_code, string ls_site_code1, string ls_site_code2, string ls_Siteid, ref string strErrorMsg)
        {
            strErrorMsg = string.Empty;
            try
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = dbConn,
                    CommandText = "add_bill",
                    CommandType = CommandType.StoredProcedure
                };
                MySqlParameter parameter = new MySqlParameter("?ps_billCode", MySqlDbType.VarChar, 20);
                command.Parameters.Add(parameter);
                MySqlParameter parameter2 = new MySqlParameter("?ps_site_code1", MySqlDbType.VarChar, 10);
                command.Parameters.Add(parameter2);
                MySqlParameter parameter3 = new MySqlParameter("?ps_site_code2", MySqlDbType.VarChar, 10);
                command.Parameters.Add(parameter3);
                MySqlParameter parameter4 = new MySqlParameter("?ps_site_code3", MySqlDbType.VarChar, 10);
                command.Parameters.Add(parameter4);
                MySqlParameter parameter5 = new MySqlParameter("?ps_SOURCE", MySqlDbType.VarChar, 1);
                command.Parameters.Add(parameter5);
                parameter5.Value = "2";
                parameter.Value = ls_bill_code;
                parameter2.Value = ls_site_code1;
                parameter3.Value = ls_site_code2;
                parameter4.Value = ls_Siteid;
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                strErrorMsg = exception.Message;
            }
        }
    }
}


