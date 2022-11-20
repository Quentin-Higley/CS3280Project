using GroupProject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Items
{
    internal class clsItemLogic
    {
        /// <summary>
        /// sql helper object
        /// </summary>
        private SQLCommands conn;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="conn">sqlcommands object</param>
        /// <exception cref="Exception">excpetion</exception>
        public clsItemLogic(SQLCommands conn)
        {
            try
            {

                this.conn = conn;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// gets all items
        /// </summary>
        /// <returns>list of items</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Item> getItems()
        {
            try
            {
                List<Item> items = new List<Item>();

                DataTable dt = conn.execSql("getItems", null).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    Item i = new Item(dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString(), dr.ItemArray[2].ToString());
                    items.Add(i);
                }
                return items;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// update an item
        /// </summary>
        /// <param name="desc">item description</param>
        /// <param name="cost">item cost</param>
        /// <returns>index for messege</returns>
        public int updateItem(string desc, string cost, Item selected)
        {
            int updated = 0;
            try
            {
                string[] args = { desc, cost, selected.ItemeId };
                conn.execSql("updateItem", args);
                updated++;
                return updated;

            }
            catch (Exception ex)
            {
                return updated;
            }
        }

        /// <summary>
        /// add an item
        /// </summary>
        /// <param name="desc">description</param>
        /// <param name="cost">cost</param>
        /// <returns>index</returns>
        public int addItem(string desc, string cost)
        {
            int added = 0;
            try
            {
                DataTable dt = conn.execSql("getIndecies", null).Tables[0];
                List<string> idx = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    string l = dr[0].ToString();
                    idx.Add(l);
                }
                string sorted = idx.OrderByDescending(x => x.Length).ThenByDescending(x => x).ToList()[0];

                string[] args = { indexing(sorted), desc, cost };
                conn.execSql("addItem", args);
                added++;
                return added;

            }
            catch (Exception ex)
            {
                return added;
            }
        }

        /// <summary>
        /// indexing algorithm
        /// </summary>
        /// <param name="idx">the letter index</param>
        /// <returns>letter index</returns>
        private string indexing(string idx)
        {
            try
            {
                int trueIdx = 0;
                foreach (char c in idx)
                {
                    trueIdx = trueIdx * 26 + c - 'A' + 1;
                }
                trueIdx++;

                int cBase = 26;
                int max = 7;
                string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string newIdx = "";
                int offset = max;
                var sb = new StringBuilder().Append(' ', max);
                if (trueIdx <= cBase)
                    return digits[trueIdx].ToString();
                int current = trueIdx;
                while (current > 0)
                {
                    sb[--offset] = digits[--current % cBase];
                    current /= cBase;
                }
                return sb.ToString(offset, max - offset);
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// delete an item
        /// </summary>
        /// <returns>messege index</returns>
        public int deleteItem(string id)
        {
            int delete = 0;
            try
            {
                string[] args = { id };
                conn.execSql("singleInvoice", args);

                int count = int.Parse(conn.Scalar);
                if (count < 1)
                {
                    conn.execSql("deleteItem", args);
                    delete = 2;
                    return delete;
                }
                delete = 1;
                return delete;

            }
            catch (Exception ex)
            {
                return delete;
            }
        }
    }
}
