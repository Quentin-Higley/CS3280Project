using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;

namespace Assignment6AirlineReservation
{
    internal class Connection
    {
        /// <summary>
        /// single line return sql result
        /// </summary>
        string scalar;
        /// <summary>
        /// regular expression to replace {} in queries
        /// </summary>
        Regex replace;
        /// <summary>
        /// db connection
        /// </summary>
        private clsDataAccess conn;

        /// <summary>
        /// sql commands
        /// </summary>
        Dictionary<string, string> sql;

        /// <summary>
        /// connection constructor
        /// </summary>
        /// <exception cref="Exception">exception</exception>
        public Connection()
        {
            try
            {
                replace = new Regex(@"{(?<exp>[^}]+)}");
                conn = new clsDataAccess();
                sql = new Dictionary<string, string>();
                sql.Add("getFlights", "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT");

                sql.Add("getPassengers", "SELECT PASSENGER.Passenger_ID, First_Name, Last_Name, Seat_Number " +
                                         "FROM FLIGHT_PASSENGER_LINK, FLIGHT, PASSENGER " +
                                         "WHERE FLIGHT.FLIGHT_ID = FLIGHT_PASSENGER_LINK.FLIGHT_ID AND " +
                                         "FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID " +
                                         "AND FLIGHT.FLIGHT_ID = {flightID}");

                sql.Add("updateSeat", "UPDATE FLIGHT_PASSENGER_LINK " +
                                      "SET Seat_Number = {seatNumber} " +
                                      "WHERE FLIGHT_ID = {flightID} AND PASSENGER_ID = {passengerId}");

                sql.Add("addPassenger", "INSERT INTO PASSENGER(First_Name, Last_Name) VALUES('{firstName}','{lastName}')");

                sql.Add("insertLink", "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) VALUES( {flightId} , {passengerId} , {seatNumber})");

                sql.Add("deleteLink", "Delete FROM FLIGHT_PASSENGER_LINK WHERE FLIGHT_ID = {flightId} AND PASSENGER_ID = {passengerId}");

                sql.Add("deletePassenger", "Delete FROM PASSENGER WHERE PASSENGER_ID ={passengerId}");

                sql.Add("getPassengerId", "SELECT Passenger_ID from Passenger where First_Name = '{firstName}' AND Last_Name = '{lastName}'");
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// executes sql
        /// </summary>
        /// <param name="statement">sql statement</param>
        /// <param name="args">sql arguements</param>
        /// <returns>Data Set</returns>
        /// <exception cref="Exception">exception</exception>
        private DataSet executeSql(string statement, string[] args)
        {
            try
            {
                DataSet ds = new DataSet();
                string execute;
                int iRet = 0;
                execute = sql[statement];
                if (args != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        execute = replace.Replace(execute, args[i], 1);
                    }
                }
                if (statement == "getFlights" || statement == "getPassengers")
                    ds = conn.ExecuteSQLStatement(execute, ref iRet);
                else if (statement == "getPassengerId")
                     scalar = conn.ExecuteScalarSQL(execute);
                else
                    conn.ExecuteNonQuery(execute);
                return ds;
            }
            catch(Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// gets all the flights
        /// </summary>
        /// <returns>list of flights</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Flight> getFlights()
        {
            try
            {
                List<Flight> flights = new List<Flight>();
                string statement = "getFlights";
                DataSet ds = executeSql(statement, null);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Flight f = new Flight(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString());
                    flights.Add(f);
                }

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// gets all passengers on a flight
        /// </summary>
        /// <param name="flightId">flight id</param>
        /// <returns>list of passengers</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Passenger> getPassenger(string flightId)
        {
            try
            {
                List<Passenger> passengers = new List<Passenger>();
                string statement = "getPassengers";
                string[] args = { flightId };
                DataSet ds = executeSql(statement, args);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Passenger p = new Passenger(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString());
                    passengers.Add(p);
                }

                return passengers;
            }
            catch(Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// public function for executing sql
        /// </summary>
        /// <param name="statement">sql statement</param>
        /// <param name="args">sql arguements</param>
        /// <returns>DataSet</returns>
        /// <exception cref="Exception">exception</exception>
        public DataSet execSql(string statement, string[] args)
        {
            try
            {
                return executeSql(statement, args);
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        /// <summary>
        /// scalar getter
        /// </summary>
        public string Scalar { get { return scalar; } }
    }
}
