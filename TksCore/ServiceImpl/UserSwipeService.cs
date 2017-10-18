using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Tks.Model;
using Tks.Entities;
using Tks.Services;


namespace Tks.ServiceImpl
{
    internal partial class UserSwipeService
        : IUserSwipeService
    {

        #region Class Variables
        IAppManager _appManager;
        SqlConnection mDbConnection;
        #endregion

        public List<UserSwipe> Retrieve(int userId, DateTime fromDate, DateTime toDate)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                //Define a Command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "RetriveUserSwipe_new";

                // Assign the parameters.
                command.Parameters.Add("@Userid", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@dateto", SqlDbType.DateTime).Value = toDate;
                adapter = new SqlDataAdapter(command);

                //Define the DataTable.
                DataTable SwipeDataTable = new DataTable();
                adapter.Fill(SwipeDataTable);
                List<UserSwipe> userSwipes = null;

                if (SwipeDataTable.Rows.Count > 0)
                {
                    //create a List Instance  of type UserSwipe.
                    userSwipes = new List<UserSwipe>();

                    //Define the Datarow.
                    foreach (DataRow dr in SwipeDataTable.Rows)
                    {
                        UserSwipe swipe = new UserSwipe(Int32.Parse(dr["SINO"].ToString()));
                        swipe.UserId = Int32.Parse(dr["UserId"].ToString());
                        swipe.WorkDate = Convert.ToDateTime(dr["WorkDate"].ToString());
                        swipe.TimeZoneId = Int32.Parse(dr["TimeZoneId"].ToString());
                        if (dr["CheckDateTime"].ToString() != "")
                            swipe.CheckInTime = Convert.ToDateTime(dr["CheckDateTime"].ToString());
                        else
                            swipe.CheckInTime = null;
                        if (dr["CheckOutDateTime"].ToString() != "")
                            swipe.CheckOutTime = Convert.ToDateTime(dr["CheckOutDateTime"].ToString());
                        else
                            swipe.CheckOutTime = null;
                        swipe.CreateUserId = Int32.Parse(dr["CreateUserId"].ToString());
                        swipe.CreateDate = Convert.ToDateTime(dr["CreateUserDate"].ToString());
                        swipe.LastUpdateUserId = Int32.Parse(dr["LastUpdateUserId"].ToString());
                        swipe.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"].ToString());
                        swipe.Shift = string.IsNullOrEmpty(dr["Shift"].ToString()) ? 0 : Int32.Parse(dr["Shift"].ToString());

                        swipe.UserworktypeId = string.IsNullOrEmpty(dr["UserworktypeId"].ToString()) ? 1 : Int32.Parse(dr["UserworktypeId"].ToString());
                        
                        swipe.Reason = dr["Reason"].ToString();
                        swipe.CustomData.Add("ShortName", dr["ShortName"].ToString());
                        //swipe.CustomData.Add("ActiveStatus", dr["ActiveStatus"].ToString());
                        //swipe.CustomData.Add("Parentuserid", dr["Parentuserid"].ToString());
                        //swipe.CustomData.Add("Attachdate", dr["Attachdate"].ToString());
                        //swipe.CustomData.Add("DetachDate", dr["DetachDate"].ToString());
                        //Add to the list.
                        userSwipes.Add(swipe);

                    }
                }
                return userSwipes;

            }
            catch { throw; }
            finally
            {
                if (command != null) { command.Dispose(); }
                if (adapter != null) { adapter.Dispose(); }

            }



        }

        public void CheckIn(UserSwipe entity)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;
            try
            {

                //Define the Command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                //command.CommandText = "UpdateUsersSwipe";

                command.CommandText = "UpdateUsersSwipe_new";

                //Assign the Parameters.
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = entity.GetXml();
                command.Parameters.Add("@EditBy", SqlDbType.VarChar).Value = entity.CustomData["EditBy"].ToString();
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;

                //Define the transaction
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // fill error datatable
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // commit
                transaction.Commit();

                // Get output parameters.
                bool hasError = bool.Parse(command.Parameters["@hasError"].Value.ToString());

                if (hasError)
                {
                    // Create exception instance.
                    //ValidationException exception = new ValidationException("Validation error(s) occurred.");
                    ValidationException exception = new ValidationException("");

                    if (errorDataTable != null)
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format("{0}", row["Value"].ToString()));
                        }
                        exception.Data.Add("IsExists", message);
                    }


                    //Throw exception if any.
                    throw exception;
                }

            }
            catch (ValidationException ve)
            {
                throw ve;

            }
            catch
            {

                if (transaction != null)
                    if (transaction.Connection != null)
                        //Rollback the transaction.
                        transaction.Rollback();

                throw;
            }
            finally
            {
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
                if (adapter != null) adapter.Dispose();
            }

        }

        public void CheckOut(UserSwipe entity)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;
            try
            {

                //Define the Command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateUsersSwipe";

                //Assign the Parameters.
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = entity.GetXml();
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;

                //Define the transaction
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // fill error datatable
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // commit
                transaction.Commit();

                // Get output parameters.
                bool hasError = bool.Parse(command.Parameters["@hasError"].Value.ToString());

                if (hasError)
                {
                    // Create exception instance.
                    //ValidationException exception = new ValidationException("Validation error(s) occurred.");
                    ValidationException exception = new ValidationException("");

                    if (errorDataTable != null)
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format("{0}", row["Value"].ToString()));
                        }
                        exception.Data.Add("IsExists", message);
                    }


                    //Throw exception if any.
                    throw exception;
                }

            }
            catch (ValidationException ve)
            {
                throw ve;
            }
            catch
            {

                if (transaction != null)
                    if (transaction.Connection != null)
                        //Rollback the transaction.
                        transaction.Rollback();

                throw;
            }
            finally
            {
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
                if (adapter != null) adapter.Dispose();
            }
        }

       

        public IAppManager AppManager
        {
            get
            {
                return this._appManager;

            }
            set
            {
                this._appManager = value;
                if (value != null)
                    mDbConnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
            }
        }

        public void Dispose()
        {
            try
            {
                // Dispose.
                if (mDbConnection != null)
                {
                    if (mDbConnection.State != ConnectionState.Closed)
                        mDbConnection.Close();
                    mDbConnection.Dispose();

                    // Cleare the sqlconnection pool.
                    SqlConnection.ClearPool(mDbConnection);
                    SqlConnection.ClearAllPools();
                }

            }
            catch { throw; }
        }


        public void RemoveUserSwipeCheckInOutDetailsint(int userId, DateTime workDate, int movedUserId, string movedreason)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            try
            {
                //Create a transaction instance.
                transaction = mDbConnection.BeginTransaction();

                //Create a command instance.
                command = mDbConnection.CreateCommand();

                //Assign the transaction to the command.
                command.Transaction = transaction;
                command.CommandText = "RemoveUserSwipeDeatils";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@workdate", SqlDbType.DateTime).Value = workDate;
                command.Parameters.Add("@moveduserid", SqlDbType.Int).Value = movedUserId;
                command.Parameters.Add("@movedReason", SqlDbType.VarChar).Value = movedreason;


                //Execute the query.
                command.ExecuteNonQuery();

                //Commit the Transaction.
                transaction.Commit();
            }
            catch {
                if (transaction != null)
                {
                    //Rollback the transaction.
                    transaction.Rollback();
                }
            }
            finally
            {
                // Dispose the object.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
                
            }
        }
    }
}
